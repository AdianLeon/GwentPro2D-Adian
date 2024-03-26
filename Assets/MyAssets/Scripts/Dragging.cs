using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Variables que guardan en donde se queda la carta y su espacio
    public Transform parentToReturnTo=null;
    public GameObject hand;
    public GameObject placeholder=null;
    public GameObject thisCard;
    public bool isDraggable;
    //Crea tipos de rango para cada carta
    public enum rank{Melee,Ranged,Siege,Aumento,Clima,Senuelo};//********Vincular con la prop de la carta////
    public rank cardType;
    public enum fields{None,P1,P2};
    public fields whichField;

    public void Start(){
        isDraggable=true;
    }
    //Detecta cuando empieza el arrastre de las cartas
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(isDraggable){
            //Guarda la posicion a la que volver si soltamos en lugar invalido y crea un espacio en el lugar de la carta
            placeholder=new GameObject();//Crea el placeholder y le asigna los mismos valores que a la carta y la posicion de la carta
            placeholder.transform.SetParent(this.transform.parent);

            LayoutElement le=placeholder.AddComponent<LayoutElement>();//Crea un espacio para saber donde esta el placeholder
            le.preferredWidth=this.GetComponent<LayoutElement>().preferredWidth;
            le.preferredHeight=this.GetComponent<LayoutElement>().preferredHeight;

            placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
            parentToReturnTo=this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);

            //Activa la penetracion de la carta por el puntero para que podamos soltarla
            GetComponent<CanvasGroup>().blocksRaycasts=false;
        }
    }

    //Mientras se arrastra
    public void OnDrag(PointerEventData eventData)
    {
        if(isDraggable){
            this.transform.position=eventData.position;//Actualiza la posicion de la carta con la del puntero

            int newSiblingIndex=parentToReturnTo.childCount;//Guarda el indice del espacio de la derecha
            for(int i=0;i<parentToReturnTo.childCount;i++)//Chequeando constantemente si se ha pasado de la posicion de otra carta
            {   
                if(this.transform.position.x<parentToReturnTo.GetChild(i).position.x)
                {
                    newSiblingIndex=i;//Actualiza el valor guardado con el actual
                    if(placeholder.transform.GetSiblingIndex()<newSiblingIndex)//Si la posicion del espacio es menor que la guardada
                    {
                        newSiblingIndex--;
                    }
                    break;
                }
            }
            placeholder.transform.SetSiblingIndex(newSiblingIndex);//Pone el espacio debajo de la carta
        }
    }

    //Cuando termina el arrastre pone la carta en donde guardamos
    public void OnEndDrag(PointerEventData eventData)
    {
    if(isDraggable){
            if(TurnManager.CardsPlayed<1 || TurnManager.lastTurn){//Solo una carta por turno

                this.transform.SetParent(parentToReturnTo);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());//Posiciona la carta en el espacio
                
                GetComponent<CanvasGroup>().blocksRaycasts=true;//Desactiva la penetracion de la carta para que podamos arrastrarla de nuevo
                
                Destroy(placeholder);//Destruye el espacio creado
                if(cardType==rank.Senuelo){
                    //Obteniendo el objeto al que el mouse apunta
                    GameObject Choosed=null;
                    if(whichField==fields.P1){
                        for(int i=0;i<TotalFieldForce.P1PlayedCards.Count;i++){
                            if(CardView.cardName==TotalFieldForce.P1PlayedCards[i].name){
                                Choosed=TotalFieldForce.P1PlayedCards[i];
                                Effects.SwapObjects(this.gameObject,Choosed);
                                Choosed.GetComponent<Dragging>().isDraggable=true;
                                isDraggable=false;
                                TotalFieldForce.P1PlayedCards.Add(thisCard);
                                TotalFieldForce.P1PlayedCards.Remove(Choosed);
                                TurnManager.PlayCard(thisCard);
                                break;
                            }
                        }
                    }else if(whichField==fields.P2){
                        for(int i=0;i<TotalFieldForce.P2PlayedCards.Count;i++){
                            if(CardView.cardName==TotalFieldForce.P2PlayedCards[i].name){
                                Choosed=TotalFieldForce.P2PlayedCards[i];
                                Effects.SwapObjects(this.gameObject,Choosed);
                                Choosed.GetComponent<Dragging>().isDraggable=true;
                                isDraggable=false;
                                TotalFieldForce.P2PlayedCards.Add(thisCard);
                                TotalFieldForce.P2PlayedCards.Remove(Choosed);
                                TurnManager.PlayCard(thisCard);
                                break;
                            }
                        }
                    }
                }else if(this.transform.parent!=hand.transform && this.transform.parent!=GameObject.Find("Trash").transform){//Si el objeto sale de la mano y no esta en la basura

                    isDraggable=false;//Quita la propiedad isDraggable de la carta

                    if(whichField==fields.P1){//Si es campo de P1 anade la carta a la lista de cartas del campo del P2
                        TotalFieldForce.P1PlayedCards.Add(thisCard);
                    }else if(whichField==fields.P2){//Si es campo de P2 anade la carta a la lista de cartas del campo del P2
                        TotalFieldForce.P2PlayedCards.Add(thisCard);
                    }
                    TurnManager.PlayCard(thisCard);//Independientemente del campo juega la carta
                }else{
                    GetComponent<CanvasGroup>().blocksRaycasts=true;//Desactiva la penetracion de la carta para que podamos arrastrarla de nuevo
                }
            }else{
                this.transform.SetParent(hand.transform);//Devuelve la carta a la mano

                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());//Posiciona la carta en el espacio
                
                GetComponent<CanvasGroup>().blocksRaycasts=true;//Desactiva la penetracion de la carta para que podamos arrastrarla de nuevo
                
                Destroy(placeholder);//Destruye el espacio creado
            }
            TotalFieldForce.UpdateForce();
        }
    }
}
