using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ContextUtils
{
    public static void AssignMethod(ContextMethod method)
    {
        if (method is ContextShuffleMethod) { ShuffleContainer((ContextShuffleMethod)method); }
        else if (method is ContextPopMethod) { PopContainer((ContextPopMethod)method); }
        else if (method is ContextCardParameterMethod) { DoActionForCardParameterMethod((ContextCardParameterMethod)method); }
        else { throw new Exception("El metodo no esta definido como ejecutable"); }
    }
    private static void ShuffleContainer(ContextShuffleMethod shuffleMethod)
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
    private static DraggableCard PopContainer(ContextPopMethod contextPopMethod)
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
        else { card = assigner[containerName].Last(); card.MoveCardTo(GameObject.Find("Trash")); }
        return card;
    }
    private static string GetContainerName(ContainerReference container)
    {
        string player = "";
        if (container.Owner.Player == PlayerReference.PlayerToGet.Self) { player = Judge.GetPlayer.ToString(); }
        else if (container.Owner.Player == PlayerReference.PlayerToGet.Other) { player = Judge.GetEnemy.ToString(); }
        return container.Name + player;
    }
    private static void DoActionForCardParameterMethod(ContextCardParameterMethod method)
    {
        DraggableCard cardToPerformActionOn;
        if (method.Card is ContextPopMethod) { cardToPerformActionOn = PopContainer((ContextPopMethod)method.Card); }
        else { throw new Exception("No se ha definido la forma de evaluar la ICardReference"); }
        if (method.Type == ContextCardParameterMethod.ActionType.Push || method.Type == ContextCardParameterMethod.ActionType.SendBottom)
        {
            if (method.Container.Name == ContainerReference.ContainerToGet.Hand || method.Container.Name == ContainerReference.ContainerToGet.Graveyard)
            {
                cardToPerformActionOn.MoveCardTo(GameObject.Find(GetContainerName(method.Container)));
                if (method.Type == ContextCardParameterMethod.ActionType.SendBottom) { cardToPerformActionOn.transform.SetSiblingIndex(0); }
            }
            else if (method.Container.Name == ContainerReference.ContainerToGet.Deck)
            {
                if (method.Type == ContextCardParameterMethod.ActionType.Push) { GameObject.Find(GetContainerName(method.Container)).GetComponent<Deck>().PushCard(cardToPerformActionOn); }
                else { GameObject.Find(GetContainerName(method.Container)).GetComponent<Deck>().SendBottomCard(cardToPerformActionOn); }
            }
            else { throw new Exception("No se ha definido '" + method.Type + "' para '" + method.Container.Name + "'"); }
        }
        else { throw new Exception("No se ha definido la evaluacion de la accion: " + method.Type); }
    }
}
