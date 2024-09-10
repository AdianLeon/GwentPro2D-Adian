
public class BooleanExpressionsParser : Parser
{
     public override INode ParseTokens() { IExpression<bool> expression = ParseOperation(); Next(-1); return expression; }
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
          if (Current.Is("true") || Current.Is("false")) { left = new BooleanValueExpression(Current.Text); Next(); }
          else if (Current.Is("("))
          {
               Next();
               left = ParseOperation(); if (hasFailed) { return null; }
               if (!Current.Is(")", true)) { hasFailed = true; return null; }
               Next();
          }
          else if (Current.Is(TokenType.identifier) && VariableScopes.ContainsVar(Current.Text) && Current.Text.ScopeValue().Type == VarType.Boolean)
          {
               VariableReference variableReference;
               if (!Try(new VariableParser().ParseTokens, out variableReference)) { throw new System.Exception("Se suponia que existia una referencia a un booleano"); }
               left = new BooleanVariableReference(variableReference);
               Next();
          }
          else if (!Try(ParseSemiGeneric, out left)) { hasFailed = true; return null; }

          return left;
     }
}
