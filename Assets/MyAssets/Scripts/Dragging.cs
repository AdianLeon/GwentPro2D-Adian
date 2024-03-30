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
    //public GameObject thisCard;
    public bool isDraggable;
    public static bool onDrag;
    //Crea tipos de rango para cada carta
    public enum rank{Melee,Ranged,Siege,Aumento,Clima,Senuelo,Despeje};//********Vincular con la prop de la carta////
    public rank cardType;
    public enum fields{None,P1,P2};
    public fields whichField;

    public void Start(){
        if(whichField==fields.P1){
            hand=GameObject.Find("Hand");
        }else if(whichField==fields.P2){
            hand=GameObject.Find("EnemyHand");
        }
        isDraggable=true;
    }
    //Detecta cuando empieza el arrastre de las cartas
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(isDraggable){
            onDrag=true;
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

            //Haciendo que las zonas donde se pueda soltar la carta "brillen"
            Effects.ZonesGlow(this.gameObject);
        }
    }

    //Mientras se arrastra
    public void OnDrag(PointerEventData eventData)
    {
        if(isDraggable){
            this.transform.position=eventData.position;//Actualiza la posicion de la carta con la del puntero
            int newSiblingIndex=parentToReturnTo.childCount;//Guarda el indice del espacio de la derecha
            for(int i=0;i<parentToReturnTo.childCount;i++){//Chequeando constantemente si se ha pasado de la posicion de otra carta   
                if(this.transform.position.x<parentToReturnTo.GetChild(i).position.x){
                    newSiblingIndex=i;//Actualiza el valor guardado con el actual
                    if(placeholder.transform.GetSiblingIndex()<newSiblingIndex){//Si la posicion del espacio es menor que la guardada
                        newSiblingIndex--;
                    }
                    break;
                }
            }
            placeholder.transform.SetSiblingIndex(newSiblingIndex);//Pone el espacio debajo de la carta
        }
        //Si la carta es senuelo la zona que "brilla" es la de la carta seleccionada
        if(this.cardType==rank.Senuelo){
            for(int i=0;i<TurnManager.PlayedCards.Count;i++){
                if(TurnManager.PlayedCards[i].name!=CardView.cardName){
                    TurnManager.PlayedCards[i].transform.parent.GetComponent<Image>().color=new Color (1,1,1,0);
                }
            }
            for(int i=0;i<TurnManager.PlayedCards.Count;i++){
                if(TurnManager.PlayedCards[i].name==CardView.cardName && TurnManager.PlayedCards[i].GetComponent<Dragging>().whichField==whichField){
                    TurnManager.PlayedCards[i].transform.parent.GetComponent<Image>().color=new Color (1,1,1,0.1f);
                    break;
                }
            }
        }
    }

    //Cuando termina el arrastre pone la carta en donde guardamos
    public void OnEndDrag(PointerEventData eventData){
        if(isDraggable){
            if(TurnManager.CardsPlayed<1 || TurnManager.lastTurn){//Solo cuando no se ha jugado una carta o si es el ultimo turno
                this.transform.SetParent(parentToReturnTo);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());//Posiciona la carta en el espacio
                GetComponent<CanvasGroup>().blocksRaycasts=true;//Desactiva la penetracion de la carta para que podamos arrastrarla de nuevo
                Destroy(placeholder);//Destruye el espacio creado
                if(cardType==rank.Senuelo){//Efecto del senuelo
                    Effects.Senuelo(this.gameObject);
                }else if(this.transform.parent!=hand.transform && this.transform.parent!=GameObject.Find("Trash").transform){//Si el objeto sale de la mano y no esta en la basura
                    TotalFieldForce.AddCard(this.gameObject);//Anade la carta segun el campo y el tipo
                    TurnManager.PlayCard(this.gameObject);//Independientemente del campo juega la carta
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
            //Cada vez que se suelte una carta necesitamos desactivar el glow de cualquier zona que hayamos iluminado
            Effects.OffZonesGlow();
            
            onDrag=false;
        }
    }
}
