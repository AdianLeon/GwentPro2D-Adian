using System;
using System.Collections.Generic;

public partial class Parser
{
    public static EffectDeclaration ProcessEffectCode(string code)
    {
        Parser parser = new Parser(new Lexer(code).TokenizeCode());
        INode effectDeclaration = parser.ParseEffect();
        if (!parser.hasFailed && effectDeclaration != null) { return (EffectDeclaration)effectDeclaration; }
        return null;
    }
    private EffectDeclaration ParseEffect()
    {
        HashSet<string> propertiesToDeclare = new HashSet<string> { "Name", "Action" };
        IExpression<string> name = new StringValueExpression("");
        List<(string, VarType)> parameters = null;
        EffectAction effectAction = null;
        if (!Next().Is("{", true)) { hasFailed = true; return null; }
        bool expectingDeclaration = true;

        while (expectingDeclaration)
        {
            Token key = Next();
            if (!Next().Is(":", true)) { hasFailed = true; }
            Next();
            switch (key.Text)
            {
                case "Name":
                    if (!propertiesToDeclare.Contains("Name")) { Errors.Write("La propiedad 'Name' ya ha sido declarada", key); hasFailed = true; return null; }
                    if (!Current.Is(TokenType.literal, true)) { hasFailed = true; return null; }
                    name = ParseStringExpression();
                    if (hasFailed) { return null; }
                    propertiesToDeclare.Remove("Name");
                    break;
                case "Params":
                    if (parameters != null) { Errors.Write("La propiedad 'Params' ya ha sido declarada", key); hasFailed = true; return null; }
                    if (!propertiesToDeclare.Contains("Action")) { Errors.Write("La propiedad 'Params' debe declararse antes que 'Action'", key); hasFailed = true; return null; }
                    parameters = new List<(string, VarType)>();
                    if (!Current.Is("{", true)) { hasFailed = true; return null; }
                    bool expectingParamDeclaration = true;
                    while (expectingParamDeclaration)
                    {
                        if (!Next().Is(TokenType.identifier, true)) { Errors.Write("'" + Current.Text + "' no es un nombre valido para una variable"); hasFailed = true; return null; }
                        string varName = Current.Text;
                        if (!Next().Is(":", true)) { hasFailed = true; return null; }
                        if (!(Next().Is("Number") || Current.Is("Bool") || Current.Is("String"))) { Errors.Write("Se esperaba un tipo de variable valido. ('Number', 'Bool' o 'String'), se encontro: " + Current.Text); hasFailed = true; return null; }
                        parameters.Add(new(varName, (VarType)Enum.Parse(typeof(VarType), Current.Text)));
                        expectingParamDeclaration = Next().Is(",");
                    }
                    if (!Current.Is("}", true)) { hasFailed = true; return null; }
                    break;
                case "Action":
                    if (!propertiesToDeclare.Contains("Action")) { Errors.Write("La propiedad 'Action' ya ha sido declarada", key); hasFailed = true; return null; }
                    effectAction = (EffectAction)ParseEffectAction(parameters);
                    if (hasFailed) { return null; }
                    propertiesToDeclare.Remove("Action");
                    break;
                default:
                    Errors.Write("Se esperaba una declaracion de propiedad de efecto. Declaraciones de propiedad validas son las siguientes: Name', 'Params' y 'Action'", key); hasFailed = true; return null;
            }
            expectingDeclaration = Next().Is(",");
        }
        if (propertiesToDeclare.Count > 0) { Errors.Write("Han faltado por declarar las siguientes propiedades: " + propertiesToDeclare.FlattenText(), Current); hasFailed = true; return null; }
        if (!Current.Is("}")) { Errors.Write("Se esperaba '}' en vez de '" + Current.Text + "', puede ser que hayas olvidado la coma antes de la declaracion"); hasFailed = true; return null; }
        return new EffectDeclaration(name, parameters, effectAction);
    }
}
