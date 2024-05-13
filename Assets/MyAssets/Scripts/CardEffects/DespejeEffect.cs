using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespejeEffect : CardEffect
{
    override public void TriggerEffect(){
        GameObject parent=null;
        GameObject target1=null;//Objetivos a los que afectan las cartas de clima
        GameObject target2=null;
        if(this.transform.parent==GameObject.Find("ClimaZoneM").transform){//Si la zona en la que esta la carta es x, afecta a tales zonas
            parent=GameObject.Find("ClimaZoneM");
            target1=GameObject.Find("MyMeleeDropZone");
            target2=GameObject.Find("EnemyMeleeDropZone");
        }else if(this.transform.parent==GameObject.Find("ClimaZoneR").transform){
            parent=GameObject.Find("ClimaZoneR");
            target1=GameObject.Find("MyRangedDropZone");
            target2=GameObject.Find("EnemyRangedDropZone");
        }else if(this.transform.parent==GameObject.Find("ClimaZoneS").transform){
            parent=GameObject.Find("ClimaZoneS");
            target1=GameObject.Find("MySiegeDropZone");
            target2=GameObject.Find("EnemySiegeDropZone");
        }
        if((target1!=null && target2!=null) && parent!=null){
            for(int j=0;j<this.transform.parent.childCount-1;j++){//Deshaciendo el efecto de clima
                for(int i=0;i<target1.transform.childCount;i++){
                    if(target1.transform.GetChild(i).GetComponent<Card>().wQuality!=Card.quality.Gold)
                        target1.transform.GetChild(i).GetComponent<Card>().addedPower++;
                        //Como la carta  de clima solo puede afectar una sola vez por partida no reiniciaremos el array affected a false
                }
                for(int i=0;i<target2.transform.childCount;i++){
                    if(target2.transform.GetChild(i).GetComponent<Card>().wQuality!=Card.quality.Gold)
                        target2.transform.GetChild(i).GetComponent<Card>().addedPower++;
                }
            }
        }
        Card[] cardsInSlot=parent.GetComponentsInChildren<Card>();//Lista de los componenetes Card en el slot que sea
        for(int i=this.transform.parent.childCount-1;i>=0;i--){//Se recorre esa lista de atras hacia adelante para que la ultima en enviarse al cementerio sea el despeje
            Graveyard.ToGraveyard(cardsInSlot[i].gameObject);//Mandando las cartas del slot para el cementerio
        }
    }
}
