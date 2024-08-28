using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ContextUtils
{
    public static void ShuffleContainer(ContextShuffleMethod shuffleMethod)
    {
        string player = "";
        if (shuffleMethod.Container.Owner.Player == PlayerReference.PlayerToGet.Self) { player = Judge.GetPlayer.ToString(); }
        else if (shuffleMethod.Container.Owner.Player == PlayerReference.PlayerToGet.Other) { player = Judge.GetEnemy.ToString(); }
        string containerName = shuffleMethod.Container.Container.ToString() + player;
        Dictionary<string, Action> assigner = new Dictionary<string, Action>
        {
            {"Board",delegate{ ShuffleAllDirectly(new List<string>{"MeleeDropZoneP1", "RangedDropZoneP1", "SiegeDropZoneP1","MeleeDropZoneP2", "RangedDropZoneP2", "SiegeDropZoneP2","WeatherZoneM","WeatherZoneR","WeatherZoneS"}); }},
            {"HandP1",delegate{ ShuffleDirectly("HandP1"); } },
            {"HandP2",delegate{ ShuffleDirectly("HandP2"); } },
            {"FieldP1",delegate{ ShuffleAllDirectly(new List<string>{"MeleeDropZoneP1", "RangedDropZoneP1", "SiegeDropZoneP1"}); } },
            {"FieldP2",delegate{ ShuffleAllDirectly(new List<string>{"MeleeDropZoneP2", "RangedDropZoneP2", "SiegeDropZoneP2"}); } },
            {"DeckP1",delegate{ GameObject.Find("DeckP1").GetComponent<Deck>().ShuffleDeck(); } },
            {"DeckP2",delegate{ GameObject.Find("DeckP2").GetComponent<Deck>().ShuffleDeck(); } },
            {"GraveyardP1",delegate{ ShuffleDirectly("GraveyardP1"); } },
            {"GraveyardP2",delegate{ ShuffleDirectly("GraveyardP2"); } }
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
}
