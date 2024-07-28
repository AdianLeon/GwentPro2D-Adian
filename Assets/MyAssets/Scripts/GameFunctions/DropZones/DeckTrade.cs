using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
//Script para el intercambio de cartas con el deck propio al inicio de la partida
public class DeckTrade : DropZone, IStateSubscriber
{
    private int tradedCardsCount;
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {//Reinicia el contador de cartas intercambiadas con el deck
        new(new List<State>{State.SettingUpGame }, new Execution(stateInfo => tradedCardsCount = 0, 0))
    };
    public override bool IsDropValid(DraggableCard card)
    {
        if (Judge.TurnNumber > 2)
        {//Si no es el primer turno del jugador
            return false;
        }
        if (tradedCardsCount > 1)
        {//Si ya se han intercambiado dos cartas
            return false;
        }
        if (gameObject.Field() != card.GetComponent<DraggableCard>().Owner)
        {//Si el deck no es el de la carta
            return false;
        }
        return true;
    }
    public override void OnDropAction(DraggableCard card)
    {
        GameObject playerDeck = GameObject.Find("Deck" + Judge.GetPlayer);//Deck del jugador
        DraggableCard pickedCard = playerDeck.GetComponent<Deck>().DrawTopCard();
        card.GetComponent<CanvasGroup>().blocksRaycasts = true;//Hace que la carta bloquee los raycasts para evitar un bug
        playerDeck.GetComponent<Deck>().AddCardRandomly(card);//Anade la copia de la carta a la lista del deck
        card.Disappear();//Nos deshacemos de la carta
        if (!Computer.IsPlaying) { UserRead.Write("Has cambiado a " + card.CardName + " por " + pickedCard.CardName); }
        tradedCardsCount++;
    }
    public override void TriggerGlow() => gameObject.GetComponent<Image>().color = new Color(0, 1, 0, 0.1f);
}
