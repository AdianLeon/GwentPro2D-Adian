using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static partial class Parser
{
    public static CardDeclaration ProcessCardCode(string code)
    {
        StartParsing(Lexer.TokenizeCode(code));
        INode cardDeclaration = ParseCard();
        if (!hasFailed && cardDeclaration != null) { return (CardDeclaration)cardDeclaration; }
        return null;
    }
    private static CardDeclaration ParseCard()
    {
        HashSet<string> propertiesToDeclare = new HashSet<string> { "Type", "Name", "Faction" };
        IExpression<string> type = new StringValueExpression("");
        IExpression<string> name = new StringValueExpression("");
        IExpression<string> description = new StringValueExpression("");
        IExpression<int> totalCopies = new NumberExpression("1");
        IExpression<string> faction = new StringValueExpression("");
        IExpression<int> power = new NumberExpression("0");
        UnitCardZone range = UnitCardZone.None;
        OnActivation onActivation = null;
        if (!Next().Is("{", true)) { hasFailed = true; return null; }
        bool expectingDeclaration = true;
        while (expectingDeclaration)
        {
            Token key = Next();
            if (!Next().Is(":", true)) { hasFailed = true; return null; }
            Next();
            switch (key.Text)
            {
                case "Description":
                    if (description.Evaluate() != "") { Errors.Write("La propiedad 'Description' ya ha sido declarada", key); hasFailed = true; return null; }
                    description = ParseStringExpression();
                    if (hasFailed) { return null; }
                    break;
                case "TotalCopies":
                    if (totalCopies.Evaluate() > 1) { Errors.Write("La propiedad 'ClonesAmount' ya ha sido declarada", key); hasFailed = true; return null; }
                    totalCopies = ParseArithmeticExpression();
                    if (totalCopies.Evaluate() < 2) { Errors.Write("El numero asociado a 'TotalCopies' no es valido. Intente con un numero entre 2 y " + int.MaxValue, Current); }
                    if (hasFailed) { return null; }
                    break;
                case "Type":
                    if (!propertiesToDeclare.Contains("Type")) { Errors.Write("La propiedad 'Type' ya ha sido declarada", key); hasFailed = true; return null; }
                    type = ParseStringExpression();
                    if (hasFailed) { return null; }
                    HashSet<string> allTypes = new HashSet<string> { "Oro", "Plata", "Clima", "Despeje", "Aumento", "Senuelo", "Lider" };
                    string evaluatedType = type.Evaluate();
                    if (!allTypes.Contains(evaluatedType)) { Errors.Write("El tipo de carta '" + evaluatedType + "' no esta definido. Los tipos definidos son: " + allTypes.FlattenText(), key); hasFailed = true; return null; }
                    if (evaluatedType == "Oro" || evaluatedType == "Plata" || evaluatedType == "Clima" || evaluatedType == "Aumento") { propertiesToDeclare.Add("Power"); }
                    if (evaluatedType == "Oro" || evaluatedType == "Plata") { propertiesToDeclare.Add("Range"); }
                    propertiesToDeclare.Remove("Type");
                    break;
                case "Name":
                    if (!propertiesToDeclare.Contains("Name")) { Errors.Write("La propiedad 'Name' ya ha sido declarada", key); hasFailed = true; return null; }
                    name = ParseStringExpression();
                    if (hasFailed) { return null; }
                    propertiesToDeclare.Remove("Name");
                    break;
                case "Faction":
                    if (!propertiesToDeclare.Contains("Faction")) { Errors.Write("La propiedad 'Faction' ya ha sido declarada", key); hasFailed = true; return null; }
                    faction = ParseStringExpression();
                    if (hasFailed) { return null; }
                    propertiesToDeclare.Remove("Faction");
                    break;
                case "Power":
                    if (!propertiesToDeclare.Contains("Power")) { Errors.Write("No se puede declarar la propiedad 'Power'. Solo se debe declarar una vez y en caso de que la propiedad 'Type' previamente declarada sea Oro, Plata, Clima o Aumento", key); hasFailed = true; return null; }
                    power = ParseArithmeticExpression();
                    if (hasFailed) { return null; }
                    propertiesToDeclare.Remove("Power");
                    break;
                case "Range":
                    if (!propertiesToDeclare.Contains("Range")) { Errors.Write("No se puede declarar la propiedad 'Range'. Solo se debe declarar una vez y en caso de que el tipo  de carta previamente declarado sea de Plata u Oro", key); hasFailed = true; return null; }
                    range = GetRange();
                    if (hasFailed) { return null; }
                    propertiesToDeclare.Remove("Range");
                    break;
                case "OnActivation":
                    if (onActivation != null) { Errors.Write("La propiedad 'OnActivation' ya ha sido declarada", key); hasFailed = true; return null; }
                    onActivation = (OnActivation)ParseOnActivation();
                    if (hasFailed) { return null; }
                    break;
                default:
                    Errors.Write("Se esperaba una declaracion de propiedad de carta. Declaraciones de propiedad validas son las siguientes: 'Type', 'Name', 'Faction', 'Power', 'Range', 'TotalCopies', 'Description' y 'OnActivation'", key); hasFailed = true; return null;
            }
            expectingDeclaration = Next().Is(",");
        }
        if (propertiesToDeclare.Count > 0) { Errors.Write("Han faltado por declarar las siguientes propiedades: " + propertiesToDeclare.FlattenText(), Current); hasFailed = true; return null; }
        if (!Current.Is("}")) { Errors.Write("Se esperaba '}' en vez de '" + Current.Text + "', puede ser que hayas olvidado la coma antes de la declaracion"); hasFailed = true; return null; }
        return new CardDeclaration(name, type, description, totalCopies, faction, power, range, onActivation);
    }
    private static UnitCardZone GetRange()
    {
        if (!Current.Is("[", true)) { hasFailed = true; return default; }
        string rangeText = "";
        bool expectingRange = true;
        while (expectingRange)
        {
            if (!Next().Is(TokenType.literal, true)) { hasFailed = true; return default; }
            string aux = Current.Text;
            if (!(aux == "Melee" || aux == "Ranged" || aux == "Siege")) { Errors.Write("El rango: '" + aux + "' no esta definido. Los valores definidos son 'Melee', 'Ranged' y 'Siege'", Current); hasFailed = true; return default; }
            if (rangeText.Contains(aux[0])) { Errors.Write("Has repetido el rango " + Current.Text + " declarado en 'Range'", Current); hasFailed = true; return default; }
            rangeText += aux[0];
            expectingRange = Next().Is(",");
        }
        if (!Current.Is("]", true)) { hasFailed = true; return default; };
        return (UnitCardZone)Enum.Parse(typeof(UnitCardZone), rangeText);
    }
}