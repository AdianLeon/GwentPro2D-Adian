using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas que seran jugadas en la zona de climas
public abstract class WeatherZoneCard : Card, IShowZone, ISpecialCard
{
    public override bool IsPlayable{get=>this.transform.parent.gameObject.GetComponent<DZWeather>()!=null;}//Son jugables si se encuentran en una zona de clima
    public override void LoadInfo(){//No tienen AddedPower
        base.LoadInfo();

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
    }
    public void ShowZone(){//Para mostrar sus zonas solo deben iluminar las zonas clima
        DZWeather[] zones=GameObject.FindObjectsOfType<DZWeather>();//Se crea un array de todas las zonas de clima
        foreach(DZWeather zone in zones){
            zone.OnGlow();
        }
    }
    public abstract void TriggerSpecialEffect();
}
