using System;
using UnityEngine;

public static partial class Parser
{
    private static INode ParseContext()
    {//Parsea cualquier declaracion que sea de acceso al context
        if (Next().Is("Board") || Current.Text.Contains("Hand") || Current.Text.Contains("Deck") || Current.Text.Contains("Field") || Current.Text.Contains("Graveyard")) { return ParseContextContainer(); }
        else if (Current.Is("TriggerPlayer")) { return new PlayerReference("Self"); }
        else if (Current.Is("TriggerEnemy")) { return new PlayerReference("Other"); }
        else { Errors.Write("No existe la propiedad del contexto: '" + Current.Text + "'"); hasFailed = true; return null; }
    }
    private static INode ParseContextContainer()
    {
        ContainerReference container;
        if (Current.Is("Board")) { container = new ContainerReference(Current.Text, new PlayerReference()); }
        else if (Current.Is("Hand") || Current.Is("Deck") || Current.Is("Field") || Current.Is("Graveyard")) { container = new ContainerReference(Current.Text, new PlayerReference("Self")); }
        else if (Current.Is("OtherHand") || Current.Is("OtherDeck") || Current.Is("OtherField") || Current.Is("OtherGraveyard")) { container = new ContainerReference(Current.Text.Substring(5), new PlayerReference("Other")); }
        else if (Current.Is("HandOfPlayer") || Current.Is("DeckOfPlayer") || Current.Is("FieldOfPlayer") || Current.Is("GraveyardOfPlayer"))
        {
            string containerName = Current.Text.Substring(0, Current.Text.Length - 8);
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            IReference playerReference;
            Next();
            if (!Try(ParseVariable, out playerReference)) { hasFailed = true; }
            if (hasFailed || playerReference.Type != VarType.Player) { Errors.Write("Se esperaba una referencia a algun jugador", Current); }
            container = new ContainerReference(containerName, playerReference);
            if (!Next().Is(")", true)) { hasFailed = true; return null; }
        }
        else { Errors.Write("No existe la propiedad del contexto: '" + Current.Text + "'", Current); hasFailed = true; return null; }

        if (Peek().Is(".")) { Next(); return ParseContextContainerMethod(container); }
        return container;
    }
    private static INode ParseContextContainerMethod(ContainerReference container)
    {
        if (Next().Is("Shuffle") || Current.Is("Pop"))
        {
            if (!Next().Is("(", true) || !Next().Is(")", true)) { hasFailed = true; return null; }
            if (Peek(-2).Is("Shuffle")) { return new ContextShuffleMethod(container); }
            else if (Peek(-2).Is("Pop")) { if (Peek().Is(".")) { Next(); return ParseCardPropertyOrAction(new ContextPopMethod(container)); } return new ContextPopMethod(container); }
            else { throw new NotImplementedException(); }
        }
        else if (Current.Is("Push") || Current.Is("SendBottom") || Current.Is("Remove"))
        {
            if (!Current.Is("Remove") && (container.ContainerName == "Board" || container.ContainerName == "Field"))
            { Errors.Write("El metodo '" + Current.Text + "' no esta disponible para el contenedor '" + container.ContainerName + "'", Current); hasFailed = true; return null; }

            Token methodToken = Current;
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            IReference cardReference;
            INode hopefullyCardReference = null;
            if (Next().Is("context")) { hopefullyCardReference = ParseVariable(); }
            else if (Current.Is(TokenType.identifier))
            {
                if (VariableScopes.ContainsVar(Current.Text)) { hopefullyCardReference = new VariableReference(Current.Text, Current.Text.ScopeValue().Type); }
                else { Errors.Write("La variable: '" + Current.Text + "' no existe en este contexto", Current); hasFailed = true; return null; }
            }
            if (hopefullyCardReference is IReference && ((IReference)hopefullyCardReference).Type == VarType.Card) { cardReference = (IReference)hopefullyCardReference; }
            else { Errors.Write("El parametro pasado a '" + methodToken.Text + "' no es una referencia a una carta"); hasFailed = true; return null; }
            if (!Next().Is(")", true)) { hasFailed = true; return null; }
            return new ContextCardParameterMethod(container, methodToken.Text, cardReference);
        }
        else if (Current.Is("Find"))
        {
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            CardPredicate cardPredicate = ParseCardPredicate();
            if (!Next().Is(")", true)) { hasFailed = true; return null; }
            if (Peek().Is("[")) { Next(); return ParseCardListIndexation(new ContextFindMethod(container, cardPredicate)); }
            return new ContextFindMethod(container, cardPredicate);
        }
        else { Errors.Write("El metodo del contenedor " + container.ContainerName + ": '" + Current.Text + "' no esta definido", Current); hasFailed = true; return null; }
    }
}
