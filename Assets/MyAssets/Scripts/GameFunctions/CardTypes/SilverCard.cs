using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para las cartas de plata
public class SilverCard : UnitCard, IAffectable
{
    List<string> affectedByWeathers=new List<string>();
    public List<string> AffectedByWeathers{get=>affectedByWeathers; set=>affectedByWeathers=value;}
    public override Color GetCardViewColor(){return new Color(0.8f,0.8f,0.8f);}
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Quality").GetComponent<Image>().sprite=Resources.Load<Sprite>("SilverQuality");
    }
}
