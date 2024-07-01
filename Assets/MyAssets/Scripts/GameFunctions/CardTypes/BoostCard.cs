using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas de aumento
public class BoostCard : Card, IShowZone
{
    public override Color GetCardViewColor(){return new Color(0.4f,1,0.3f);}
    public int boost;//Cant de poder aumentado cuando una carta es afectada por el aumento
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[A]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="+"+boost;
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,1);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
    }
    public void ShowZone(){
        DZBoost[] boostZones=GameObject.FindObjectsOfType<DZBoost>();//Se crea un array de todas las zonas de aumento
        foreach(DZBoost boostZone in boostZones){
            if(boostZone.validPlayer==GetComponent<Card>().WhichField){//Si la zona es del jugador
                boostZone.GetComponent<Image>().color=new Color (1,1,1,0.1f);//Se ilumina
            }
        }
    }
}
