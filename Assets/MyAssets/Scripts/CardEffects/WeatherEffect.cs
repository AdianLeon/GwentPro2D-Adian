using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffect : CardEffect
{
    override public void TriggerEffect(){
        GameObject target1=null;//Objetivos a los que quitarle 1 de poder a los hijos
        GameObject target2=null;
        if(this.transform.parent==GameObject.Find("ClimaZoneM").transform){//Si la zona en la que esta la carta es x, afecta a tales zonas
            target1=GameObject.Find("MyMeleeDropZone");
            target2=GameObject.Find("EnemyMeleeDropZone");
        }else if(this.transform.parent==GameObject.Find("ClimaZoneR").transform){
            target1=GameObject.Find("MyRangedDropZone");
            target2=GameObject.Find("EnemyRangedDropZone");
        }else if(this.transform.parent==GameObject.Find("ClimaZoneS").transform){
            target1=GameObject.Find("MySiegeDropZone");
            target2=GameObject.Find("EnemySiegeDropZone");
        }
        for(int i=0;i<target1.transform.childCount;i++){//Disminuye en 1 el poder de la fila seleccionada y lo marca
            if(target1.transform.GetChild(i).GetComponent<UnitCard>().affected[this.GetComponent<WeatherCard>().id]==false && target1.transform.GetChild(i).GetComponent<UnitCard>().wichQuality!=UnitCard.quality.Gold){
                target1.transform.GetChild(i).GetComponent<UnitCard>().addedPower--;
                target1.transform.GetChild(i).GetComponent<UnitCard>().affected[this.GetComponent<WeatherCard>().id]=true;
            }
        }
        for(int i=0;i<target2.transform.childCount;i++){
            if(target2.transform.GetChild(i).GetComponent<UnitCard>().affected[this.GetComponent<WeatherCard>().id]==false && target2.transform.GetChild(i).GetComponent<UnitCard>().wichQuality!=UnitCard.quality.Gold){
                target2.transform.GetChild(i).GetComponent<UnitCard>().addedPower--;
                target2.transform.GetChild(i).GetComponent<UnitCard>().affected[this.GetComponent<WeatherCard>().id]=true;
            }
        }
    }
    public static void UpdateWeather(){//Esta funcion se llama cada vez que se juega una nueva carta
        UpdateWeatherByZones("ClimaZoneM");//Actualiza el clima por zonas
        UpdateWeatherByZones("ClimaZoneR");
        UpdateWeatherByZones("ClimaZoneS");
    }
    public static void UpdateWeatherByZones(string zoneToUpdate){//Reactiva los efectos de las cartas clima en una zona especifica
        CardEffect[] cardsInZone=GameObject.Find(zoneToUpdate).GetComponentsInChildren<CardEffect>();//Se acceden a todos los hijos de esa zona
        for(int i=0;i<cardsInZone.Length;i++){//Itera por cada uno de esos hijos
            cardsInZone[i].TriggerEffect();//Hace que activen el efecto de clima otra vez
        }
    }
}
