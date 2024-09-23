using UnityEngine;

public partial class Parser
{
    private IExpression<bool> ParseComparisonExpression() { IExpression<bool> expression = ParseComparisonOperation(); Next(-1); return expression; }
    private IExpression<bool> ParseComparisonOperation(IExpression<IReference> left = null)
    {
        if (left == null) { left = ParseComparisonValue(); if (hasFailed) { return null; } }
        if (Current.Is("==") || Current.Is("!=") || Current.Is("<") || Current.Is(">") || Current.Is("<=") || Current.Is(">="))
        {
            string op = Current.Text; Next();
            IExpression<IReference> right = ParseComparisonValue(); if (hasFailed) { return null; }
            if (left.Evaluate().Type != right.Evaluate().Type) { Errors.Write("Se esperaba una comparacion entre referencias del mismo tipo", Current); hasFailed = true; return null; }
            return new ComparisonExpression(left, op, right);
        }
        else { hasFailed = true; return null; }
    }
    private IExpression<IReference> ParseComparisonValue()
    {
        IReference reference;
        if (!Try(ParseBooleanExpression, out reference, false) && !Try(ParseArithmeticExpression, out reference, false) && !Try(ParseStringExpression, out reference, false)) { Errors.Write("Se esperaba una referencia", Current); hasFailed = true; return null; }
        Next();
        return new ComparisonValueExpression(reference);
    }
}
