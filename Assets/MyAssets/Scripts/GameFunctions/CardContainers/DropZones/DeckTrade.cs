using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para el intercambio de cartas con el deck propio al inicio de la partida
public class DeckTrade : DropZone
{//Script para intercambiar hasta 2 cartas de la mano con una random del deck
    private static bool isFirstTurn;//Solo es valido el intercambio para los primeros turnos
    public static bool SetFirstTurn{set=>isFirstTurn=value;}
    private static bool isFirstAction;
    public static bool SetFirstAction{set=>isFirstAction=value;}
    private static int tradeCount;//Para controlar cuantas cartas ha cambiado el jugador
    public static void ResetTradeCount(){tradeCount=0;}
    public static bool CheckValidTrade{get=>isFirstAction && isFirstTurn && tradeCount<2;}
    public Fields whichField;//Campos de dragging
    void Start(){
        isFirstTurn=true;
        isFirstAction=true;
        tradeCount=0;
    }
    //Detecta cuando se suelta una carta en una zona valida
    public override void OnDrop(PointerEventData eventData){
        if(CheckValidTrade){//Solo se ejecuta si han sido menos de 2 y si es el primer turno
            Dragging d=eventData.pointerDrag.GetComponent<Dragging>();//Componente Dragging de la carta
            Card c=eventData.pointerDrag.GetComponent<Card>();//Componente Card de la carta
            if(whichField==c.WhichField)//Si se esta soltando en el deck del campo correcto
            {
                
                d.DestroyPlaceholder();//Destruimos el placeholder
                GameObject playerDeck=GameObject.Find("Deck"+Board.GetPlayerTurn);//Deck del jugador

                playerDeck.GetComponent<Deck>().AddCardRandomly(d.gameObject);//Anade la copia de la carta a la lista del deck

                d.gameObject.transform.SetParent(GameObject.Find("Trash").transform);//Envia la carta a intercambiar afuera de la escena
                d.ParentToReturnTo=GameObject.Find("Trash").transform;

                GameObject pickedCard=playerDeck.GetComponent<Deck>().DrawTopCard();

                pickedCard.GetComponent<CanvasGroup>().blocksRaycasts=true;//Permite asegurar que se puede arrastrar la carta

                RoundPoints.LongWriteUserRead("Has cambiado a "+c.cardName+" por "+pickedCard.GetComponent<Card>().cardName);
                
                tradeCount++;//Controla cuantas veces se ha intercambiado
            }
        }
    }
    public override void OnGlow(){
        this.gameObject.GetComponent<Image>().color=new Color(0,1,0,0.1f);
    }
}
