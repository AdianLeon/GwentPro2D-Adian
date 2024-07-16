using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para los comportamientos que comparten las cartas que seran jugadas en la zona de climas
public abstract class WeatherZoneCard : DraggableCard, IShowZone, ISpecialCard
{
    public abstract string GetEffectDescription{get;}
    public override bool IsPlayable=>this.transform.parent.gameObject.GetComponent<DZWeather>()!=null;//Son jugables si se encuentran en una zona de clima
    public override void LoadInfo(){//No tienen AddedPower
        base.LoadInfo();

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
    }
    public abstract void TriggerSpecialEffect();
}
