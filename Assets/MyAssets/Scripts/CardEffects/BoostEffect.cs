using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : CardEffect
{
    override public void TriggerEffect(){
        GameObject target=null;//Objetivo al que anadirle 1 de poder a los hijos
        if(this.transform.parent==GameObject.Find("MyAumentoZoneM").transform){//Si la zona en la que esta la carta es x, afecta a tal zona
            target=GameObject.Find("MyMeleeDropZone");
        }else if(this.transform.parent==GameObject.Find("MyAumentoZoneR").transform){
            target=GameObject.Find("MyRangedDropZone");
        }else if(this.transform.parent==GameObject.Find("MyAumentoZoneS").transform){
            target=GameObject.Find("MySiegeDropZone");
        }else if(this.transform.parent==GameObject.Find("EnemyAumentoZoneM").transform){
            target=GameObject.Find("EnemyMeleeDropZone");
        }else if(this.transform.parent==GameObject.Find("EnemyAumentoZoneR").transform){
            target=GameObject.Find("EnemyRangedDropZone");
        }else if(this.transform.parent==GameObject.Find("EnemyAumentoZoneS").transform){
            target=GameObject.Find("EnemySiegeDropZone");
        }
        if(target!=null){
            for(int i=0;i<target.transform.childCount;i++){
                if(target.transform.GetChild(i).GetComponent<UnitCard>().wichQuality!=UnitCard.quality.Gold)     
                    target.transform.GetChild(i).GetComponent<UnitCard>().addedPower++;//Aumenta el poder en 1
            }
            Graveyard.ToGraveyard(this.gameObject);//Envia la carta usada al cementerio
        }
    }
}
