using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectActionParser : Parser
{
    public static VariableScopes scopes;
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
        IActionStatement actionStatement = null;
        if (Current.Is("context"))
        {
            object hopefullyIActionStatement = ContextParser();
            if (hasFailed) { return null; }
            if (hopefullyIActionStatement is IActionStatement) { actionStatement = (IActionStatement)hopefullyIActionStatement; }
            else { Errors.Write("La declaracion no es una accion", Current); hasFailed = true; return null; }
        }
        else if (Current.Is("Print"))
        {
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            if (Next().Is(TokenType.literal, true)) { actionStatement = new PrintAction(Current.Text); }
            // else if (Current.Is(TokenType.identifier)) { }
            else { hasFailed = true; return null; }

            if (!Next().Is(")", true)) { hasFailed = true; return null; }
        }
        else if (Current.Is("for"))
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
            actionStatement = new ForEachCycle(iteratorVarName, cardReferenceList, foreachStatements);
        }
        else if (Current.Is(TokenType.identifier))
        {//Parseo de variables o accesos a variables
            Token varToken = Current;
            string varName = Current.Text;
            if (Next().Is("="))
            {
                if (Next().Is("context"))
                {
                    object varValue = ContextParser();
                    if (varValue is IReference)
                    {
                        actionStatement = new VariableDeclaration(varName, (IReference)varValue);
                        scopes.AddNewVar((VariableDeclaration)actionStatement);
                    }
                    else { Errors.Write("El valor asignado a la variable '" + varName + "' no es una referencia", varToken); hasFailed = true; return null; }
                }
                else if (Current.Is(TokenType.identifier))
                {
                    if (scopes.ContainsVar(Current.Text))
                    {
                        actionStatement = new VariableDeclaration(varName, new VariableReference(Current.Text, scopes.GetValue(Current.Text).Type));
                        scopes.AddNewVar((VariableDeclaration)actionStatement);
                    }
                }
                else { Errors.Write("No se pudo asignar a la variable '" + varName + "' el valor '" + Current.Text + "'", Current); hasFailed = true; return null; }
            }
            else if (Current.Is("."))
            {
                if (!scopes.ContainsVar(varName)) { Errors.Write("La variable: '" + varName + "' no ha sido declarada", varToken); hasFailed = true; return null; }
                else if (scopes.GetValue(varName).Type == VarType.Container) { Next(-1); actionStatement = ContextContainerMethodParser((ContainerReference)scopes.GetValue(varName)); }
                else { Errors.Write("El tipo de variable de '" + varName + "' no admite ningun operador '.'", Current); hasFailed = true; return null; }
            }
            else { Errors.Write("Se esperaba '=' para asignar un valor o '.' para realizar acciones sobre '" + Current.Text + "'", Current); hasFailed = true; return null; }
        }
        else { Errors.Write("Accion desconocida: '" + Current.Text + "'", Current); hasFailed = true; return null; }

        if (!Next().Is(";", true)) { hasFailed = true; return null; }

        if (actionStatement == null) { Errors.Write("No se pudo procesar una accion", Current); hasFailed = true; }
        return actionStatement;
    }
    private object ContextParser()
    {//Parsea cualquier declaracion que sea de acceso al context
        if (!Current.Is("context", true)) { hasFailed = true; return null; }
        if (!Next().Is(".", true)) { hasFailed = true; return null; }

        if (Next().Is("Board") || Current.Text.Contains("Hand") || Current.Text.Contains("Deck") || Current.Text.Contains("Field") || Current.Text.Contains("Graveyard")) { return ContextContainerParser(); }
        else if (Current.Is("TriggerPlayer")) { return new PlayerReference("Self"); }
        else if (Current.Is("TriggerEnemy")) { return new PlayerReference("Other"); }
        else { Errors.Write("No existe la propiedad del contexto: '" + Current.Text + "'"); hasFailed = true; return null; }
    }
    private object ContextContainerParser()
    {
        ContainerReference container;
        if (Current.Is("Board")) { container = new ContainerReference(Current.Text, new PlayerReference()); }
        else if (Current.Is("Hand") || Current.Is("Deck") || Current.Is("Field") || Current.Is("Graveyard")) { container = new ContainerReference(Current.Text, new PlayerReference("Self")); }
        else if (Current.Is("OtherHand") || Current.Is("OtherDeck") || Current.Is("OtherField") || Current.Is("OtherGraveyard")) { container = new ContainerReference(Current.Text.Substring(5), new PlayerReference("Other")); }
        else if (Current.Is("HandOfPlayer") || Current.Is("DeckOfPlayer") || Current.Is("FieldOfPlayer") || Current.Is("GraveyardOfPlayer"))
        {
            string containerName = Current.Text.Substring(0, Current.Text.Length - 8);
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            object hopefullyPlayerReference;

            if (Next().Is("context")) { hopefullyPlayerReference = ContextParser(); }
            else if (Current.Is(TokenType.identifier))
            {
                if (scopes.ContainsVar(Current.Text)) { hopefullyPlayerReference = new VariableReference(Current.Text, scopes.GetValue(Current.Text).Type); }
                else { Errors.Write("La variable: '" + Current.Text + "' no existe en este contexto", Current); hasFailed = true; return null; }
            }
            else { hopefullyPlayerReference = null; }
            if (hopefullyPlayerReference is IReference && (hopefullyPlayerReference as IReference).Type == VarType.Player)
            {
                while (hopefullyPlayerReference is VariableReference) { hopefullyPlayerReference = scopes.GetValue((hopefullyPlayerReference as VariableReference).VarName); }
                if (hopefullyPlayerReference is not PlayerReference) { throw new System.Exception("Las referencias no apuntaron a un jugador valido"); }
                PlayerReference player = (PlayerReference)hopefullyPlayerReference;
                if (!Next().Is(")", true)) { hasFailed = true; return null; }
                container = new ContainerReference(containerName, player);
            }
            else { Errors.Write("Se esperaba una referencia a algun jugador", Current); hasFailed = true; return null; }
        }
        else { Errors.Write("No existe la propiedad del contexto: '" + Current.Text + "'", Current); hasFailed = true; return null; }

        if (Peek().Is(".")) { return ContextContainerMethodParser(container); }
        return container;
    }
    private IActionStatement ContextContainerMethodParser(ContainerReference container)
    {
        if (!Next().Is(".", true)) { hasFailed = true; return null; }

        if (Next().Is("Shuffle") || Current.Is("Pop"))
        {
            if (!Next().Is("(", true) || !Next().Is(")", true)) { hasFailed = true; return null; }
            if (Peek(-2).Is("Shuffle")) { return new ContextShuffleMethod(container); }
            else { return new ContextPopMethod(container); }
        }
        else if (Current.Is("Push") || Current.Is("SendBottom") || Current.Is("Remove"))
        {
            if (!Current.Is("Remove") && (container.ContainerName == "Board" || container.ContainerName == "Field"))
            { Errors.Write("El metodo '" + Current.Text + "' no esta disponible para el contenedor '" + container.ContainerName + "'", Current); hasFailed = true; return null; }

            Token methodToken = Current;
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            IReference cardReference;
            object hopefullyCardReference = null;
            if (Next().Is("context")) { hopefullyCardReference = ContextParser(); }
            else if (Current.Is(TokenType.identifier))
            {
                if (scopes.ContainsVar(Current.Text)) { hopefullyCardReference = new VariableReference(Current.Text, scopes.GetValue(Current.Text).Type); }
                else { Errors.Write("La variable: '" + Current.Text + "' no existe en este contexto", Current); hasFailed = true; return null; }
            }
            if (hopefullyCardReference is IReference && ((IReference)hopefullyCardReference).Type == VarType.Card) { cardReference = (IReference)hopefullyCardReference; }
            else { Errors.Write("El parametro pasado a '" + methodToken.Text + "' no es una referencia a una carta"); hasFailed = true; return null; }
            if (!Next().Is(")", true)) { hasFailed = true; return null; }
            return new ContextCardParameterMethod(container, methodToken.Text, cardReference);
        }
        else { Errors.Write("El metodo del contenedor " + container.ContainerName + ": '" + Current.Text + "' no esta definido", Current); hasFailed = true; return null; }
    }
}
