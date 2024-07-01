using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para el intercambio de cartas con el deck propio al inicio de la partida
public class DeckTrade : DropZone
{//Script para intercambiar hasta 2 cartas de la mano con una random del deck
    public static bool firstTurn;//Solo es valido el intercambio para los primeros turnos
    public static bool firstAction;
    public static int twice;//Para controlar cuantas cartas lleva el jugador
    public static bool redrawable;
    public fields whichField;//Campos de dragging
    void Start(){
        firstTurn=true;
        firstAction=true;
        twice=0;
        redrawable=true;
    }
    public static bool UpdateRedraw(){
        redrawable=firstAction && firstTurn && twice<2;
        return redrawable;
    }
    //Detecta cuando se suelta una carta en una zona valida
    public override void OnDrop(PointerEventData eventData){
        if(redrawable){//Solo se ejecuta si han sido menos de 2 y si es el primer turno
            Dragging d=eventData.pointerDrag.GetComponent<Dragging>();//Componente Dragging de la carta
            Card c=eventData.pointerDrag.GetComponent<Card>();//Componente Card de la carta
            if(whichField==c.WhichField)//Si se esta soltando en el deck del campo correcto
            {
                
                d.DestroyPlaceholder();//Destruimos el placeholder
                GameObject playerDeck=GameObject.Find("Deck"+TurnManager.PTurn);//Deck del jugador

                playerDeck.GetComponent<DrawCards>().cardsInDeck.Add(d.gameObject);//Anade la copia de la carta a la lista del deck

                d.gameObject.transform.SetParent(GameObject.Find("Trash").transform);//Envia la carta a intercambiar afuera de la escena
                d.parentToReturnTo=GameObject.Find("Trash").transform;

                GameObject picked=playerDeck.GetComponent<DrawCards>().cardsInDeck[Random.Range(0,playerDeck.GetComponent<DrawCards>().cardsInDeck.Count)];//Escoge una carta aleatoria del deck
                GameObject pickedCard = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia una copia de esa escogida
                pickedCard.transform.SetParent(playerDeck.GetComponent<DrawCards>().playerArea.transform,false);//Se pone en la mano
                pickedCard.GetComponent<CanvasGroup>().blocksRaycasts=true;//Permite asegurar que se puede arrastrar la carta
                playerDeck.GetComponent<DrawCards>().cardsInDeck.Remove(picked);//Se quita de la lista

                RoundPoints.URLongWrite("Has cambiado a "+c.cardRealName+" por "+pickedCard.GetComponent<Card>().cardRealName);
                
                twice++;//Controla cuantas veces se ha intercambiado
            }
        }
    }
}
