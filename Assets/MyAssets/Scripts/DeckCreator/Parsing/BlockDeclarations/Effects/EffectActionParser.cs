using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class Parser
{
    private static INode ParseEffectAction(List<(string, VarType)> parameters)
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
        if (parameters != null) { parameters.ForEach(parameter => VariableScopes.AddNewVar(parameter.Item1, new FutureReference(parameter.Item2))); }
        while (!Next().Is("}"))
        {
            effectAction.ActionStatements.Add(ActionStatementParser());
            if (hasFailed) { return null; }
        }
        VariableScopes.PopLastScope();
        return effectAction;
    }
    private static IActionStatement ActionStatementParser()
    {//Parsea una linea de codigo dentro del Action: (targets, contexts) => {...} (incluye ';')
        IActionStatement actionStatement = ParseActionStatement();
        if (actionStatement is VariableDeclaration) { actionStatement.PerformAction(); }
        if (!Next().Is(";", true)) { hasFailed = true; return null; }
        return actionStatement;
    }
    private static IActionStatement ParseActionStatement()
    {//Solo parsea la accion dentro del Action (excluye ';')
        Debug.Log("Parsing actionStatement in: " + Current);
        IActionStatement actionStatement;
        if (Current.Text == "Print") { actionStatement = ParsePrintAction(); }
        else if (Current.Text == "for") { actionStatement = ParseForEachCycle(); }
        else if (Current.Text == "while") { actionStatement = ParseWhileCycle(); }
        else if (!Try(ParseVariable, out actionStatement)) { Errors.Write("Se esperaba una accion", Current); hasFailed = true; return null; }
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
        if (!Next().Is(TokenType.identifier)) { Errors.Write("Se esperaba una referencia a una lista de cartas", Current); hasFailed = true; return null; }

        if (!Try(ParseVariable, out cardReferenceList) && cardReferenceList.Type == VarType.CardList) { Errors.Write("Se esperaba una referencia a una lista de cartas"); hasFailed = true; return null; }

        List<IActionStatement> foreachStatements = new List<IActionStatement>();
        AddStatements(foreachStatements); if (hasFailed) { return null; }

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

        List<IActionStatement> actionStatements = new List<IActionStatement>();
        AddStatements(actionStatements); if (hasFailed) { return null; }

        VariableScopes.PopLastScope();
        return new WhileCycle(booleanExpression, actionStatements);
    }
    private static void AddStatements(List<IActionStatement> actionStatements)
    {
        if (Next().Is("{"))
        {
            while (!Next().Is("}")) { actionStatements.Add(ActionStatementParser()); if (hasFailed) { return; } }
        }
        else { actionStatements.Add(ParseActionStatement()); if (hasFailed) { return; } }
    }
    private static PrintAction ParsePrintAction()
    {
        PrintAction printAction;
        if (!Next().Is("(", true)) { hasFailed = true; return null; }
        Next();
        IExpression<string> message = ParseStringExpression();
        if (hasFailed) { return null; }
        printAction = new PrintAction(message);
        if (!Next().Is(")", true)) { hasFailed = true; return null; }
        return printAction;
    }
}
