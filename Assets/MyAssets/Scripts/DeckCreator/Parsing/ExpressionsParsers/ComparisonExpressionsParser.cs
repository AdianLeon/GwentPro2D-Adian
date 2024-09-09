using System;
using UnityEngine;

public class ComparisonExpressionsParser : Parser
{
    public override INode ParseTokens() { IExpression<bool> expression = ParseOperation(); Next(-1); return expression; }
    private IExpression<bool> ParseOperation()
    {
        IExpression<IReference> left = ParseComparisonValue(); if (hasFailed) { return null; }
        if (Current.Is("==") || Current.Is("!=") || Current.Is("<") || Current.Is(">") || Current.Is("<=") || Current.Is(">="))
        {
            Token op = Current; Next();
            IExpression<IReference> right = ParseComparisonValue(); if (hasFailed) { return null; }
            if (left.GetType() != right.GetType()) { Debug.Log("Comparison Expression with mismatching types"); }
            return new ComparisonExpression(left, op, right);
        }
        else { Errors.Write("Se esperaba operador de comparacion, en cambio se encontro '" + Current.Text + "'", Current); hasFailed = true; return null; }
    }
    private IExpression<IReference> ParseComparisonValue()
    {
        IReference reference;
        if (!TryParse(out reference) || reference is IReference) { Errors.Write("Se esperaba una referencia"); hasFailed = true; return null; }
        Next();
        return new ComparisonValueExpression(reference);
    }
}
