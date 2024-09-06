
public class StringExpressionsParser : Parser
{
    public override INode ParseTokens() => ParseOperation();
    private IExpression<string> ParseOperation()
    {
        IExpression<string> left = ParseStringValue(); if (hasFailed) { return null; }
        while (Current.Is("@") || Current.Is("@@"))
        {
            Token op = Current; Next();
            var right = ParseStringValue(); if (hasFailed) { return null; }
            left = new StringExpression(left, op, right);
        }
        return left;
    }
    private IExpression<string> ParseStringValue()
    {
        if (Current.Is(TokenType.literal)) { Next(); return new StringValueExpression(Peek(-1).Text); }
        else { Errors.Write(Current); hasFailed = true; return null; }
    }
}
