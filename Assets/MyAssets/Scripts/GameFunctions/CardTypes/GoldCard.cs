using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para las cartas heroe (cartas de unidad de oro)
public class GoldCard : UnitCard
{
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Quality").GetComponent<Image>().sprite=Resources.Load<Sprite>("GoldQuality");
    }
}
