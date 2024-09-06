using System;

public class ComparisonExpressionsParser : Parser
{
    public override INode ParseTokens() => ParseOperation();
    private IExpression<bool> ParseOperation()
    {
        IExpression<IComparable> left = ParseComparisonValue(); if (hasFailed) { return null; }
        if (Current.Is("==") || Current.Is("!=") || Current.Is("<") || Current.Is(">") || Current.Is("<=") || Current.Is(">="))
        {
            Token op = Current; Next();
            var right = ParseComparisonValue(); if (hasFailed) { return null; }
            return new ComparisonExpression(left, op, right);
        }
        else { Errors.Write("Se esperaba operador de comparacion, en cambio se encontro '" + Current.Text + "'", Current); hasFailed = true; return null; }
    }
    private IExpression<IComparable> ParseComparisonValue()
    {
        IExpression<IComparable> left;
        if (Current.Is(TokenType.number)) { left = new ComparisonValueExpression(Current.Text); Next(); }
        else { Errors.Write(Current); hasFailed = true; return null; }
        return left;
    }
}
