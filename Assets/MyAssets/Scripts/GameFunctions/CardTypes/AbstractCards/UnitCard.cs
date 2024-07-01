using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas de unidades (oro y plata)
abstract public class UnitCard : CardWithPower, IShowZone
{
    public enum zonesUC{M,R,S,MR,MS,RS,MRS};//Zona(s) donde la carta se puede jugar
    public zonesUC whichZone;

    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="["+whichZone.ToString()+"]";
    }
    public void ShowZone(){
        DZUnits[] unitZones=GameObject.FindObjectsOfType<DZUnits>();//Se crea un array con todas las zonas de cartas de unidad
        foreach(DZUnits unitZone in unitZones){
            if(unitZone.GetComponent<DZUnits>().isDropValid(this.gameObject) && unitZone.validPlayer==this.GetComponent<Card>().WhichField){
                //La zona se ilumina solo si coincide con la zona jugable y el campo de la carta
                unitZone.GetComponent<Image>().color=new Color (1,1,1,0.1f);
            }
        }
    }
}
