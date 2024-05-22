using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para las cartas de aumento
public class BoostEffect : CardEffect
{
    override public void TriggerEffect(){
        GameObject target=this.transform.parent.GetComponent<DZBoost>().target;//Objetivo padre de las cartas a las que anadirle poder
        for(int i=0;i<target.transform.childCount;i++){
            if(target.transform.GetChild(i).GetComponent<UnitCard>()!=null && target.transform.GetChild(i).GetComponent<UnitCard>().wichQuality==UnitCard.quality.Gold){//Si es de oro
                continue;//No se afecta
            }
            target.transform.GetChild(i).GetComponent<CardWithPower>().addedPower+=this.GetComponent<BoostCard>().boost;//Aumenta el poder
        }
        Graveyard.ToGraveyard(this.gameObject);//Envia la carta usada al cementerio
    }
}
