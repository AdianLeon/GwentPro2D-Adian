using System;
using UnityEngine;
public static partial class Parser
{
     private static IExpression<bool> ParseBooleanExpression() { IExpression<bool> expression = ParseBooleanOperation(); Next(-1); return expression; }
     private static IExpression<bool> ParseBooleanOperation(IExpression<bool> left = null)
     {
          if (left == null) { left = ParseBoolValue(); if (hasFailed) { return null; } }
          while (Current.Is("&&") || Current.Is("||"))
          {
               Token op = Current; Next();
               var right = ParseBoolValue(); if (hasFailed) { return null; }
               left = new BooleanExpression(left, op, right);
          }
          return left;
     }
     private static IExpression<bool> ParseBoolValue()
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
               VariableReference variableReference;
               if (!Try(ParseVariable, out variableReference) || variableReference.Type != VarType.Bool) { Errors.Write("Se esperaba una referencia a un valor booleano", Current); hasFailed = true; return null; }
               left = new BooleanVariableReference(variableReference);
               Next();
          }
          else { hasFailed = true; return null; }

          return left;
     }
}
