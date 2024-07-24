using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para el intercambio de cartas con el deck propio al inicio de la partida
public class DeckTrade : DropZone, IStateListener
{
    private int tradedCardsCount;
    public int GetPriority => 0;
    public void CheckState() { if (Judge.CurrentState == State.SettingUpGame) { tradedCardsCount = 0; } }//Reinicia el contador de cartas intercambiadas con el deck
    public override bool IsDropValid(DraggableCard card)
    {
        if (!Judge.IsFirstTurnOfPlayer)
        {//Si no es el primer turno
            return false;
        }
        if (tradedCardsCount > 1)
        {//Si ya se han intercambiado dos cartas
            return false;
        }
        if (gameObject.Field() != card.GetComponent<DraggableCard>().WhichPlayer)
        {//Si el deck no es el de la carta
            return false;
        }
        return true;
    }
    public override void OnDrop(PointerEventData eventData)
    {//Detecta cuando se suelta una carta en una zona valida
        if (!DraggableCard.IsOnDrag) { return; }//Si no se esta arrastrando
        if (IsDropValid(eventData.pointerDrag.GetComponent<DraggableCard>())) { TradeCardWithDeck(eventData.pointerDrag.GetComponent<DraggableCard>()); }
    }
    public void TradeCardWithDeck(DraggableCard card)
    {
        GameObject playerDeck = GameObject.Find("Deck" + Judge.GetPlayer);//Deck del jugador
        card.GetComponent<CanvasGroup>().blocksRaycasts = true;//Hace que la carta bloquee los raycasts para evitar un bug
        playerDeck.GetComponent<Deck>().AddCardRandomly(card);//Anade la copia de la carta a la lista del deck
        card.Disappear();//Nos deshacemos de la carta
        DraggableCard pickedCard = playerDeck.GetComponent<Deck>().DrawTopCard();
        UserRead.Write("Has cambiado a " + card.CardName + " por " + pickedCard.CardName);
        tradedCardsCount++;
    }
    public override void OnGlow()
    {
        this.gameObject.GetComponent<Image>().color = new Color(0, 1, 0, 0.1f);
    }
}
