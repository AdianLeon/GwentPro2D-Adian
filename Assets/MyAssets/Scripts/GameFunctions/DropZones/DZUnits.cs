using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
//Script para las DropZones de cartas de unidad
public class DZUnits : DropZone
{
    public UnitDropZoneType GetValidZone{get=>(UnitDropZoneType)Enum.Parse(typeof(UnitDropZoneType),this.name[0].ToString());}
    public override bool IsDropValid(GameObject card){
        if(card.GetComponent<UnitCard>()==null){//Si no es carta de unidad
            return false;
        }
        if(gameObject.Field()!=card.GetComponent<Card>().WhichPlayer){//Si los campos no coinciden
            return false;
        }
        if(!card.GetComponent<UnitCard>().WhichZone.ToString().Contains(GetValidZone.ToString())){//Si esta zona no es una de las de la carta
            return false;
        }
        return true;
    }
}
