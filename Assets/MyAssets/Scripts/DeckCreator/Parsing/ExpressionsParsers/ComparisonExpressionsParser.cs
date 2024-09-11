using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Parser
{
    private static IExpression<bool> ParseComparisonExpression() { IExpression<bool> expression = ParseComparisonOperation(); Next(-1); return expression; }
    private static IExpression<bool> ParseComparisonOperation()
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
    private static IExpression<IReference> ParseComparisonValue()
    {
        Debug.Log("Comienzo a parsear expresion para comparar");
        IReference reference;
        if (!Try(ParseBooleanExpression, out reference, false) && !Try(ParseArithmeticExpression, out reference, false) && !Try(ParseStringExpression, out reference, false))
        {
            Debug.Log("Se esperaba una referencia. Token: " + Current); hasFailed = true; return null;
        }
        Next();
        Debug.Log("Termino de parsear expresion para comparar");
        return new ComparisonValueExpression(reference);
    }
}
