using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private static bool isPlaying;
    public static bool IsPlaying => isPlaying;
    public void CheckPlayConditions()
    {
        if (PlayerPrefs.GetInt("SinglePlayerMode") == 0) { return; }//Si el modo un solo jugador esta desactivado
        if (Judge.GetPlayer != Player.P2) { return; }//Si no es el turno de P2
        if (!Judge.CanPlay) { return; }//Si no se puede jugar
        Play();
    }
    public bool Play()
    {//Juega algunas cartas en dependencia de si es el ultimo turno de la ronda o no
        foreach (BaitCard baitCard in Hand.PlayerHandCards.Where(card => card.GetComponent<BaitCard>() != null))
        {
            if (GameObject.Find("DeckZoneP2").GetComponent<DeckTrade>().IsDropValid(baitCard)) { GameObject.Find("DeckZoneP2").GetComponent<DeckTrade>().TradeCardWithDeck(baitCard); }
        }
        isPlaying = true;
        Debug.Log("Es el turno de P2 y la computadora decide jugar----------------------------------------");
        if (Judge.IsLastTurnOfRound)
        {
            Debug.Log("La computadora decide jugar todas las cartas porque es el ultimo turno de la ronda");
            PlayAllCards();
        }
        else
        {
            Debug.Log("La computadora decide jugar una carta");
            DoAfterTime(delegate { PlayOneCard(); ActEndingTurn(); }, UnityEngine.Random.Range(1, 5));
        }
        return true;
    }


    private void PlayAllCards()
    {//Juega alguna de las cartas por cada carta que se puede jugar, o sea todas
        DraggableCard[] actions = Hand.PlayerHandCards.Where(card => card.GetComponent<BaitCard>() == null).ToArray();
        for (int i = 1; i <= actions.Length; i++) { DoAfterTime(delegate { PlayOneCard(); }, i * UnityEngine.Random.Range(1, 3)); }
        DoAfterTime(delegate { ActEndingTurn(); }, actions.Length * 2);
    }
    private void PlayOneCard()
    {//Juega alguna de las cartas que puede jugar
        List<DraggableCard> possibleActions = Hand.PlayerHandCards.Where(card => card.GetComponent<BaitCard>() == null).ToList();
        if (possibleActions.Count == 0) { Debug.Log("La computadora intenta jugar pero no quedan acciones"); return; }
        DraggableCard chosenCard = possibleActions[UnityEngine.Random.Range(0, possibleActions.Count)];
        DropZone chosenZone = PossibleZonesToPlay(chosenCard)[UnityEngine.Random.Range(0, PossibleZonesToPlay(chosenCard).Count)];
        UserRead.Write("El enemigo ha jugado a " + chosenCard.CardName + " en la zona: " + chosenZone.name);
        chosenCard.TryPlayCardIn(chosenZone);
    }
    private void ActEndingTurn()
    {//Termina el turno
        Debug.Log("La computadora termina de ejecutar acciones y termina el turno----------------------------------------");
        Judge.EndTurnOrRound();
        isPlaying = false;
    }
    private void DoAfterTime(Action action, int time)
    {
        StartCoroutine(CorroutineDoAfterTime(action, time));
    }
    private IEnumerator CorroutineDoAfterTime(Action action, int time)
    {
        GFUtils.FindGameObjectsOfType<HandCover>().ForEach(cover => cover.gameObject.SetActive(true));
        yield return new WaitForSeconds(time);
        action();
    }
    private List<DropZone> PossibleZonesToPlay(DraggableCard card)
    {//Devuelve las zonas donde la carta se puede jugar
        return GameObject.FindObjectsOfType<DropZone>().Where(zone => zone.IsDropValid(card)).ToList();
    }
}