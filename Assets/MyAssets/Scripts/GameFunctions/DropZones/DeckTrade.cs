using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para el intercambio de cartas con el deck propio al inicio de la partida
public class DeckTrade : DropZone
{//Script para intercambiar hasta 2 cartas de la mano con una random del deck
    private int tradeCount;//Para controlar cuantas cartas ha cambiado el jugador
    public static void ResetTradeCount(){
        DeckTrade[] deckTrades=GameObject.FindObjectsOfType<DeckTrade>();
        foreach(DeckTrade deckTrade in deckTrades){deckTrade.tradeCount=0;}
    }
    private bool CheckValidTrade{get=>Judge.GetTurnNumber==1 && tradeCount<2;}//Solo se puede intercambiar dos veces como maximo y si es el primer turno
    public override bool IsDropValid(GameObject card){
        if(!CheckValidTrade){//Si no se puede intercambiar
            return false;
        }
        if(GFUtils.GetField(this.name)!=card.GetComponent<Card>().WhichPlayer){//Si el deck no es de la carta
            return false;
        }
        return true;
    }
    public override void OnDrop(PointerEventData eventData){//Detecta cuando se suelta una carta en una zona valida
        if(!Dragging.IsOnDrag){//Si no se esta arrastrando
            return;
        }
        if(IsDropValid(eventData.pointerDrag)){
            TradeCardWithDeck(eventData.pointerDrag);
        }
    }
    private void TradeCardWithDeck(GameObject card){
        GameObject playerDeck=GameObject.Find("Deck"+Judge.GetPlayer);//Deck del jugador
        playerDeck.GetComponent<Deck>().AddCardRandomly(card);//Anade la copia de la carta a la lista del deck
        GFUtils.GetRidOf(card);//Nos deshacemos de la carta
        GameObject pickedCard=playerDeck.GetComponent<Deck>().DrawTopCard();
        GFUtils.UserRead.Write("Has cambiado a "+card.GetComponent<Card>().CardName+" por "+pickedCard.GetComponent<Card>().CardName);
        tradeCount++;
    }
    public override void OnGlow(){
        this.gameObject.GetComponent<Image>().color=new Color(0,1,0,0.1f);
    }
}
