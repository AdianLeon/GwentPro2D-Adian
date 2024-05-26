using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para las cartas de clima
public class WeatherEffect : CardEffect
{
    override public void TriggerEffect(){
        GameObject target1=this.transform.parent.GetComponent<DZWeather>().target1;//Objetivos a los danarle los hijos
        GameObject target2=this.transform.parent.GetComponent<DZWeather>().target2;
        //Afecta a las zonas
        AffectZoneWithWeather(target1);//La de P1
        AffectZoneWithWeather(target2);//La de P2
    }
    private void AffectZoneWithWeather(GameObject zoneTarget){//Afecta la zona determinada con el efecto clima
        GameObject card;
        for(int i=0;i<zoneTarget.transform.childCount;i++){//Itera por todos los hijos
            card=zoneTarget.transform.GetChild(i).gameObject;
            //Si la carta en esa zona no es senuelo, o es unidad pero no de oro
            if(card.GetComponent<BaitCard>()!=null || (card.GetComponent<UnitCard>()!=null && card.GetComponent<UnitCard>().whichQuality!=UnitCard.quality.Gold)){
                if(!(card.GetComponent<CardWithPower>().affectedBy.Contains(this.gameObject.name))){//Si no han sido afectados
                    //Si la carta es senuelo o si la carta no es heroe
                    card.GetComponent<CardWithPower>().addedPower-=this.GetComponent<WeatherCard>().damage;
                    card.GetComponent<CardWithPower>().affectedBy.Add(this.gameObject.name);
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
