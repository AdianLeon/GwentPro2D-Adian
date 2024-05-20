using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Script para las DropZones de cartas de unidad
public class DZUnits : DropZone
{
    public enum zonesDZ{M,R,S};//Zona valida
    public zonesDZ validZone;
    public Card.fields validPlayer;//Jugador valido
    public override void OnDrop(PointerEventData eventData){//Detecta cuando se suelta una carta en una zona valida
        //Cambia donde se queda la carta, en vez de quedarse en la mano ahora se queda en la zona soltada si es valida
        UnitCard c=eventData.pointerDrag.GetComponent<UnitCard>();
        if(c!=null){
            //Solo si coincide el tipo de carta con el tipo de dropzone y es en el campo correspondiente
            if(isDropValid(c.gameObject) && (validPlayer==c.whichField)){
                c.gameObject.GetComponent<Dragging>().parentToReturnTo=this.transform;
            }
        }
    }
    public bool isDropValid(GameObject card){
        if(card.GetComponent<UnitCard>().whichZone.ToString().Length==1){
            return validZone.ToString()==card.GetComponent<UnitCard>().whichZone.ToString();
        }else{
            string cardZones=card.GetComponent<UnitCard>().whichZone.ToString();
            for(int i=0;i<cardZones.Length;i++){
                if(validZone.ToString()==cardZones[i].ToString()){
                    return true;
                }
            }
            return false;
        }
    }
}
