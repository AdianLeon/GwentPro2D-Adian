using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Script para las DropZones de cartas de unidad
public class DZUnits : DropZone
{
    public Card.zones validZone;//Zona valida
    public Card.fields validPlayer;//Jugador valido
    public override void OnDrop(PointerEventData eventData){//Detecta cuando se suelta una carta en una zona valida
        //Cambia donde se queda la carta, en vez de quedarse en la mano ahora se queda en la zona soltada si es valida
        Card c=eventData.pointerDrag.GetComponent<Card>();
        if(c!=null){
            //Solo si coincide el tipo de carta con el tipo de dropzone y es en el campo correspondiente
            if(validZone==c.whichZone && (validPlayer==c.whichField))
            {
                c.gameObject.GetComponent<Dragging>().parentToReturnTo=this.transform;
            }
        }
    }
}
