
public class BooleanExpressionsParser : Parser
{
     public override INode ParseTokens() => ParseOperation();
     private IExpression<bool> ParseOperation()
     {
          IExpression<bool> left = ParseBoolValue(); if (hasFailed) { return null; }
          while (Current.Is("&&") || Current.Is("||"))
          {
               Token op = Current; Next();
               var right = ParseBoolValue(); if (hasFailed) { return null; }
               left = new BooleanExpression(left, op, right);
          }
          return left;
     }
     private IExpression<bool> ParseBoolValue()
     {
          IExpression<bool> left;
          if (Current.Is(TokenType.boolean)) { left = new BooleanValueExpression(Current.Text); Next(); }
          else if (Current.Is("("))
          {
               Next();
               left = ParseOperation(); if (hasFailed) { return null; }
               if (!Current.Is(")", true)) { hasFailed = true; return null; }
               Next();
          }
          else { Errors.Write(Current); hasFailed = true; return null; }

          return left;
     }
}
