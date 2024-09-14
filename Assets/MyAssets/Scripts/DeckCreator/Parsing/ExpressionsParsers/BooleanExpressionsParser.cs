using System;
using UnityEngine;
public partial class Parser
{
     private IExpression<bool> ParseBooleanExpression() { IExpression<bool> expression = ParseBooleanOperation(); Next(-1); return expression; }
     private IExpression<bool> ParseBooleanOperation(IExpression<bool> left = null)
     {
          if (left == null) { left = ParseBoolValue(); if (hasFailed) { return null; } }
          while (Current.Is("&&") || Current.Is("||"))
          {
               string op = Current.Text; Next();
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
               left = ParseBooleanOperation(); if (hasFailed) { return null; }
               if (!Current.Is(")", true)) { hasFailed = true; return null; }
               Next();
          }
          else if (Current.Is(TokenType.identifier))
          {
               IReference reference;
               if (!Try(ParseVariable, out reference) || reference.Type != VarType.Bool) { Errors.Write("Se esperaba una referencia a un valor booleano", Current); hasFailed = true; return null; }
               else if (reference is VariableReference) { left = new BooleanVariableReference((VariableReference)reference); }
               else { throw new NotImplementedException(); }
               Next();
          }
          else { hasFailed = true; return null; }

          return left;
     }
}
