using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardParser : Parser
{
    private Dictionary<string, string> cardTypes = new Dictionary<string, string>()
    {
        {"Oro","GoldCard"},
        {"Plata","SilverCard"},
        {"Clima","WeatherCard"},
        {"Despeje","ClearWeatherCard"},
        {"Aumento","BoostCard"},
        {"Senuelo","BaitCard"},
        {"Lider","LeaderCard"}
    };
    public static CardDeclaration ProcessCode(string code)
    {
        StartParsing(Lexer.TokenizeCode(code));
        INode cardDeclaration = new CardParser().ParseTokens();
        if (cardDeclaration != null) { return (CardDeclaration)cardDeclaration; }
        return null;
    }
    public override INode ParseTokens()
    {
        HashSet<string> propertiesToDeclare = new HashSet<string> { "Type", "Name", "Faction" };
        string type = "";
        string name = "";
        string description = "";
        string faction = "";
        int power = 0;
        UnitCardZone range = UnitCardZone.None;
        OnActivation onActivation = null;
        if (!Next().Is("{", true)) { hasFailed = true; return null; }
        bool expectingDeclaration = true;
        while (expectingDeclaration)
        {
            Token key = Next();
            if (!key.Is(TokenType.assignment, true)) { hasFailed = true; return null; }
            if (!Next().Is(":", true)) { hasFailed = true; return null; }
            Next();
            switch (key.Text)
            {
                case "Description":
                    if (description != "") { Errors.Write("La propiedad 'Description' ya ha sido declarada", key); hasFailed = true; return null; }
                    if (!Current.Is(TokenType.literal, true)) { hasFailed = true; return null; }
                    description = Current.Text;
                    break;
                case "Type":
                    if (!propertiesToDeclare.Contains("Type")) { Errors.Write("La propiedad 'Type' ya ha sido declarada", key); hasFailed = true; return null; }
                    if (!Current.Is(TokenType.literal, true)) { hasFailed = true; return null; }
                    if (!cardTypes.ContainsKey(Current.Text)) { Errors.Write("El tipo de carta '" + Current.Text + "' no esta definido. Los tipos definidos son: 'Oro', 'Plata', 'Clima', 'Despeje', 'Aumento', 'Senuelo' o 'Lider'"); hasFailed = true; return null; }
                    type = cardTypes[Current.Text];
                    if (Current.Text == "Oro" || Current.Text == "Plata" || Current.Text == "Clima" || Current.Text == "Aumento") { propertiesToDeclare.Add("Power"); }
                    if (Current.Text == "Oro" || Current.Text == "Plata") { propertiesToDeclare.Add("Range"); }
                    propertiesToDeclare.Remove("Type");
                    break;
                case "Name":
                    if (!propertiesToDeclare.Contains("Name")) { Errors.Write("La propiedad 'Name' ya ha sido declarada", key); hasFailed = true; return null; }
                    if (!Current.Is(TokenType.literal, true)) { hasFailed = true; return null; }
                    name = Current.Text;
                    propertiesToDeclare.Remove("Name");
                    break;
                case "Faction":
                    if (!propertiesToDeclare.Contains("Faction")) { Errors.Write("La propiedad 'Faction' ya ha sido declarada", key); hasFailed = true; return null; }
                    if (!Current.Is(TokenType.literal, true)) { hasFailed = true; return null; }
                    faction = Current.Text;
                    propertiesToDeclare.Remove("Faction");
                    break;
                case "Power":
                    if (!propertiesToDeclare.Contains("Power")) { Errors.Write("No se puede declarar la propiedad 'Power'. Solo se debe declarar una vez y en caso de que la propiedad 'Type' previamente declarada sea Oro, Plata, Clima o Aumento", key); hasFailed = true; return null; }
                    if (!Current.Is(TokenType.number, true)) { hasFailed = true; return null; }
                    power = int.Parse(Current.Text);
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
                    onActivation = (OnActivation)new OnActivationParser().ParseTokens();
                    if (hasFailed) { return null; }
                    break;
                default:
                    Errors.Write("Se esperaba una declaracion de propiedad de carta. Declaraciones de propiedad validas son las siguientes: 'Type', 'Name', 'Faction', 'Power', 'Range', 'ClonesAmount', 'Description' y 'OnActivation'", key); hasFailed = true; return null;
            }
            expectingDeclaration = Next().Is(",");
        }
        if (propertiesToDeclare.Count > 0) { Errors.Write("Han faltado por declarar las siguientes propiedades: " + propertiesToDeclare.GetText(), Current); hasFailed = true; return null; }
        if (!Current.Is("}")) { Errors.Write("Se esperaba '}' en vez de '" + Current.Text + "', puede ser que hayas olvidado la coma antes de la declaracion"); hasFailed = true; return null; }
        Next();
        return new CardDeclaration(name, type, description, faction, power, range, onActivation);
    }
    private UnitCardZone GetRange()
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