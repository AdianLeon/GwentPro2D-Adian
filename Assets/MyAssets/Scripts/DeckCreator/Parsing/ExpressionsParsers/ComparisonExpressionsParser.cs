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
            Debug.Log("Recibido operador: " + op);
            IExpression<IReference> right = ParseComparisonValue(); if (hasFailed) { return null; }
            if (left.GetType() != right.GetType()) { throw new Exception("Comparison Expression with mismatching types"); }
            return new ComparisonExpression(left, op, right);
        }
        else { hasFailed = true; return null; }
    }
    private IExpression<IReference> ParseComparisonValue()
    {
        Debug.Log("Comienzo a parsear expresion para comparar");
        IReference reference = ParseExpression(false);
        if (hasFailed) { Debug.Log("Se esperaba una referencia. Token: " + Current); return null; }
        Next();
        Debug.Log("Termino de parsear expresion para comparar");
        return new ComparisonValueExpression(reference);
    }
}
