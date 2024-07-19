using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Computer
{
    public static void ListenToPlayer()
    {
        if (Judge.GetPlayer != Player.P2) { return; }
        Debug.Log("Es el turno de P2 y la computadora comienza a realizar acciones----------------------------------------");
        if (!Judge.CanPlay) { Debug.Log("P2 no puede jugar"); ActEndingTurn(); return; }
        if (Judge.IsLastTurnOfRound)
        {
            Debug.Log("La computadora decide jugar todo porque es el ultimo turno de la ronda");
            PlayEverything();
        }
        else
        {
            Debug.Log("La computadora decide hacer algo");
            PlaySomething();
        }
        ActEndingTurn();
    }
    public static void PlaySomething()
    {
        List<DraggableCard> possibleActions = Hand.PlayerHandCards.Where(card => card.GetComponent<BaitCard>() == null).ToList();
        if (possibleActions.Count == 0) { Debug.Log("La computadora intenta jugar pero no quedan acciones"); return; }
        possibleActions[Random.Range(0, possibleActions.Count)].PlayCardOnRandomZone();
    }
    public static void PlayEverything()
    {
        Hand.PlayerHandCards.Where(card => card.GetComponent<BaitCard>() == null).ForEach(card => PlaySomething());
    }
    private static void PlayCardOnRandomZone(this DraggableCard card)
    {
        card.PlayCardOnZone(card.PossibleZonesToPlay()[Random.Range(0, card.PossibleZonesToPlay().Count)]);
    }
    private static void PlayCardOnZone(this DraggableCard card, DropZone zone)
    {
        Debug.Log("Computadora: Juego a " + card.name + " en la zona " + zone.name);
        card.MoveCardTo(zone.gameObject);
        if (card.IsPlayable) { card.Play(); }
        else { Debug.Log("La carta se movio para la zona pero no es jugable en ella!!"); }
    }
    public static void ActEndingTurn()
    {
        Debug.Log("La computadora termina de ejecutar acciones y termina el turno----------------------------------------");
        Judge.EndTurnOrRound();
    }

    private static List<DropZone> PossibleZonesToPlay(this DraggableCard card)
    {
        return GameObject.FindObjectsOfType<DropZone>().Where(zone => zone.IsDropValid(card) && zone.GetComponent<DeckTrade>() == null).ToList();
    }
}