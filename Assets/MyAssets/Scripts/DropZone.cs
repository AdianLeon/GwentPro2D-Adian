using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para soltar las cartas y que se asignen a la dropzone donde corresponden
public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Accede a la propiedad de Dragging de los rangos y los campos
    public Dragging.rank cardType;
    public Dragging.fields whichField;
    //Funciones necesarias para controlar si el puntero esta encima de la carta
    public void OnPointerEnter(PointerEventData eventData)
    {}
    public void OnPointerExit(PointerEventData eventData)
    {}
    //Detecta cuando se suelta una carta en una zona valida
    public void OnDrop(PointerEventData eventData)
    {
        //Cambia donde se queda la carta, en vez de quedarse en la mano ahora se queda en la zona soltada si es valida
        Dragging d=eventData.pointerDrag.GetComponent<Dragging>();
        if(d!=null){
            //Solo si coincide el tipo de carta con el tipo de dropzone y es en el campo correspondiente
            if(cardType==d.cardType && (whichField==d.whichField || whichField==Dragging.fields.None))
            {
                d.parentToReturnTo=this.transform;
            }
        }

    }
    
}
