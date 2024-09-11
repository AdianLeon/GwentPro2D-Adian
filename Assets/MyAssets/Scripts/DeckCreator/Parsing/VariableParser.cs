using System;
using UnityEngine;

public static partial class Parser
{
     private static INode ParseVariable()
     {
          if (!Current.Is(TokenType.identifier)) { Errors.Write("Se esperaba la mencion a una variable o el acceso a la propiedad de alguna variable, se encontro: '" + Current.Text + "'", Current); hasFailed = true; return null; }
          string varName = Current.Text;
          if (Next().Is("="))
          {
               IReference value;
               Next();
               if (Try(ParseExpressions, out value, false) || Try(ParseVariable, out value)) { hasFailed = false; return new VariableDeclaration(varName, value); }
               else { Errors.Write("Se esperaba una referencia para asignar a la variable '" + varName + "'", Current); hasFailed = true; return null; }
          }
          else if (Current.Is("."))
          {
               if (!VariableScopes.ContainsVar(varName)) { Errors.Write("No es posible acceder a propiedades de la variable '" + varName + "' ya que no existe en el contexto actual", Current); hasFailed = true; return null; }
               else if (varName == "context") { return ParseContext(); }
               else if (varName.ScopeValue().Type == VarType.Card) { return ParseCardPropertyOrAction(new VariableReference(varName, VarType.Card)); }
               else if (varName.ScopeValue().Type == VarType.Container) { return ParseContextContainerMethod((ContainerReference)varName.ScopeValue()); }
               else { throw new NotImplementedException(); }
          }
          else
          {
               if (VariableScopes.ContainsVar(varName)) { Next(-1); Debug.Log("Referencia a la var: '" + varName + "' devuelta"); return new VariableReference(varName, varName.ScopeValue().Type); }
               else { Errors.Write("La variable '" + varName + "' no existe en el contexto actual pero se esta intentando acceder a ella", Current); hasFailed = true; return null; }
          }
     }
     private static IActionStatement ParseCardPowerSetting(IReference cardReference)
     {
          if (!Next().Is("Power")) { Errors.Write("La unica propiedad sobre la cual se puede realizar una accion es 'Power' y se intento acceder a: '" + Current.Text + "'", Current); hasFailed = true; return null; }
          if (!Next().Is("=")) { Errors.Write("Se esperaba token de asignacion '=' para modificar la propiedad 'Power'. Cualquier otro intento de acceso a propiedad de carta no es valido como accion", Current); hasFailed = true; return null; }
          Next();
          IExpression<int> newPower = ParseArithmeticExpression();
          if (hasFailed) { return null; }
          return new CardPowerSetting(cardReference, newPower);
     }
     private static INode ParseCardPropertyOrAction(IReference cardReference)
     {
          if (Peek().Is("Power") && Peek(2).Is("=")) { return ParseCardPowerSetting(cardReference); }
          else if (Next().Is("Power")) { return new CardPropertyReference(cardReference, Current.Text); }
          else if (Current.Is("Owner")) { return new CardPropertyReference(cardReference, Current.Text); }
          else { Errors.Write("No existe una propiedad de carta definida como '" + Current.Text + "'", Current); hasFailed = true; return null; }
     }
}