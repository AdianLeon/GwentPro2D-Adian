using System;
using UnityEngine;

public class ComparisonExpressionsParser : Parser
{
    public override INode ParseTokens() { IExpression<bool> expression = ParseOperation(); Next(-1); return expression; }
    private IExpression<bool> ParseOperation()
    {
        IExpression<IComparable> left = ParseComparisonValue(); if (hasFailed) { return null; }
        if (Current.Is("==") || Current.Is("!=") || Current.Is("<") || Current.Is(">") || Current.Is("<=") || Current.Is(">="))
        {
            Token op = Current; Next();
            IExpression<IComparable> right = ParseComparisonValue(); if (hasFailed) { return null; }
            if (left.GetType() != right.GetType()) { Debug.Log("Comparison Expression with mismatching types"); }
            return new ComparisonExpression(left, op, right);
        }
        else { Errors.Write("Se esperaba operador de comparacion, en cambio se encontro '" + Current.Text + "'", Current); hasFailed = true; return null; }
    }
    private IExpression<IComparable> ParseComparisonValue()
    {
        IExpression<IComparable> left;
        if (Current.Is(TokenType.number)) { left = new ComparisonValueExpression(Current.Text); Next(); }
        else if (Current.Is(TokenType.literal)) { left = new ComparisonValueExpression(Current.Text); Next(); }
        else { Errors.Write("Se esperaba una expresion comparativa en vez de " + Current.Text, Current); hasFailed = true; return null; }
        return left;
    }
}
