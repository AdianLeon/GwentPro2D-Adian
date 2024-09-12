using UnityEngine;

public static partial class Parser
{
    private static IExpression<string> ParseStringExpression() { IExpression<string> expression = ParseStringOperation(); Next(-1); return expression; }
    private static IExpression<string> ParseStringOperation(IExpression<string> left = null)
    {
        if (left == null) { left = ParseStringValue(); if (hasFailed) { return null; } }
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
        else if (Current.Is(TokenType.identifier))
        {
            IReference reference;
            if (!Try(ParseVariable, out reference) || reference.Type != VarType.String) { Errors.Write("Se esperaba una referencia a un string", Current); hasFailed = true; return null; }
            else if (reference is VariableReference) { left = new StringVariableReference((VariableReference)reference); }
            else if (reference is CardPropertyReference) { left = new StringCardPropertyExpression((CardPropertyReference)reference); }
            else { throw new System.NotImplementedException(); }
            Next();
        }
        else if (!Try(ParseVariable, out left)) { hasFailed = true; return null; }
        return left;
    }
}
