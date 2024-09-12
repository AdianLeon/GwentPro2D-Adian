using UnityEngine;

public static partial class Parser
{
    private static IExpression<int> ParseArithmeticExpression() { IExpression<int> expression = ParseArithmeticOperation(); Next(-1); Debug.Log("Returns arithmetic expression!"); return expression; }
    private static IExpression<int> ParseArithmeticOperation(IExpression<int> left = null) => ParseSum(left);
    private static IExpression<int> ParseSum(IExpression<int> left = null)
    {
        left = ParseMultiplication(); if (hasFailed) { return null; }
        while (Current.Is("+") || Current.Is("-"))
        {
            Token op = Current; Next();
            var right = ParseMultiplication(); if (hasFailed) { return null; }
            left = new ArithmeticExpression(left, op, right);
        }
        return left;
    }
    private static IExpression<int> ParseMultiplication(IExpression<int> left = null)
    {
        if (left == null) { left = ParseNumber(); if (hasFailed) { return null; } }
        while (Current.Is("*") || Current.Is("/"))
        {
            Token op = Current; Next();
            IExpression<int> right = ParseNumber(); if (hasFailed) { return null; }
            left = new ArithmeticExpression(left, op, right);
        }
        return left;
    }
    private static IExpression<int> ParseNumber()
    {
        IExpression<int> left;
        if (Current.Is("-") && Peek().Is(TokenType.number)) { left = new NumberExpression(Current.Text + Next().Text); Next(); }
        else if (Current.Is(TokenType.number)) { left = new NumberExpression(Current.Text); Next(); }
        else if (Current.Is("("))
        {
            Next();
            left = ParseSum(); if (hasFailed) { return null; }
            if (!Current.Is(")", true)) { hasFailed = true; return null; }
            Next();
        }
        else if (Current.Is(TokenType.identifier))
        {
            IReference reference;
            if (!Try(ParseVariable, out reference) || reference.Type != VarType.Number) { Errors.Write("Se esperaba una referencia a un numero", Current); hasFailed = true; return null; }
            else if (reference is VariableReference) { left = new NumberVariableReference((VariableReference)reference); }
            else if (reference is CardPropertyReference) { left = new PowerCardPropertyExpression((CardPropertyReference)reference); }
            else { throw new System.NotImplementedException(); }
            Next();
        }
        else if (!Try(ParseVariable, out left)) { hasFailed = true; return null; }

        while (Current.Is("^"))
        {
            Token op = Current; Next();
            IExpression<int> right = ParseNumber(); if (hasFailed) { return null; }
            left = new ArithmeticExpression(left, op, right);
        }
        return left;
    }
}