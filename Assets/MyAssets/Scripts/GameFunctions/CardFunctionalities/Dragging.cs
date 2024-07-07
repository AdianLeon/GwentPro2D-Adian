using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Script para el arrastre de las cartas y muchas otras cosas que se ejecutan junto con el arrastre
public class Dragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Variables que guardan en donde se queda la carta y su espacio
    private Transform parentToReturnTo=null;
    //public Transform ParentToReturnTo{get=>parentToReturnTo;set=>parentToReturnTo=value;}
    private GameObject GetHand{get=>GameObject.Find("Hand"+this.gameObject.GetComponent<Card>().WhichField);}
    public bool IsOnHand{get=>this.gameObject.transform.parent.gameObject==GetHand;}
    private GameObject placeholder=null;
    public GameObject Placeholder{get=>placeholder;set=>placeholder=value;}
    //Si la carta es arrastrable
    //private bool isDraggable;
    //public bool IsDraggable{get=>isDraggable;set=>isDraggable=value;}
    //Si se esta arrastrando una carta
    private static bool onDrag;
    public static bool IsOnDrag{get=>onDrag;}

    //Detecta cuando empieza el arrastre de las cartas
    public void OnBeginDrag(PointerEventData eventData){
        if(IsOnHand && Board.CanPlay){//Solo si es arrastrable y si se puede jugar
            onDrag=true;//Comenzamos el arrastre

            //Guarda la posicion a la que volver si soltamos en lugar invalido y crea un espacio en el lugar de la carta
            Placeholder=new GameObject();//Crea el placeholder y le asigna los mismos valores que a la carta y la posicion de la carta
            Placeholder.transform.SetParent(this.transform.parent);
            Placeholder.AddComponent<LayoutElement>();//Crea un espacio para saber donde esta el placeholder
            Placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());//Hace que ese espacio este en el indice correspondiente a donde estaba la carta
            parentToReturnTo=this.transform.parent;//Padre al que volver, aqui guardaremos a donde la carta va cuando se suelta
            this.transform.SetParent(GameObject.Find("Canvas").transform);//Cambia el padre de la carta al canvas para que podamos moverla libremente

            //Activa la penetracion de la carta por el puntero para que podamos soltarla
            this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts=false;
        }else{
            RoundPoints.WriteRoundInfoUserRead();
        }
    }

    //Mientras se arrastra
    public void OnDrag(PointerEventData eventData){
        if(onDrag){//Solo si es arrastrable y si se puede jugar
            //Haciendo que las zonas donde se pueda soltar la carta "brillen"
            VisualEffects.ZonesGlow(this.gameObject);
            this.transform.position=eventData.position;//Actualiza la posicion de la carta con la del puntero
            int newSiblingIndex=parentToReturnTo.childCount;//Guarda el indice del espacio de la derecha
            for(int i=0;i<parentToReturnTo.childCount;i++){//Chequeando constantemente si se ha pasado de la posicion de otra carta   
                if(this.transform.position.x<parentToReturnTo.GetChild(i).position.x){
                    newSiblingIndex=i;//Actualiza el valor guardado con el actual
                    if(Placeholder.transform.GetSiblingIndex()<newSiblingIndex){//Si la posicion del espacio es menor que la guardada
                        newSiblingIndex--;
                    }
                    break;
                }
            }
            Placeholder.transform.SetSiblingIndex(newSiblingIndex);//Pone el espacio debajo de la carta
        }
    }

    //Cuando termina el arrastre
    public void OnEndDrag(PointerEventData eventData){
        if(onDrag){//Solo si es arrastrable y si se puede jugar
            //Mueve la carta a donde se determino
            this.transform.SetParent(parentToReturnTo);

            //Caso en el que se devuelve a la mano
            if(IsOnHand){//Si la carta cae de nuevo en la mano
                this.transform.SetSiblingIndex(Placeholder.transform.GetSiblingIndex());//Posiciona la carta en la mano
                DestroyPlaceholder();//Destruye el espacio creado
                if(this.GetComponent<BaitCard>()!=null && CardView.GetSelectedCard!=null){//Si la carta jugada es senuelo y estamos sobre otra carta valida
                    this.GetComponent<BaitCard>().SwapConditions();
                }
            }else{
                DestroyPlaceholder();//Destruye el espacio creado
                if(this.transform.parent!=GameObject.Find("Trash").transform){//Si el objeto sale de la mano y no esta en la basura
                    Board.PlayCard(this.gameObject);//Independientemente del campo juega la carta
                }
            }
            onDrag=false;//Terminamos el arrastre
            this.GetComponent<CanvasGroup>().blocksRaycasts=true;//Desactiva la penetracion de la carta para que podamos mostrarla/arrastrarla de nuevo
            Hand.CheckHands();//Chequeamos si las cartas en la mano no exceden el limite

            //Cada vez que se suelte una carta necesitamos desactivar el glow de cualquier objeto que hayamos iluminado
            VisualEffects.AllGlowOff();
            RoundPoints.WriteRoundInfoUserRead();
        }
    }
    public void DestroyPlaceholder(){
        Placeholder.transform.SetParent(GameObject.Find("Trash").transform);//Mueve el placeholder para que al destruirlo
        //no deje rastros y no cuente como objeto perteneciente a Hand, esto es necesario para arreglar un bug referente a la cantidad de hijos en Hand
        Destroy(Placeholder);
    }
    public void ForcedDropOnZone(GameObject zone){//Si la supuesta zona es una zona entonces la carta ira a esa zona
        if(zone.GetComponent<DropZone>()!=null || zone.GetComponent<IContainer>()!=null){
            this.transform.SetParent(zone.transform);
            parentToReturnTo=zone.transform;
        }else{
            Debug.Log(this.gameObject.name+" tried to go to : "+zone.name);
        }
    }
    public static void GetRidOf(GameObject card){
        card.transform.SetParent(GameObject.Find("Trash").transform);
        card.GetComponent<Dragging>().parentToReturnTo=GameObject.Find("Trash").transform;
    }
}