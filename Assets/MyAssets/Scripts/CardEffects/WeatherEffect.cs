using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para las cartas de clima
public class WeatherEffect : CardEffect
{
    override public void TriggerEffect(){
        GameObject target1=this.transform.parent.GetComponent<DZWeather>().target1;//Objetivos a los que quitarle 1 de poder a los hijos
        GameObject target2=this.transform.parent.GetComponent<DZWeather>().target2;
        //GetWeatherTargets(this.gameObject,out target1,out target2);
        //Afecta a las zonas
        AffectZoneWithWeather(target1);//La de P1
        AffectZoneWithWeather(target2);//La de P2
    }
    private void AffectZoneWithWeather(GameObject zoneTarget){//Afecta la zona determinada con el efecto clima
        for(int i=0;i<zoneTarget.transform.childCount;i++){//Itera por todos los hijos
            if(zoneTarget.transform.GetChild(i).GetComponent<UnitCard>().affected[this.GetComponent<WeatherCard>().id]==false){//Si no han sido afectados
                if(zoneTarget.transform.GetChild(i).GetComponent<UnitCard>().wichQuality!=UnitCard.quality.Gold){//Ademas si no son heroes
                    zoneTarget.transform.GetChild(i).GetComponent<UnitCard>().addedPower--;
                    zoneTarget.transform.GetChild(i).GetComponent<UnitCard>().affected[this.GetComponent<WeatherCard>().id]=true;
                }
            }
        }
    }
    public static void UpdateWeather(){//Esta funcion se llama cada vez que se juega una nueva carta
        RectivateWeathersInZone("ClimaZoneM");//Actualiza el clima por zonas
        RectivateWeathersInZone("ClimaZoneR");
        RectivateWeathersInZone("ClimaZoneS");
    }
    private static void RectivateWeathersInZone(string zoneToUpdate){//Reactiva los efectos de las cartas clima en una zona especifica
        WeatherEffect[] cardsInZone=GameObject.Find(zoneToUpdate).GetComponentsInChildren<WeatherEffect>();//Se acceden a todos los hijos de esa zona
        for(int i=0;i<cardsInZone.Length;i++){//Itera por cada uno de esos hijos
            cardsInZone[i].TriggerEffect();//Hace que activen el efecto de clima otra vez
        }
    }
}
