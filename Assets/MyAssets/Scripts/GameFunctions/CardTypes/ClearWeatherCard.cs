using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas de despeje
public class ClearWeatherCard : Card, IShowZone
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
        for(int i=0;i<zones.Length;i++){
            zones[i].GetComponent<Image>().color=new Color (1,1,1,0.1f);//Se iluminan
        }
    }
}
