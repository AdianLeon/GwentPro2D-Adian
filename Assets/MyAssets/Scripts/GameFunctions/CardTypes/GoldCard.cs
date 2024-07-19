using UnityEngine;
using UnityEngine.UI;
//Script para las cartas heroe (cartas de unidad de oro)
public class GoldCard : UnitCard
{
    public override Color GetCardViewColor => new Color(0.8f, 0.7f, 0.2f);
    public override void LoadInfo()
    {
        base.LoadInfo();
        GameObject.Find("Quality").GetComponent<Image>().sprite = Resources.Load<Sprite>("GoldQuality");
    }
}
