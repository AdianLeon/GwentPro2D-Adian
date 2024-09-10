
using UnityEngine;

public class ArithmeticExpressionsParser : Parser
{
    public override INode ParseTokens() { IExpression<int> expression = ParseSum(); Next(-1); return expression; }
    private IExpression<int> ParseSum()
    {
        IExpression<int> left = ParseMultiplication(); if (hasFailed) { return null; }
        while (Current.Is("+") || Current.Is("-"))
        {
            Token op = Current; Next();
            var right = ParseMultiplication(); if (hasFailed) { return null; }
            left = new ArithmeticExpression(left, op, right);
        }
        return left;
    }
    private IExpression<int> ParseMultiplication()
    {
        IExpression<int> left = ParseNumber(); if (hasFailed) { return null; }
        while (Current.Is("*") || Current.Is("/"))
        {
            Token op = Current; Next();
            IExpression<int> right = ParseNumber(); if (hasFailed) { return null; }
            left = new ArithmeticExpression(left, op, right);
        }
        return left;
    }
    private IExpression<int> ParseNumber()
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
        else if (Current.Is(TokenType.identifier) && VariableScopes.ContainsVar(Current.Text) && Current.Text.ScopeValue().Type == VarType.Number)
        {
            VariableReference variableReference;
            if (!Try(new VariableParser().ParseTokens, out variableReference)) { throw new System.Exception("Se suponia que existia una referencia a un numero"); }
            left = new NumberVariableReference(variableReference);
            Next();
        }
        else if (!Try(ParseSemiGeneric, out left)) { hasFailed = true; return null; }

        while (Current.Is("^"))
        {
            Token op = Current; Next();
            IExpression<int> right = ParseNumber(); if (hasFailed) { return null; }
            left = new ArithmeticExpression(left, op, right);
        }
        return left;
    }
}