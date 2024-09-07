using UnityEngine;

public class StringExpressionsParser : Parser
{
    public override INode ParseTokens() { IExpression<string> expression = ParseOperation(); Next(-1); return expression; }
    private IExpression<string> ParseOperation()
    {
        IExpression<string> left = ParseStringValue(); if (hasFailed) { return null; }
        while (Next().Is("@") || Current.Is("@@"))
        {
            Token op = Current; Next();
            var right = ParseStringValue(); if (hasFailed) { return null; }
            left = new StringExpression(left, op, right);
        }
        return left;
    }
    private IExpression<string> ParseStringValue()
    {
        if (!Current.Is(TokenType.literal, true)) { hasFailed = true; return null; }
        return new StringValueExpression(Current.Text);
    }
}
