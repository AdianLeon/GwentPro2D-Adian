using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para el intercambio de cartas con el deck propio al inicio de la partida
public class ExtraDrawCard : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{//Script para intercambiar hasta 2 cartas de la mano con una random del deck
    public static bool firstTurn=true;//Solo es valido el intercambio para los primeros turnos
    public static bool firstAction=true;
    public static int twice;//Para controlar cuantas cartas lleva el jugador
    public static bool redrawable=true;
    public Dragging.fields whichField;//Campos de dragging
    //Funciones necesarias para controlar si el puntero esta encima de la carta
    public void OnPointerEnter(PointerEventData eventData)
    {}
    public void OnPointerExit(PointerEventData eventData)
    {}
    public static void UpdateRedraw(){
        redrawable=firstAction && firstTurn && twice<2;
    }
    //Detecta cuando se suelta una carta en una zona valida
    public void OnDrop(PointerEventData eventData){
        if(redrawable){//Solo se ejecuta si han sido menos de 2 y si es el primer turno
            Dragging d=eventData.pointerDrag.GetComponent<Dragging>();//Componente Dragging de la carta
            if(d!=null){//Si el componente se encuentra
                if(whichField==d.whichField)//Si se esta soltando en el deck del campo correcto
                {
                    Destroy(d.placeholder);//Destruimos el placeholder
                    GameObject playerDeck=null;//Esta variable guardara el deck del jugador

                    if(d.whichField==Dragging.fields.P1){//Obtiene el deck correcto
                        playerDeck=GameObject.Find("Deck");
                        if(playerDeck!=null)
                            playerDeck.GetComponent<DrawCards>().cards.Add(d.gameObject);//Anade la copia de la carta a la lista del deck

                        d.gameObject.transform.SetParent(GameObject.Find("Trash").transform);//Envia la carta a intercambiar afuera de la escena
                        d.parentToReturnTo=GameObject.Find("Trash").transform;//Destruye el componente dragging para que no vuelva a la mano, como sea no la vamos a usar de nuevo
                        //Esto se hace asi porque si se destruye la carta se destruye lo que anadimos a la lista del deck

                        GameObject picked=playerDeck.GetComponent<DrawCards>().cards[Random.Range(0,playerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
                        GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
                        Card.transform.SetParent(playerDeck.GetComponent<DrawCards>().PlayerArea.transform,false);//Se pone en la mano
                        Card.GetComponent<CanvasGroup>().blocksRaycasts=true;//Esto es importante, permite asegurar que se puede arrastrar la carta
                        playerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista
                        twice++;//Controla cuantas veces has intercambiado

                    }else if(d.whichField==Dragging.fields.P2){
                        playerDeck=GameObject.Find("EnemyDeck");
                        if(playerDeck!=null)
                            playerDeck.GetComponent<DrawCards>().cards.Add(d.gameObject);//Anade la copia de la carta a la lista del deck

                        d.gameObject.transform.SetParent(GameObject.Find("Trash").transform);//Envia la carta a intercambiar afuera de la escena
                        d.parentToReturnTo=GameObject.Find("Trash").transform;//Destruye el componente dragging para que no vuelva a la mano, como sea no la vamos a usar de nuevo
                        //Esto se hace asi porque si se destruye la carta se destruye lo que anadimos a la lista del deck

                        GameObject picked=playerDeck.GetComponent<DrawCards>().cards[Random.Range(0,playerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
                        GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
                        Card.transform.SetParent(playerDeck.GetComponent<DrawCards>().PlayerArea.transform,false);//Se pone en la mano
                        Card.GetComponent<CanvasGroup>().blocksRaycasts=true;//Esto es importante, permite asegurar que se puede arrastrar la carta
                        playerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista
                        twice++;//Controla cuantas veces has intercambiado

                    }


                }
            }
        }
    }
}
