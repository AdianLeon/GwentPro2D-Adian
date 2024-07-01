using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas de unidades (oro y plata)
abstract public class UnitCard : CardWithPower
{
    public enum zonesUC{M,R,S,MR,MS,RS,MRS};//Zona(s) donde la carta se puede jugar
    public zonesUC whichZone;

    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="["+whichZone.ToString()+"]";
    }
}
