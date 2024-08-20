using UnityEngine;
using TMPro;
//Script para los comportamientos en comun de las cartas de unidades (oro y plata)
public abstract class UnitCard : PowerCard
{
    public UnitCardZone Range;//Zona(s) donde se puede jugar la carta
    public override void LoadInfo()
    {
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text = "[" + Range + "]";
    }
}
