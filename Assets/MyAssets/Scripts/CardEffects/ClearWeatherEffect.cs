using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para las cartas de despeje
public class ClearWeatherEffect : CardEffect
{
    override public void TriggerEffect(){
        GameObject target1=this.transform.parent.GetComponent<DZWeather>().target1;//Objetivos a los que afectan las cartas de clima
        GameObject target2=this.transform.parent.GetComponent<DZWeather>().target2;

        for(int i=0;i<this.transform.parent.childCount-1;i++){//Deshaciendo el efecto de clima carta por carta
            UnaffectZoneOfWeathers(target1);//Deshace el efecto de esa carta en el campo correspondiente de P1
            UnaffectZoneOfWeathers(target2);//Deshace el efecto de esa carta en el campo correspondiente de P2
        }
        //Card[] cardsInSlot=this.transform.parent.GetComponentsInChildren<Card>();//Lista de los componenetes Card en el slot que sea
        int count=this.transform.parent.childCount;//Cantidad de hijos que tiene la zona clima
        for(int i=0;i<count;i++){
            Graveyard.ToGraveyard(this.transform.parent.GetChild(0).gameObject);//Mandando las cartas de la zona para el cementerio
        }
    }
    private void UnaffectZoneOfWeathers(GameObject zoneTarget){//Deshace el efecto de clima en la zona
        for(int i=0;i<zoneTarget.transform.childCount;i++){
            if(zoneTarget.transform.GetChild(i).GetComponent<UnitCard>().wichQuality!=UnitCard.quality.Gold){
                zoneTarget.transform.GetChild(i).GetComponent<UnitCard>().addedPower++;
                //Como la carta  de clima solo puede afectar una sola vez por partida no reiniciaremos el array affected a false
            }
        }
    }
}
