using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para las cartas de plata
public class SilverCard : UnitCard, IAffectable
{
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Quality").GetComponent<Image>().sprite=Resources.Load<Sprite>("SilverQuality");
    }
}
