using UnityEngine;

public static partial class Parser
{
    private static IExpression<string> ParseStringExpression() { IExpression<string> expression = ParseStringOperation(); Next(-1); return expression; }
    private static IExpression<string> ParseStringOperation()
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
    private static IExpression<string> ParseStringValue()
    {
        IExpression<string> left;
        if (Current.Is(TokenType.literal)) { left = new StringValueExpression(Current.Text); Next(); }
        else if (Current.Is(TokenType.identifier) && VariableScopes.ContainsVar(Current.Text) && Current.Text.ScopeValue().Type == VarType.String)
        {
            VariableReference variableReference;
            if (!Try(ParseVariable, out variableReference)) { throw new System.Exception("Se suponia que existia una referencia a un string"); }
            left = new StringVariableReference(variableReference);
            Next();
        }
        else if (!Try(ParseVariable, out left)) { hasFailed = true; return null; }
        return left;
    }
}
