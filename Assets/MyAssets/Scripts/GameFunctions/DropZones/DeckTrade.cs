using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
//Script para el intercambio de cartas con el deck propio al inicio de la partida
public class DeckTrade : DropZone, IStateSubscriber
{
    private int tradedCardsCount;
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {//Reinicia el contador de cartas intercambiadas con el deck
        new(new List<State>{State.SettingUpGame }, new Execution(stateInfo => tradedCardsCount = 0, 0))
    };
    public override bool IsDropValid(DraggableCard card) => Judge.TurnNumber < 3 && tradedCardsCount < 2 && gameObject.Field() == card.GetComponent<DraggableCard>().Owner;
    public override void OnDropAction(DraggableCard card)
    {//Cambia la carta dropeada por una nueva del deck
        GameObject playerDeck = GameObject.Find("Deck" + Judge.GetPlayer);//Deck del jugador
        DraggableCard pickedCard = playerDeck.GetComponent<Deck>().DrawTopCard();
        playerDeck.GetComponent<Deck>().AddCardRandomly(card);//Anade la copia de la carta a la lista del deck
        card.Disappear();//Nos deshacemos de la carta original
        if (!Computer.IsPlaying) { UserRead.Write("Has cambiado a " + card.CardName + " por " + pickedCard.CardName); }
        tradedCardsCount++;
    }
    public override void TriggerGlow() => gameObject.GetComponent<Image>().color = new Color(0, 1, 0, 0.1f);
}
