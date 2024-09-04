using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectActionParser : Parser
{
    protected static VariableScopes scopes;
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
        scopes = new VariableScopes();
        scopes.AddNewScope();
        scopes.AddNewVar("targets", new FutureReference(VarType.CardList));
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
        if (!scopes.ContainsVar(varToken.Text)) { Errors.Write("La variable: '" + varToken.Text + "' no ha sido declarada", varToken); hasFailed = true; return null; }
        else if (scopes.GetValue(varToken.Text).Type == VarType.Container) { Next(-1); return new ContextParser().ContextContainerMethodParser((ContainerReference)scopes.GetValue(varToken.Text)); }
        else if (scopes.GetValue(varToken.Text).Type == VarType.Card) { return ParseCardPowerSetting(varToken); }
        else { Errors.Write("El tipo de variable de '" + varToken.Text + "' no admite ningun operador '.'", Current); hasFailed = true; return null; }
    }
    private IActionStatement ParseCardPowerSetting(Token varToken)
    {
        if (!Next().Is("Power")) { Errors.Write("La unica propiedad sobre la cual se puede realizar una accion es 'Power' y se intento acceder a: '" + Current.Text + "'"); hasFailed = true; return null; }
        if (!Next().Is("=")) { Errors.Write("Se esperaba token de asignacion '=' para modificar la propiedad 'Power'. Cualquier otro intento de acceso a propiedad de carta no es valido como accion"); hasFailed = true; return null; }
        if (!Next().Is(TokenType.number, true)) { hasFailed = true; return null; }
        return new CardPowerSetting(new VariableReference(varToken.Text, scopes.GetValue(varToken.Text).Type), int.Parse(Current.Text));
    }
    private VariableDeclaration ParseVariableDeclaration(Token varToken)
    {
        VariableDeclaration varDeclaration;
        if (Next().Is("context"))
        {
            INode varValue = new ContextParser().Parse();
            if (varValue is IReference) { varDeclaration = new VariableDeclaration(varToken.Text, (IReference)varValue); scopes.AddNewVar(varDeclaration); }
            else { Errors.Write("El valor asignado a la variable '" + varToken.Text + "' no es una referencia", varToken); hasFailed = true; return null; }
        }
        else if (Current.Is(TokenType.identifier))
        {
            if (scopes.ContainsVar(Current.Text)) { varDeclaration = new VariableDeclaration(varToken.Text, new VariableReference(Current.Text, scopes.GetValue(Current.Text).Type)); scopes.AddNewVar(varDeclaration); }
            else { Errors.Write("El valor asignado a la variable '" + varToken.Text + "' es una referencia a otra variable '" + Current.Text + "' la cual no existe en este contexto", Current); hasFailed = true; return null; }
        }
        else { Errors.Write("No se pudo asignar a la variable '" + varToken.Text + "' el valor '" + Current.Text + "'", Current); hasFailed = true; return null; }
        return varDeclaration;
    }
    private ForEachCycle ParseForEachCycle()
    {
        if (!Next().Is(TokenType.identifier, true)) { hasFailed = true; return null; }
        string iteratorVarName = Current.Text;
        scopes.AddNewScope();
        scopes.AddNewVar(iteratorVarName, new FutureReference(VarType.Card));
        if (!Next().Is("in", true)) { hasFailed = true; return null; }
        IReference cardReferenceList;
        if (Next().Is(TokenType.identifier))
        {
            string cardListName = Current.Text;
            if (!scopes.ContainsVar(Current.Text)) { Errors.Write("La variable: '" + Current.Text + "' no existe en este contexto", Current); hasFailed = true; return null; }
            IReference reference = scopes.GetValue(Current.Text);
            while (reference is VariableReference) { reference = scopes.GetValue(((VariableReference)reference).VarName); }
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
        scopes.PopLastScope();
        return new ForEachCycle(iteratorVarName, cardReferenceList, foreachStatements);
    }
    private IActionStatement ParseContextAction()
    {
        INode hopefullyIActionStatement = new ContextParser().Parse();
        if (hopefullyIActionStatement is IActionStatement) { return (IActionStatement)hopefullyIActionStatement; }
        else { Errors.Write("La declaracion no es una accion", Current); hasFailed = true; return null; }
    }
    private PrintAction ParsePrintAction()
    {
        PrintAction printAction;
        if (!Next().Is("(", true)) { hasFailed = true; return null; }
        if (Next().Is(TokenType.literal)) { printAction = new PrintAction(Current.Text); }
        // else if (Current.Is(TokenType.identifier)) { }
        else { Errors.Write("Token inesperado: '" + Current.Text + "'. Se esperaba 'literal'", Current); hasFailed = true; return null; }
        if (!Next().Is(")", true)) { hasFailed = true; return null; }
        return printAction;
    }
}
