using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para las cartas de aumento
public class BoostEffect : CardEffect
{
    override public void TriggerEffect(){
        GameObject target=this.transform.parent.GetComponent<DZBoost>().target;//Objetivo al que anadirle 1 de poder a los hijos
        for(int i=0;i<target.transform.childCount;i++){
            if(target.transform.GetChild(i).GetComponent<UnitCard>().wichQuality!=UnitCard.quality.Gold)     
                target.transform.GetChild(i).GetComponent<UnitCard>().addedPower++;//Aumenta el poder en 1
        }
        Graveyard.ToGraveyard(this.gameObject);//Envia la carta usada al cementerio
    }
}
