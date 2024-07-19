using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para el intercambio de cartas con el deck propio al inicio de la partida
public class DeckTrade : DropZone
{
    public override bool IsDropValid(DraggableCard card){
        if(!Judge.IsFirstTurnOfPlayer){//Si no es el primer turno
            return false;
        }
        if(GameObject.Find("Deck"+Judge.GetPlayer).GetComponent<Deck>().GetTradeCount>1){//Si se han intercambiado dos cartas o mas
            return false;
        }
        if(gameObject.Field()!=card.GetComponent<DraggableCard>().WhichPlayer){//Si el deck no es de la carta
            return false;
        }
        return true;
    }
    public override void OnDrop(PointerEventData eventData){//Detecta cuando se suelta una carta en una zona valida
        if(!DraggableCard.IsOnDrag){//Si no se esta arrastrando
            return;
        }
        if(IsDropValid(eventData.pointerDrag.GetComponent<DraggableCard>())){
            TradeCardWithDeck(eventData.pointerDrag.GetComponent<DraggableCard>());
        }
    }
    private void TradeCardWithDeck(DraggableCard card){
        GameObject playerDeck=GameObject.Find("Deck"+Judge.GetPlayer);//Deck del jugador
        playerDeck.GetComponent<Deck>().AddCardRandomly(card);//Anade la copia de la carta a la lista del deck
        card.Disappear();//Nos deshacemos de la carta
        DraggableCard pickedCard=playerDeck.GetComponent<Deck>().DrawTopCard();
        UserRead.Write("Has cambiado a "+card.CardName+" por "+pickedCard.CardName);
        playerDeck.GetComponent<Deck>().OnCardTrade();
    }
    public override void OnGlow(){
        this.gameObject.GetComponent<Image>().color=new Color(0,1,0,0.1f);
    }
}
