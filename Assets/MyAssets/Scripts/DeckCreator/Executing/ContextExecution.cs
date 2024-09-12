using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public static class ContextExecution
{
    private static string GetContainerName(ContainerReference container) => container.ContainerName + GetPlayer(container.Owner);
    private static string GetPlayer(IReference owner)
    {
        if (owner is VariableReference) { return GetPlayer(((VariableReference)owner).VarName.ScopeValue()); }
        if (owner is PlayerReference)
        {
            string player = "";
            if ((owner as PlayerReference).Player == "Self") { player = Judge.GetPlayer.ToString(); }
            else if ((owner as PlayerReference).Player == "Other") { player = Judge.GetEnemy.ToString(); }
            return player;
        }
        else if (owner is CardPropertyReference)
        {
            IReference reference = ((CardPropertyReference)owner).CardReference.DeReference();
            if (reference is CardReference) { return ((CardReference)reference).Owner.ToString(); }
            else if (reference is ContextPopMethod) { return new CardReference(PopContainer((ContextPopMethod)reference)).Owner.ToString(); }
            else { throw new NotImplementedException("Evaluacion no implementada"); }
        }
        else { throw new NotImplementedException("Evaluacion de posible owner no implementado"); }
    }
    public static void ShuffleContainer(ContextShuffleMethod shuffleMethod)
    {
        string containerName = GetContainerName(shuffleMethod.Container);
        Dictionary<string, Action> assigner = new Dictionary<string, Action>
        {
            {"Board",delegate{ ShuffleAllDirectly(new List<string>{"MeleeDropZoneP1", "RangedDropZoneP1", "SiegeDropZoneP1","MeleeDropZoneP2", "RangedDropZoneP2", "SiegeDropZoneP2","WeatherZoneM","WeatherZoneR","WeatherZoneS"}); }},
            {"HandP1",delegate{ ShuffleDirectly("HandP1"); } }, {"HandP2",delegate{ ShuffleDirectly("HandP2"); } },
            {"FieldP1",delegate{ ShuffleAllDirectly(new List<string>{"MeleeDropZoneP1", "RangedDropZoneP1", "SiegeDropZoneP1"}); } }, {"FieldP2",delegate{ ShuffleAllDirectly(new List<string>{"MeleeDropZoneP2", "RangedDropZoneP2", "SiegeDropZoneP2"}); } },
            {"DeckP1",delegate{ GameObject.Find("DeckP1").GetComponent<Deck>().ShuffleDeck(); } }, {"DeckP2",delegate{ GameObject.Find("DeckP2").GetComponent<Deck>().ShuffleDeck(); } },
            {"GraveyardP1",delegate{ ShuffleDirectly("GraveyardP1"); } }, {"GraveyardP2",delegate{ ShuffleDirectly("GraveyardP2"); } }
        };
        if (!assigner.ContainsKey(containerName)) { throw new Exception("El nombre del contenedor a .Shuffle(): '" + containerName + "' no esta entre los definidos."); }
        else { assigner[containerName].Invoke(); }
    }
    private static void ShuffleAllDirectly(List<string> gameObjectNames) => gameObjectNames.ForEach(name => ShuffleDirectly(name));
    private static void ShuffleDirectly(string gameObjectName)
    {
        GameObject gameObject = GameObject.Find(gameObjectName) ?? throw new Exception("No se encontro el gameObject nombrado: '" + gameObjectName + "'");
        if (gameObject.transform.childCount == 0) { return; }
        foreach (Transform child in gameObject.transform) { child.SetSiblingIndex(UnityEngine.Random.Range(0, gameObject.transform.childCount)); }
    }
    public static DraggableCard PopContainer(ContextPopMethod contextPopMethod)
    {
        DraggableCard card;
        string containerName = GetContainerName(contextPopMethod.Container);
        Dictionary<string, IEnumerable<DraggableCard>> assigner = new Dictionary<string, IEnumerable<DraggableCard>>
        {
            {"Board", Field.PlayedFieldCards.Cast<DraggableCard>().Randomize()},
            {"HandP1", GameObject.Find("HandP1").GetComponent<Hand>().GetCards}, { "HandP2", GameObject.Find("HandP2").GetComponent<Hand>().GetCards},
            { "FieldP1", GameObject.Find("FieldP1").GetComponent<Field>().GetCards.Randomize()}, { "FieldP2", GameObject.Find("FieldP2").GetComponent<Field>().GetCards.Randomize()},
            { "DeckP1", GameObject.Find("DeckP1").GetComponent<Deck>().GetCards}, { "DeckP2", GameObject.Find("DeckP2").GetComponent<Deck>().GetCards},
            { "GraveyardP1", GameObject.Find("GraveyardP1").GetComponent<Graveyard>().GetCards}, { "GraveyardP2", GameObject.Find("GraveyardP2").GetComponent<Graveyard>().GetCards}
        };
        if (!assigner.ContainsKey(containerName)) { throw new Exception("El nombre del contenedor a .Pop(): '" + containerName + "' no esta entre los definidos."); }
        else if (assigner[containerName].Count() == 0) { return null; }
        else if (containerName == "DeckP1" || containerName == "DeckP2") { card = GameObject.Find(containerName).GetComponent<Deck>().DrawTopCard(); card.Disappear(); }
        else { card = assigner[containerName].Last(); card.Disappear(); }
        return card;
    }
    public static void DoActionForCardParameterMethod(ContextCardParameterMethod method)
    {
        IReference cardReference = method.Card.DeReference();
        DraggableCard cardToPerformActionOn;
        if (cardReference is ContextPopMethod) { cardToPerformActionOn = PopContainer((ContextPopMethod)cardReference); }
        else if (cardReference is CardReference) { cardToPerformActionOn = ((CardReference)cardReference).Card; }
        else if (cardReference is CardListIndexation) { cardToPerformActionOn = IndexingResult((CardListIndexation)cardReference); }
        else { throw new Exception("No se ha definido la forma de evaluar la ICardReference"); }

        if (method.ActionType == "Push" || method.ActionType == "SendBottom")
        {
            if (method.Container.ContainerName == "Hand" || method.Container.ContainerName == "Graveyard")
            {
                if (cardToPerformActionOn != null)
                {
                    cardToPerformActionOn.MoveCardTo(GameObject.Find(GetContainerName(method.Container)));
                    if (method.ActionType == "SendBottom") { cardToPerformActionOn.transform.SetSiblingIndex(0); }
                }
            }
            else if (method.Container.ContainerName == "Deck")
            {
                if (method.ActionType == "Push") { GameObject.Find(GetContainerName(method.Container)).GetComponent<Deck>().PushCard(cardToPerformActionOn); }
                else if (method.ActionType == "SendBottom") { GameObject.Find(GetContainerName(method.Container)).GetComponent<Deck>().SendBottomCard(cardToPerformActionOn); }
            }
            else { throw new Exception("No se ha definido '" + method.ActionType + "' para '" + method.Container.ContainerName + "'"); }
        }
        else if (method.ActionType == "Remove")
        {
            DraggableCard cardToRemove = null;
            cardToRemove = GameObject.Find(GetContainerName(method.Container)).CardsInside<DraggableCard>().SingleOrDefault(card => card == cardToPerformActionOn);
            if (cardToRemove != null) { cardToRemove.Disappear(); }
        }
        else { throw new Exception("No se ha definido la evaluacion de la accion: " + method.ActionType); }
    }
    private static DraggableCard IndexingResult(CardListIndexation indexationOrder)
    {
        List<DraggableCard> cards;
        if (indexationOrder.CardListReference is ContextFindMethod) { cards = FindCards((ContextFindMethod)indexationOrder.CardListReference); }
        else { throw new NotImplementedException("No se ha definido la forma de evaluar la lista de cartas a indexar"); }
        int index = indexationOrder.Index.Evaluate();
        if (index < 0 || index >= cards.Count) { throw new Exception("Indice:" + index + " no valido"); /*Executer.RaiseErrorScreen(); */}
        return cards[index];
    }
    public static List<DraggableCard> FindCards(ContextFindMethod findMethod)
    {
        List<DraggableCard> cards;
        string containerName = GetContainerName(findMethod.Container);
        Dictionary<string, IEnumerable<DraggableCard>> assigner = new Dictionary<string, IEnumerable<DraggableCard>>
        {
            {"Board", Field.PlayedFieldCards.Cast<DraggableCard>().Randomize()},
            {"HandP1", GameObject.Find("HandP1").GetComponent<Hand>().GetCards}, { "HandP2", GameObject.Find("HandP2").GetComponent<Hand>().GetCards},
            { "FieldP1", GameObject.Find("FieldP1").GetComponent<Field>().GetCards.Randomize()}, { "FieldP2", GameObject.Find("FieldP2").GetComponent<Field>().GetCards.Randomize()},
            { "DeckP1", GameObject.Find("DeckP1").GetComponent<Deck>().GetCards}, { "DeckP2", GameObject.Find("DeckP2").GetComponent<Deck>().GetCards},
            { "GraveyardP1", GameObject.Find("GraveyardP1").GetComponent<Graveyard>().GetCards}, { "GraveyardP2", GameObject.Find("GraveyardP2").GetComponent<Graveyard>().GetCards}
        };
        if (!assigner.ContainsKey(containerName)) { throw new Exception("El nombre del contenedor a .Find(): '" + containerName + "' no esta entre los definidos."); }
        else if (assigner[containerName].Count() == 0) { return new List<DraggableCard> { }; }
        cards = assigner[containerName].ToList();
        cards = cards.Where(card => findMethod.CardPredicate.EvaluateCard(new CardReference(card))).ToList();
        return cards;
    }
}
