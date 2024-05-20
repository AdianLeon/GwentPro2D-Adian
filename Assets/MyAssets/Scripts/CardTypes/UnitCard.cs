using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas de unidades (oro y plata)
public class UnitCard : CardWithPower
{
    public enum quality{Silver,Gold}//Calidad de la carta, si es plata tendra hasta 3 copias, si es oro no sera afectada por ningun efecto durante el juego
    public quality wichQuality;
    public enum zonesUC{M,R,S,MR,MS,RS,MRS};
    public zonesUC whichZone;

    public override void LoadInfo(){
        base.LoadInfo();
        string typeText="[";
        typeText+=whichZone.ToString()+"]";
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text=typeText;
    }
}
