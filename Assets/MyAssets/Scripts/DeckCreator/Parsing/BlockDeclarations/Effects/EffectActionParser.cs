using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectActionParser : Parser
{
    public override INode ParseTokens()
    {
        if (!Current.Is("(", true)) { hasFailed = true; }
        if (!Next().Is("targets", true)) { hasFailed = true; }
        if (!Next().Is(",", true)) { hasFailed = true; }
        if (!Next().Is("context", true)) { hasFailed = true; }
        if (!Next().Is(")", true)) { hasFailed = true; }
        if (!Next().Is("=>", true)) { hasFailed = true; }
        if (!Next().Is("{", true)) { hasFailed = true; }
        if (hasFailed) { return null; }

        EffectAction effectAction = new EffectAction();
        VariableScopes.Reset();
        VariableScopes.AddNewVar("targets", new FutureReference(VarType.CardList));
        while (!Next().Is("}"))
        {
            effectAction.ActionStatements.Add(ActionStatementParser());
            if (hasFailed) { return null; }
        }
        return effectAction;
    }
    private IActionStatement ActionStatementParser()
    {//Parsea una linea de codigo dentro del Action: (targets, contexts) => {...}
        IActionStatement actionStatement;
        if (Current.Is("context")) { actionStatement = ParseContextAction(); if (hasFailed) { return null; } }
        else if (Current.Is("Print")) { actionStatement = ParsePrintAction(); if (hasFailed) { return null; } }
        else if (Current.Is("for")) { actionStatement = ParseForEachCycle(); if (hasFailed) { return null; } }
        else if (Current.Is(TokenType.identifier))
        {
            Token varToken = Current;
            if (Next().Is("=")) { actionStatement = ParseVariableDeclaration(varToken); if (hasFailed) { return null; } }
            else if (Current.Is(".")) { actionStatement = ParseVariableAction(varToken); if (hasFailed) { return null; } }
            else { Errors.Write("Se esperaba '=' para asignar un valor o '.' para realizar acciones sobre '" + Current.Text + "'", Current); hasFailed = true; return null; }
        }
        else { Errors.Write("Accion desconocida: '" + Current.Text + "'", Current); hasFailed = true; return null; }

        if (!Next().Is(";", true)) { hasFailed = true; return null; }
        if (actionStatement == null && !hasFailed) { throw new Exception("No se pudo procesar una accion sin embargo el parser nunca fallo"); }
        return actionStatement;
    }
    private IActionStatement ParseVariableAction(Token varToken)
    {
        if (!VariableScopes.ContainsVar(varToken.Text)) { Errors.Write("La variable: '" + varToken.Text + "' no ha sido declarada", varToken); hasFailed = true; return null; }
        else if (varToken.Text.ScopeValue().Type == VarType.Container)
        {
            Next(-1); INode hopefullyActionStatement = new ContextParser().ContextContainerMethodParser((ContainerReference)varToken.Text.ScopeValue());
            if (hopefullyActionStatement is IActionStatement) { return (IActionStatement)hopefullyActionStatement; }
            else { Errors.Write("Se esperaba una accion a realizar sobre la variable '" + varToken.Text + "'", varToken); hasFailed = true; return null; }
        }
        else if (varToken.Text.ScopeValue().Type == VarType.Card) { return ParseCardPowerSetting(varToken); }
        else { Errors.Write("El tipo de variable de '" + varToken.Text + "' no admite ningun operador '.'", Current); hasFailed = true; return null; }
    }
    private IActionStatement ParseCardPowerSetting(Token varToken)
    {
        if (!Next().Is("Power")) { Errors.Write("La unica propiedad sobre la cual se puede realizar una accion es 'Power' y se intento acceder a: '" + Current.Text + "'"); hasFailed = true; return null; }
        if (!Next().Is("=")) { Errors.Write("Se esperaba token de asignacion '=' para modificar la propiedad 'Power'. Cualquier otro intento de acceso a propiedad de carta no es valido como accion"); hasFailed = true; return null; }
        Next();
        IExpression<int> newPower = (IExpression<int>)new ArithmeticExpressionsParser().ParseTokens();
        if (hasFailed) { return null; }
        return new CardPowerSetting(new VariableReference(varToken.Text, varToken.Text.ScopeValue().Type), newPower);
    }
    private VariableDeclaration ParseVariableDeclaration(Token varToken)
    {
        VariableDeclaration varDeclaration;
        if (Next().Is("context"))
        {
            INode varValue = new ContextParser().ParseTokens();
            if (varValue is IReference) { varDeclaration = new VariableDeclaration(varToken.Text, (IReference)varValue); VariableScopes.AddNewVar(varDeclaration); }
            else { Errors.Write("El valor asignado a la variable '" + varToken.Text + "' no es una referencia", varToken); hasFailed = true; return null; }
        }
        else if (Current.Is(TokenType.identifier))
        {
            if (VariableScopes.ContainsVar(Current.Text))
            {
                if (Peek().Is(".") && Current.Text.ScopeValue().Type == VarType.Card)
                {
                    VariableReference cardReference = new VariableReference(Current.Text, VarType.Card);
                    Next(); varDeclaration = new VariableDeclaration(varToken.Text, ParseCardProperty(cardReference));
                    VariableScopes.AddNewVar(varDeclaration);
                }
                else { varDeclaration = new VariableDeclaration(varToken.Text, new VariableReference(Current.Text, Current.Text.ScopeValue().Type)); VariableScopes.AddNewVar(varDeclaration); }
            }
            else { Errors.Write("El valor asignado a la variable '" + varToken.Text + "' es una referencia a otra variable '" + Current.Text + "' la cual no existe en este contexto", Current); hasFailed = true; return null; }
        }
        else { Errors.Write("No se pudo asignar a la variable '" + varToken.Text + "' el valor '" + Current.Text + "'", Current); hasFailed = true; return null; }
        return varDeclaration;
    }
    public CardPropertyReference ParseCardProperty(IReference cardReference)
    {
        if (Next().Is("Owner")) { return new CardPropertyReference(cardReference, Current.Text); }
        else { Errors.Write("No existe una propiedad de carta definida como '" + Current.Text + "'", Current); hasFailed = true; return null; }
    }
    private ForEachCycle ParseForEachCycle()
    {
        if (!Next().Is(TokenType.identifier, true)) { hasFailed = true; return null; }
        string iteratorVarName = Current.Text;
        VariableScopes.AddNewScope();
        VariableScopes.AddNewVar(iteratorVarName, new FutureReference(VarType.Card));
        if (!Next().Is("in", true)) { hasFailed = true; return null; }
        IReference cardReferenceList;
        if (Next().Is(TokenType.identifier))
        {
            string cardListName = Current.Text;
            if (!VariableScopes.ContainsVar(Current.Text)) { Errors.Write("La variable: '" + Current.Text + "' no existe en este contexto", Current); hasFailed = true; return null; }
            IReference reference = Current.Text.ScopeValue();
            while (reference is VariableReference) { reference = ((VariableReference)reference).VarName.ScopeValue(); }
            if (reference.Type != VarType.CardList  /*&& reference.Type is not VarType.Container*/) { Errors.Write("La variable: '" + Current.Text + "' no guarda una referencia a una lista de cartas", Current); hasFailed = true; return null; }
            cardReferenceList = new VariableReference(cardListName, VarType.CardList);
        }
        // else if(Current.Is("context")){}//Se pueden usar las propiedades del context como FutureCardReferenceList(), modificar la herencia de los ContainerReference para que sean FutureCardReferenceList()
        else { Errors.Write("Se esperaba una lista de cartas", Current); hasFailed = true; return null; }
        if (!Next().Is("{", true)) { hasFailed = true; return null; }
        List<IActionStatement> foreachStatements = new List<IActionStatement>();
        while (!Next().Is("}"))
        {
            foreachStatements.Add(ActionStatementParser());
            if (hasFailed) { return null; }
        }
        VariableScopes.PopLastScope();
        return new ForEachCycle(iteratorVarName, cardReferenceList, foreachStatements);
    }
    private IActionStatement ParseContextAction()
    {
        INode hopefullyIActionStatement = new ContextParser().ParseTokens();
        if (hopefullyIActionStatement is IActionStatement) { return (IActionStatement)hopefullyIActionStatement; }
        else { Errors.Write("La declaracion no es una accion", Current); hasFailed = true; return null; }
    }
    private PrintAction ParsePrintAction()
    {
        PrintAction printAction;
        if (!Next().Is("(", true)) { hasFailed = true; return null; }
        Next();
        IExpression<string> message = (IExpression<string>)new StringExpressionsParser().ParseTokens();
        if (hasFailed) { return null; }
        printAction = new PrintAction(message);
        if (!Next().Is(")", true)) { hasFailed = true; return null; }
        return printAction;
    }
}
