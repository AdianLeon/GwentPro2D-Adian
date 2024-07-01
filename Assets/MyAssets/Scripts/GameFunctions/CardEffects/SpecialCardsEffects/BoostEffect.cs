using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para las cartas de aumento
public class BoostEffect : MonoBehaviour, ICardEffect
{
    public void TriggerEffect(){
        GameObject target=this.transform.parent.GetComponent<DZBoost>().target;//Objetivo padre de las cartas a las que anadirle poder
        for(int i=0;i<target.transform.childCount;i++){
            if(target.transform.GetChild(i).GetComponent<IAffectable>()!=null){//Si es afectable
                target.transform.GetChild(i).GetComponent<CardWithPower>().addedPower+=this.GetComponent<BoostCard>().boost;//Aumenta el poder
            }
        }
        Graveyard.ToGraveyard(this.gameObject);//Envia la carta usada al cementerio
    }
}
