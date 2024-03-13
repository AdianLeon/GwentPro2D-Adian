using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Variables que guardan en donde se queda la carta y su espacio
    public Transform parentToReturnTo=null;
    GameObject placeholder=null;
    //Crea tipos de rango para cada carta
    public enum rank{Melee,Ranged,Siege,Aumento,Clima,Despeje,Senuelo,Dead};//********Vincular con la prop de la carta////
    public rank cardType;
    //Detecta cuando empieza el arrastre de las cartas
    public void OnBeginDrag(PointerEventData eventData)
    {
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

    //Mientras se arrastra
    public void OnDrag(PointerEventData eventData)
    {
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

    //Cuando termina el arrastre pone la carta en donde guardamos
    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());//Posiciona la carta en el espacio
        //Desactiva la penetracion de la carta para que podamos arrastrarla de nuevo
        GetComponent<CanvasGroup>().blocksRaycasts=true;
        //Destruye el espacio creado
        Destroy(placeholder);
        //if(parentToReturnTo!=Hand){
            //DisplayCard.card.PlayCard();
        //}
    }
}
