using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas de aumento
public class BoostCard : Card, IShowZone, ICardEffect
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
    public void TriggerEffect(){//Efecto de las cartas aumento
        GameObject target=this.transform.parent.GetComponent<DZBoost>().Target;//Objetivo padre de las cartas a las que anadirle poder
        for(int i=0;i<target.transform.childCount;i++){
            if(target.transform.GetChild(i).GetComponent<IAffectable>()!=null){//Si es afectable
                target.transform.GetChild(i).GetComponent<CardWithPower>().AddedPower+=this.GetComponent<BoostCard>().boost;//Aumenta el poder
            }
        }
        Graveyard.SendToGraveyard(this.gameObject);//Envia la carta usada al cementerio
    }
    public void ShowZone(){
        DZBoost[] boostZones=GameObject.FindObjectsOfType<DZBoost>();//Se crea un array de todas las zonas de aumento
        foreach(DZBoost boostZone in boostZones){
            if(boostZone.ValidPlayer==GetComponent<Card>().WhichField){//Si la zona es del jugador
                boostZone.OnGlow();//Se ilumina
            }
        }
    }
}
