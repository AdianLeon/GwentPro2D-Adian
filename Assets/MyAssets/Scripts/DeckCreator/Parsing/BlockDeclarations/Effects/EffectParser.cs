using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParser : Parser
{
    public static EffectDeclaration ProcessCode(string code)
    {
        StartParsing(Lexer.TokenizeCode(code));
        INode effectDeclaration = new EffectParser().ParseTokens();
        if (!hasFailed && effectDeclaration != null) { return (EffectDeclaration)effectDeclaration; }
        return null;
    }
    public override INode ParseTokens()
    {
        HashSet<string> propertiesToDeclare = new HashSet<string> { "Name", "Action" };
        IExpression<string> name = new StringValueExpression("");
        EffectAction effectAction = null;
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
                case "Name":
                    if (!propertiesToDeclare.Contains("Name")) { Errors.Write("La propiedad 'Name' ya ha sido declarada", key); hasFailed = true; return null; }
                    if (!Current.Is(TokenType.literal, true)) { hasFailed = true; return null; }
                    name = (IExpression<string>)new StringExpressionsParser().ParseTokens();
                    if (hasFailed) { return null; }
                    propertiesToDeclare.Remove("Name");
                    break;
                case "Action":
                    if (!propertiesToDeclare.Contains("Action")) { Errors.Write("La propiedad 'Action' ya ha sido declarada", key); hasFailed = true; return null; }
                    effectAction = (EffectAction)new EffectActionParser().ParseTokens();
                    if (hasFailed) { return null; }
                    propertiesToDeclare.Remove("Action");
                    break;
                default:
                    Errors.Write("Se esperaba una declaracion de propiedad de efecto. Declaraciones de propiedad validas son las siguientes: Name'" +/*, 'Params'*/" y 'Action'", key); hasFailed = true; return null;
            }
            expectingDeclaration = Next().Is(",");
        }

        if (propertiesToDeclare.Count > 0) { Errors.Write("Han faltado por declarar las siguientes propiedades: " + propertiesToDeclare.GetText(), Current); hasFailed = true; return null; }
        if (!Current.Is("}")) { Errors.Write("Se esperaba '}' en vez de '" + Current.Text + "', puede ser que hayas olvidado la coma antes de la declaracion"); hasFailed = true; return null; }
        return new EffectDeclaration(name, effectAction);
    }
}
