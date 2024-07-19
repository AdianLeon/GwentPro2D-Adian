using UnityEngine;
using TMPro;
//Script para los comportamientos en comun de las cartas de unidades (oro y plata)
public abstract class UnitCard : PowerCard
{
    public UnitCardZone WhichZone;//Zona(s) donde se puede jugar la carta
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="["+WhichZone.ToString()+"]";
    }
    public override bool IsPlayable{get=>this.transform.parent.gameObject.GetComponent<DZUnit>()!=null;}//Es jugable si se encuentra en una zona de cartas de unidad
}
