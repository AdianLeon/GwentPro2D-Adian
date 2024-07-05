using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas de despeje
public class ClearWeatherCard : Card, IShowZone, ISpecialCard
{
    public override Color GetCardViewColor(){return new Color(0.5f,1,1);}
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[D]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
    }
    public void ShowZone(){
        DZWeather[] zones=GameObject.FindObjectsOfType<DZWeather>();//Se crea un array de todas las zonas de clima
        foreach(DZWeather zone in zones){
            zone.OnGlow();
        }
    }
    public void TriggerSpecialEffect(){//Efecto de las cartas despeje
        Debug.Log(this.transform.parent.gameObject.name);
        Debug.Log(this.transform.parent.GetComponent<DZWeather>());
        Debug.Log(this.transform.parent.GetComponent<DZWeather>().Target1);
        GameObject target1=this.transform.parent.GetComponent<DZWeather>().Target1;//Objetivos a los que afectan las cartas de clima
        GameObject target2=this.transform.parent.GetComponent<DZWeather>().Target2;

        for(int i=0;i<this.transform.parent.childCount-1;i++){//Deshaciendo el efecto de clima carta por carta
            ClearZoneOfWeathers(target1);//Deshace el efecto de esa carta en el campo correspondiente de P1
            ClearZoneOfWeathers(target2);//Deshace el efecto de esa carta en el campo correspondiente de P2
        }
        int count=this.transform.parent.childCount;//Cantidad de hijos que tiene la zona clima
        for(int i=0;i<count;i++){
            Graveyard.SendToGraveyard(this.transform.parent.GetChild(0).gameObject);//Mandando las cartas de la zona para el cementerio
        }
    }
    private static void ClearZoneOfWeathers(GameObject zoneTarget){//Deshace el efecto de clima en la zona
        for(int i=0;i<zoneTarget.transform.childCount;i++){
            ClearCardOfWeathers(zoneTarget.transform.GetChild(i).gameObject);
        }
    }
    public static void ClearCardOfWeathers(GameObject affectedCard){//Deshace completamente el efecto de clima de la carta pasada como parametro
        if(affectedCard.GetComponent<IAffectable>()!=null){
            int count=affectedCard.GetComponent<IAffectable>().AffectedByWeathers.Count;
            for(int i=0;i<count;i++){
                if(affectedCard.GetComponent<CardWithPower>()!=null){
                    affectedCard.GetComponent<CardWithPower>().AddedPower+=GameObject.Find(affectedCard.GetComponent<IAffectable>().AffectedByWeathers[0]).GetComponent<WeatherCard>().damage;
                }
                affectedCard.GetComponent<IAffectable>().AffectedByWeathers.RemoveAt(0);
            }
        }
    }
}
