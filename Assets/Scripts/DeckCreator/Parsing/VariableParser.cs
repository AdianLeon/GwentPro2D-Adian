using System;
using UnityEngine;

public partial class Parser
{
     private INode ParseVariable()
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
          else if (Current.Is("+=") || Current.Is("-=") || Current.Is("*=") || Current.Is("/=") || Current.Is("^="))
          {
               Token op = Current; Next();
               IExpression<int> right = ParseArithmeticExpression();
               return new VariableAlteration(varName, op, right);
          }
          else if (Current.Is("++") || Current.Is("--"))
          {
               Token op = Current;
               return new VariableAlteration(varName, op, new NumberExpression("1"));
          }
          else if (Current.Is("["))
          {
               if (varName.ScopeValue().Type != VarType.CardList) { Errors.Write(""); hasFailed = true; return null; }
               return ParseCardListIndexation(varName.ScopeValue());
          }
          else
          {
               if (VariableScopes.ContainsVar(varName)) { Next(-1); return new VariableReference(varName, varName.ScopeValue().Type); }
               else { Errors.Write("La variable '" + varName + "' no existe en el contexto actual pero se esta intentando acceder a ella", Current); hasFailed = true; return null; }
          }
     }
     private CardListIndexation ParseCardListIndexation(IReference cardListReference)
     {
          if (!Current.Is("[", true)) { hasFailed = true; return null; }
          Next();
          IExpression<int> index = ParseArithmeticExpression();
          if (!Next().Is("]", true)) { hasFailed = true; return null; }
          return new CardListIndexation(cardListReference, index);
     }
     private IActionStatement ParseCardPowerAlteration(IReference cardReference)
     {
          if (Next().Is("++") || Current.Is("--")) { return new CardPowerAlteration(cardReference, Current.Text); }
          if (!(Current.Is("=") || Current.Is("+=") || Current.Is("-=") || Current.Is("*=") || Current.Is("/=") || Current.Is("^="))) { Errors.Write("La operacion '" + Current.Text + "' no esta definida", Current); hasFailed = true; return null; }
          string operation = Current.Text;
          Next();
          IExpression<int> newPower = ParseArithmeticExpression(); if (hasFailed) { return null; }
          return new CardPowerAlteration(cardReference, operation, newPower);
     }
     private INode ParseCardPropertyOrAction(IReference cardReference)
     {
          if (Next().Is("Power") && (Peek().Is("=") || Peek().Is("+=") || Peek().Is("-=") || Peek().Is("*=") || Peek().Is("/=") || Peek().Is("^=") || Peek().Is("++") || Peek().Is("--"))) { return ParseCardPowerAlteration(cardReference); }
          else if (Current.Is("Power") || Current.Is("Owner") || Current.Is("Name") || Current.Is("Faction")) { return new CardPropertyReference(cardReference, Current.Text); }
          else { Errors.Write("No existe una propiedad de carta definida como '" + Current.Text + "'", Current); hasFailed = true; return null; }
     }
}