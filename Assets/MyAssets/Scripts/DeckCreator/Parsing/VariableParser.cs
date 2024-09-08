using System;
using UnityEngine;

public class VariableParser : Parser
{
     public override INode ParseTokens()
     {
          if (!Current.Is(TokenType.identifier, true)) { hasFailed = true; return null; }
          Token varToken = Current;
          if (Next().Is("="))
          {
               IReference value;
               Next();
               if (!TryParse(out value)) { Errors.Write("Se esperaba un valor valido para asignar a la variable '" + varToken.Text + "'", Current); hasFailed = true; return null; }
               return new VariableDeclaration(varToken.Text, value);
          }
          else if (Current.Is("."))
          {
               Debug.Log("Analizando a variable: '" + varToken.Text + "' despues del .");
               if (!VariableScopes.ContainsVar(varToken.Text)) { Errors.Write("No es posible acceder a propiedades de la variable '" + varToken.Text + "' ya que no existe en el contexto actual", Current); hasFailed = true; return null; }
               else if (varToken.Text.ScopeValue().Type == VarType.Card) { Debug.Log("Carta"); return ParseCardPropertyOrAction(new VariableReference(varToken.Text, VarType.Card), varToken); }
               else if (varToken.Text.ScopeValue().Type == VarType.Container) { Debug.Log("Container"); return new ContextParser().ContextContainerMethodParser((ContainerReference)varToken.Text.ScopeValue()); }
               else { throw new NotImplementedException(); }
          }
          else
          {
               if (VariableScopes.ContainsVar(varToken.Text)) { Next(-1); return new VariableReference(varToken.Text, varToken.Text.ScopeValue().Type); }
               else { Errors.Write("La variable '" + varToken.Text + "' no existe en el contexto actual pero se esta intentando acceder a ella", Current); hasFailed = true; return null; }
          }
     }
     public IActionStatement ParseCardPowerSetting(IReference cardReference)
     {
          if (!Next().Is("Power")) { Errors.Write("La unica propiedad sobre la cual se puede realizar una accion es 'Power' y se intento acceder a: '" + Current.Text + "'", Current); hasFailed = true; return null; }
          if (!Next().Is("=")) { Errors.Write("Se esperaba token de asignacion '=' para modificar la propiedad 'Power'. Cualquier otro intento de acceso a propiedad de carta no es valido como accion", Current); hasFailed = true; return null; }
          Next();
          IExpression<int> newPower = (IExpression<int>)new ArithmeticExpressionsParser().ParseTokens();
          if (hasFailed) { return null; }
          return new CardPowerSetting(cardReference, newPower);
     }
     public INode ParseCardPropertyOrAction(IReference cardReference, Token varToken)
     {
          if (Peek().Is("Power") && Peek(2).Is("=")) { return ParseCardPowerSetting(cardReference); }
          else if (Next().Is("Power")) { return new CardPropertyReference(cardReference, Current.Text); }
          else if (Current.Is("Owner")) { return new CardPropertyReference(cardReference, Current.Text); }
          else { Errors.Write("No existe una propiedad de carta definida como '" + Current.Text + "'", Current); hasFailed = true; return null; }
     }
}