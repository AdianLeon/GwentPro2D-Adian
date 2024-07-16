using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Script para el arrastre de las cartas
public abstract class DraggableCard : Card, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public override abstract bool IsPlayable{get;}
    private Transform parentToReturnTo=null;//Padre al que volver cuando se acaba el arrastre
    public bool IsOnHand=>transform.parent.gameObject==GameObject.Find("Hand"+this.gameObject.GetComponent<Card>().WhichPlayer);//Devuelve si la carta se encuentra en la mano
    private GameObject placeholder=null;//El espacio que deja la carta en la mano cuando se arrastra
    private static bool onDrag;//Si se esta arrastrando una carta
    public static bool IsOnDrag{get=>onDrag;}
    public void OnBeginDrag(PointerEventData eventData){//Este metodo se llama cuando empieza el arrastre de la carta
        if(IsOnHand && Judge.CanPlay){//Solo si esta en la mano y se puede jugar
            onDrag=true;//Comenzamos el arrastre
            UserRead.Show(". . .");
            //Guarda la posicion a la que volver si soltamos en lugar invalido y crea un espacio en el lugar de la carta
            placeholder=new GameObject();//Crea el placeholder y le asigna los mismos valores que a la carta y la posicion de la carta
            placeholder.transform.SetParent(this.transform.parent);
            placeholder.AddComponent<LayoutElement>();//Crea un espacio para saber donde esta el placeholder

            parentToReturnTo=this.transform.parent;//Padre al que volver, aqui guardaremos a donde la carta va cuando se suelta
            this.transform.SetParent(GameObject.Find("Canvas").transform);//Cambia el padre de la carta al canvas para que podamos moverla libremente

            //Activa la penetracion de la carta por el puntero para que podamos soltarla
            this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts=false;
        }
    }
    public void OnDrag(PointerEventData eventData){//Este metodo se llama continuamente mientras se arrastra la carta
        if(onDrag){//Solo si se esta arrastrando
            this.gameObject.GetComponent<IShowZone>()?.ShowZone();//Haciendo que las zonas donde se pueda soltar la carta "brillen"

            this.transform.position=eventData.position;//Actualiza la posicion de la carta con la del puntero
            int newSiblingIndex=parentToReturnTo.childCount;//Guarda el indice del espacio de la derecha
            for(int i=0;i<parentToReturnTo.childCount;i++){//Chequeando constantemente si se esta a la izquierda de cada una de las cartas de la mano
                if(this.transform.position.x<parentToReturnTo.GetChild(i).position.x){//Si la carta arrastrada esta a la izquierda
                    newSiblingIndex=i;//Actualiza el valor guardado con el actual
                    if(placeholder.transform.GetSiblingIndex()<newSiblingIndex){//Si la posicion del placeholder es menor que la guardada
                        newSiblingIndex--;
                    }
                    break;
                }
            }
            placeholder.transform.SetSiblingIndex(newSiblingIndex);//Pone el espacio debajo de la carta
        }
    }
    public void OnEndDrag(PointerEventData eventData){//Este metodo se llama cuando termina el arrastre de la carta
        if(onDrag){//Solo si se estaba arrastrando
            onDrag=false;//Terminamos el arrastre
            DropCardOnZone(parentToReturnTo.gameObject);//Mueve la carta a donde se determino
            if(IsOnHand){//Si la carta cae de nuevo en la mano
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());//Posiciona la carta en la mano
            }
            DestroyPlaceholder();//Destruye el espacio creado

            if(this.gameObject.GetComponent<Card>().IsPlayable){//Si las condiciones para jugar esa carta se cumplen
                Judge.PlayCard(this.gameObject);//Juega la carta
            }
            this.GetComponent<CanvasGroup>().blocksRaycasts=true;//Desactiva la penetracion de la carta para que podamos mostrarla o arrastrarla de nuevo
            GFUtils.GlowOff();//Desactivamos el glow de cualquier objeto que hayamos iluminado
        }
    }
    public void DestroyPlaceholder(){//Mueve el placeholder para que al destruirlo no deje rastros y no cuente como objeto perteneciente a Hand
        //Hacer esto es necesario para arreglar un bug referente a la cantidad de hijos en Hand en ciertos momentos
        placeholder.transform.SetParent(GameObject.Find("Trash").transform);
        Destroy(placeholder);
    }
    public void DropCardOnZone(GameObject zone){//Si la supuesta zona es valida entonces la carta ira alli
        if(zone.GetComponent<DropZone>()!=null || zone.GetComponent<IContainer>()!=null || zone.name=="Trash"){
            this.transform.SetParent(zone.transform);
            parentToReturnTo=zone.transform;
        }else{//Si la carta se envio a una zona invalida
            Debug.Log(this.gameObject.name+" tried to go to : "+zone.name+". Throwing error...");
            throw new System.Exception();
        }
    }
}