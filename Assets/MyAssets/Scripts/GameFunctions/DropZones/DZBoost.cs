using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Script para las DropZones de cartas de aumento
public class DZBoost : DropZone
{
    public GameObject target;//Objetivo del efecto aumento
    public Card.fields validPlayer;//Jugador valido
    public override void OnDrop(PointerEventData eventData){//Detecta cuando se suelta una carta en una zona valida
        //Cambia donde se queda la carta, en vez de quedarse en la mano ahora se queda en la zona soltada si es valida
        BoostCard c=eventData.pointerDrag.GetComponent<BoostCard>();
        if(c!=null){
            //Solo si son cartas de aumento del jugador correspondiente
            if(validPlayer==c.whichField){
                c.gameObject.GetComponent<Dragging>().parentToReturnTo=this.transform;
            }
        }
    }
}
