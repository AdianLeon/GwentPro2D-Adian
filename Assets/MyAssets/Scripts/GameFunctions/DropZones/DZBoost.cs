using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Script para las DropZones de cartas de aumento
public class DZBoost : DropZone
{
    public GameObject Target;//Objetivo del efecto aumento
    public override bool IsDropValid(GameObject card){
        if(card.GetComponent<BoostCard>()==null){//Si la carta no es un aumento
            return false;
        }
        if(gameObject.Field()!=card.GetComponent<Card>().WhichPlayer){//Si es del jugador incorrecto
            return false;
        }
        return true;
    }
}
