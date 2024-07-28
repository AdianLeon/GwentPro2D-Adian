using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para los comportamientos que comparten las cartas que seran jugadas en la zona de climas
public abstract class WeatherZoneCard : DraggableCard, ISpecialCard
{
    public abstract string GetEffectDescription { get; }
    public abstract void TriggerSpecialEffect();
    public override void LoadInfo()
    {//No tienen AddedPower
        base.LoadInfo();
        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text = "";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
}
