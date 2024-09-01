using System.Collections.Generic;
using UnityEngine;

public class EffectActionParser : Parser
{
    public static VariableScopes varScopes;
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
        varScopes = new VariableScopes();
        varScopes.AddNewScope();

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
        else if (Current.Is(TokenType.identifier))
        {//Parseo de variables o accesos a variables
            Token varToken = Current;
            string varName = Current.Text;
            if (Next().Is("=", true))
            {
                if (Next().Is("context"))
                {
                    object varValue = ContextParser();
                    if (varValue is IReference)
                    {
                        actionStatement = new VariableDeclaration(varName, (IReference)varValue);
                        varScopes.AddNewVar((VariableDeclaration)actionStatement);
                    }
                    else { Errors.Write("El valor asignado a la variable '" + varName + "' no es una referencia", varToken); hasFailed = true; return null; }
                }
                else if (Current.Is(TokenType.identifier))
                {
                    if (varScopes.ContainsVar(Current.Text))
                    {
                        actionStatement = new VariableDeclaration(varName, new VariableReference(Current.Text));
                        varScopes.AddNewVar((VariableDeclaration)actionStatement);
                    }
                }
                else { Errors.Write("No se pudo asignar a la variable '" + varName + "' el valor '" + Current.Text + "'", Current); hasFailed = true; return null; }
            }
            else if (Current.Is("."))
            {
                if (!varScopes.ContainsVar(varName)) { Errors.Write("La variable: '" + varName + "' no ha sido declarada", varToken); hasFailed = true; return null; }
                if (varScopes.GetValue(varName) is ContainerReference) { Next(-1); ContextContainerMethodParser((ContainerReference)varScopes.GetValue(varName)); }
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
                if (varScopes.ContainsVar(Current.Text)) { hopefullyPlayerReference = varScopes.GetValue(Current.Text); }
                else { Errors.Write("La variable: '" + Current.Text + "' no existe en este contexto", Current); hasFailed = true; return null; }
            }
            else { hopefullyPlayerReference = null; }
            if (hopefullyPlayerReference is PlayerReference)
            {
                PlayerReference player = (PlayerReference)hopefullyPlayerReference;
                if (!Next().Is(")", true)) { hasFailed = true; return null; }
                container = new ContainerReference(containerName, player);
            }
            else { Errors.Write("Se esperaba una referencia a algun jugador", Current); hasFailed = true; return null; }
        }
        else { Errors.Write("No existe la propiedad del contexto: '" + Current.Text + "'"); hasFailed = true; return null; }

        if (Peek().Is(".")) { return ContextContainerMethodParser(container); }
        return container;
    }
    private IActionStatement ContextContainerMethodParser(ContainerReference container)
    {
        if (!Next().Is(".")) { hasFailed = true; return null; }

        if (Next().Is("Shuffle") || Current.Is("Pop"))
        {
            if (!Next().Is("(", true) || !Next().Is(")", true)) { hasFailed = true; return null; }
            if (Peek(-2).Is("Shuffle")) { return new ContextShuffleMethod(container); }
            else { return new ContextPopMethod(container); }
        }
        else if (Current.Is("Push") || Current.Is("SendBottom"))
        {
            if (container.Name == ContainerReference.ContainerToGet.Board || container.Name == ContainerReference.ContainerToGet.Field)
            { Errors.Write("El metodo '" + Current.Text + "' no esta disponible para el contenedor '" + container.Name + "'", Current); hasFailed = true; return null; }

            Token methodToken = Current;
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            ICardReference cardReference;
            Next();
            object hopefullyCardReference;
            if (Current.Is("context"))
            {
                hopefullyCardReference = ContextParser();
            }
            else if (Current.Is(TokenType.identifier))
            {
                if (varScopes.ContainsVar(Current.Text)) { hopefullyCardReference = varScopes.GetValue(Current.Text); }
                else { Errors.Write("La variable: '" + Current.Text + "' no existe en este contexto", Current); hasFailed = true; return null; }
            }
            else { Errors.Write("El parametro pasado a '" + methodToken.Text + "' no es una referencia a una carta"); hasFailed = true; return null; }
            if (hopefullyCardReference is ICardReference) { cardReference = (ICardReference)hopefullyCardReference; }
            else { Errors.Write("El parametro pasado a '" + methodToken.Text + "' no es una referencia a una carta"); hasFailed = true; return null; }
            if (!Next().Is(")", true)) { hasFailed = true; return null; }
            return new ContextCardParameterMethod(container, methodToken.Text, cardReference);
        }
        else { Errors.Write("El metodo del contenedor " + container.Name + ": '" + Current.Text + "' no esta definido", Current); hasFailed = true; return null; }
    }
}
