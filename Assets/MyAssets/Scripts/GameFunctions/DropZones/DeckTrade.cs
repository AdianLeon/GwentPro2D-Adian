using System.Collections;
using System.Collections.Generic;
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
    public bool CheckValidTrade{get=>Judge.GetTurnNumber==1 && tradeCount<2;}
    //Detecta cuando se suelta una carta en una zona valida
    public override void OnDrop(PointerEventData eventData){
        if(!Dragging.IsOnDrag){return;}
        Card c=eventData.pointerDrag.GetComponent<Card>();//Componente Card de la carta
        
        if(CheckValidTrade && GFUtils.GetField(this.name)==c.WhichPlayer){//Solo se ejecuta si han sido menos de 2 y si es el primer turno
            Dragging d=eventData.pointerDrag.GetComponent<Dragging>();//Componente Dragging de la carta
            
            //d.DestroyPlaceholder();//Destruimos el placeholder
            GameObject playerDeck=GameObject.Find("Deck"+Judge.GetPlayer);//Deck del jugador

            playerDeck.GetComponent<Deck>().AddCardRandomly(d.gameObject);//Anade la copia de la carta a la lista del deck
            Dragging.GetRidOf(c.gameObject);//Nos deshacemos de la carta

            GameObject pickedCard=playerDeck.GetComponent<Deck>().DrawTopCard();
            GFUtils.UserRead.LongWrite("Has cambiado a "+c.CardName+" por "+pickedCard.GetComponent<Card>().CardName);
            
            tradeCount++;//Controla cuantas veces se ha intercambiado
        }
    }
    public override void OnGlow(){
        if(CheckValidTrade){//Si se pueden intercambiar cartas con el deck
            this.gameObject.GetComponent<Image>().color=new Color(0,1,0,0.1f);
        }
    }
}
