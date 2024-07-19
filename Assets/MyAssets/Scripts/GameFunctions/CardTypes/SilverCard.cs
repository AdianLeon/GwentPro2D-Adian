using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para las cartas de plata
public class SilverCard : UnitCard, IAffectable
{
    private List<WeatherCard> weathersAffecting = new List<WeatherCard>();
    public List<WeatherCard> WeathersAffecting { get => weathersAffecting; }
    public override Color GetCardViewColor => new Color(0.8f, 0.8f, 0.8f);
    public override void LoadInfo()
    {
        base.LoadInfo();
        GameObject.Find("Quality").GetComponent<Image>().sprite = Resources.Load<Sprite>("SilverQuality");
    }
}
