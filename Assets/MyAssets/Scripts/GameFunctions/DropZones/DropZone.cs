using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para soltar las cartas y que se asignen a la dropzone donde corresponden si es la correcta
public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData){//Cuando el mouse esta en la zona
        // if(!Dragging.onDrag){
        //     this.gameObject.GetComponent<Image>().color= new Color (1,1,1,0.1f);//La zona se ilumina
        // }
    }
    public void OnPointerExit(PointerEventData eventData){//Cuando el mouse sale de la zona
        // if(!Dragging.onDrag){
        // this.gameObject.GetComponent<Image>().color= new Color (1,1,1,0);//Se deja de iluminar
        // }
    }
    public virtual void OnDrop(PointerEventData eventData){}//Detecta cuando se suelta una carta en una zona valida
    //Esta funcion varia en dependencia de la DropZone, por ello se desarrolla en los scripts que heredan de este
}
