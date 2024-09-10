using Unity.VisualScripting;
using UnityEngine;

public class StringExpressionsParser : Parser
{
    public override INode ParseTokens() { IExpression<string> expression = ParseOperation(); Next(-1); return expression; }
    private IExpression<string> ParseOperation()
    {
        IExpression<string> left = ParseStringValue(); if (hasFailed) { return null; }
        while (Current.Is("@") || Current.Is("@@"))
        {
            Token op = Current; Next();
            var right = ParseStringValue(); if (hasFailed) { return null; }
            left = new StringExpression(left, op, right);
        }
        return left;
    }
    private IExpression<string> ParseStringValue()
    {
        Debug.Log("Parseando string");
        IExpression<string> left;
        if (Current.Is(TokenType.literal)) { left = new StringValueExpression(Current.Text); Next(); }
        else if (Current.Is(TokenType.identifier) && VariableScopes.ContainsVar(Current.Text) && Current.Text.ScopeValue().Type == VarType.String)
        {
            Debug.Log("Variable declarada que contiene a string");
            VariableReference variableReference;
            if (!Try(new VariableParser().ParseTokens, out variableReference)) { throw new System.Exception("Se suponia que existia una referencia a un string"); }
            left = new StringVariableReference(variableReference);
            Next();
        }
        else if (!Try(ParseSemiGeneric, out left)) { hasFailed = true; return null; }
        Debug.Log("Terminando de parsear string: " + Current);
        return left;
    }
}
