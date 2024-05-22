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
            ClearZoneOfWeathers(target1);//Deshace el efecto de esa carta en el campo correspondiente de P1
            ClearZoneOfWeathers(target2);//Deshace el efecto de esa carta en el campo correspondiente de P2
        }
        int count=this.transform.parent.childCount;//Cantidad de hijos que tiene la zona clima
        for(int i=0;i<count;i++){
            Graveyard.ToGraveyard(this.transform.parent.GetChild(0).gameObject);//Mandando las cartas de la zona para el cementerio
        }
    }
    private static void ClearZoneOfWeathers(GameObject zoneTarget){//Deshace el efecto de clima en la zona
        for(int i=0;i<zoneTarget.transform.childCount;i++){
            ClearCardOfWeathers(zoneTarget.transform.GetChild(i).gameObject);
        }
    }
    public static void ClearCardOfWeathers(GameObject affectedCard){//Deshace completamente el efecto de clima de la carta pasada como parametro
        CardWithPower c=affectedCard.GetComponent<CardWithPower>();
        for(int i=0;i<c.affected.Length;i++){
            if(c.affected[i]){
                c.addedPower+=WeatherCard.GetWeatherWithID(i).GetComponent<WeatherCard>().damage;
                c.affected[i]=false;
            }
        }
    }
}
