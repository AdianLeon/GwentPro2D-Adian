using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class Parser
{
    private static INode ParseEffectAction()
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
        VariableScopes.AddNewVar("context", new FutureReference(VarType.None));
        while (!Next().Is("}"))
        {
            effectAction.ActionStatements.Add(ActionStatementParser());
            if (hasFailed) { return null; }
        }
        VariableScopes.PopLastScope();
        return effectAction;
    }
    private static IActionStatement ActionStatementParser()
    {//Parsea una linea de codigo dentro del Action: (targets, contexts) => {...}
        IActionStatement actionStatement;
        Debug.Log("Parsing actionStatement in: " + Current);
        if (Current.Text == "Print") { actionStatement = ParsePrintAction(); }
        else if (Current.Text == "for") { actionStatement = ParseForEachCycle(); }
        else if (Current.Text == "while") { actionStatement = ParseWhileCycle(); }
        else if (!Try(ParseVariable, out actionStatement)) { Errors.Write("Se esperaba una accion", Current); hasFailed = true; return null; }

        if (actionStatement is VariableDeclaration) { actionStatement.PerformAction(); }
        if (!Next().Is(";", true)) { hasFailed = true; return null; }
        return actionStatement;
    }
    private static ForEachCycle ParseForEachCycle()
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
    private static WhileCycle ParseWhileCycle()
    {
        if (!Current.Is("while", true)) { hasFailed = true; return null; }
        VariableScopes.AddNewScope();
        if (!Next().Is("(", true)) { hasFailed = true; return null; }
        Next();
        IExpression<bool> booleanExpression;
        if (!Try(ParseExpressions, out booleanExpression)) { Errors.Write("Se esperaba una expresion booleana", Current); hasFailed = true; return null; }
        if (!Next().Is(")", true)) { hasFailed = true; return null; }
        if (!Next().Is("{", true)) { hasFailed = true; return null; }
        List<IActionStatement> actionStatements = new List<IActionStatement>();
        while (!Next().Is("}"))
        {
            actionStatements.Add(ActionStatementParser());
            if (hasFailed) { return null; }
        }
        VariableScopes.PopLastScope();
        return new WhileCycle(booleanExpression, actionStatements);
    }
    private static PrintAction ParsePrintAction()
    {
        PrintAction printAction;
        if (!Next().Is("(", true)) { hasFailed = true; return null; }
        Next();
        IExpression<string> message = (IExpression<string>)ParseStringExpression();
        if (hasFailed) { return null; }
        printAction = new PrintAction(message);
        if (!Next().Is(")", true)) { hasFailed = true; return null; }
        return printAction;
    }
}
