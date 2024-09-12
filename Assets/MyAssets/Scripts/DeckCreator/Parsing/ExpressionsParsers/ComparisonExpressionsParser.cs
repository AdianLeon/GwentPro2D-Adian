using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Parser
{
    private static IExpression<bool> ParseComparisonExpression() { IExpression<bool> expression = ParseComparisonOperation(); Next(-1); return expression; }
    private static IExpression<bool> ParseComparisonOperation(IExpression<IReference> left = null)
    {
        if (left == null) { left = ParseComparisonValue(); if (hasFailed) { return null; } }
        if (Current.Is("==") || Current.Is("!=") || Current.Is("<") || Current.Is(">") || Current.Is("<=") || Current.Is(">="))
        {
            Token op = Current; Next();
            Debug.Log("Recibido operador: " + op);
            IExpression<IReference> right = ParseComparisonValue(); if (hasFailed) { return null; }
            if (left.Evaluate().Type != right.Evaluate().Type) { Errors.Write("Se esperaba una comparacion entre referencias del mismo tipo", Current); hasFailed = true; return null; }
            return new ComparisonExpression(left, op, right);
        }
        else { hasFailed = true; return null; }
    }
    private static IExpression<IReference> ParseComparisonValue()
    {
        IReference reference;
        if (!Try(ParseBooleanExpression, out reference, false) && !Try(ParseArithmeticExpression, out reference, false) && !Try(ParseStringExpression, out reference, false)) { Errors.Write("Se esperaba una referencia", Current); hasFailed = true; return null; }
        Next();
        return new ComparisonValueExpression(reference);
    }
}
