using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Script para las DropZones de cartas de unidad
public class DZUnits : DropZone
{
    public ZonesDZ ValidZone{get=>(ZonesDZ)Enum.Parse(typeof(ZonesDZ),this.name[0].ToString());}
    public Player ValidPlayer{get=>GFUtils.GetField(this.name);}//Jugador valido
    public override void OnDrop(PointerEventData eventData){//Detecta cuando se suelta una carta en una zona valida
        if(!Dragging.IsOnDrag){return;}
        //Cambia donde se queda la carta, en vez de quedarse en la mano ahora se queda en la zona soltada si es valida
        UnitCard c=eventData.pointerDrag.GetComponent<UnitCard>();
        if(c!=null){
            //Solo si coincide el tipo de carta con el tipo de dropzone y es en el campo correspondiente
            if(isDropValid(c.gameObject) && (ValidPlayer==c.WhichPlayer)){
                c.gameObject.GetComponent<Dragging>().DropCardOnZone(this.gameObject);
            }
        }
    }
    public bool isDropValid(GameObject card){//Analiza si es valido soltar una carta de unidad en esta zona
        return card.GetComponent<UnitCard>().whichZone.ToString().Contains(ValidZone.ToString());//Devuelve si esta zona es una de las de la carta
    }
}
