using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas clima
public class WeatherCard : WeatherZoneCard
{
    public override string GetEffectDescription=>"Reduce el poder de las cartas de la fila seleccionada (Para ambos campos)";
    public int Damage;//Cantidad de poder restado cuando una carta es afectada por el clima
    public override Color GetCardViewColor=>new Color(0.7f,0.2f,0.2f);
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[C]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="-"+Damage;
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,1);
    }
    public override void TriggerSpecialEffect(){//Efecto de las cartas clima
        //Afecta a las zonas
        AffectZoneWithWeather(this.transform.parent.GetComponent<DZWeather>().Target1);//La de P1
        AffectZoneWithWeather(this.transform.parent.GetComponent<DZWeather>().Target2);//La de P2
    }
    private void AffectZoneWithWeather(GameObject zoneTarget){//Afecta la zona determinada con el efecto clima
        foreach(Transform card in zoneTarget.transform){//Itera por todos los hijos
            if(card.GetComponent<IAffectable>()==null){continue;}//Si la carta no es afectable no la consideramos
            if(card.GetComponent<IAffectable>().AffectedByWeathers.Contains(this)){continue;}//Si la carta ya ha sido afectada por este clima no la consideramos
            //La carta es afectable y todavia no ha sido afectada por este clima
            card.GetComponent<IAffectable>().AffectedByWeathers.Add(this);
            if(card.GetComponent<PowerCard>()!=null){//Si contiene componente de carta con poder
                card.GetComponent<PowerCard>().AddedPower-=Damage;
            }
        }
    }
    public static void RectivateAllWeathers(){//Reactiva los efectos de todas las cartas clima jugadas
        List<GameObject> playedWeathers=Field.PlayedWeatherCards;
        foreach(GameObject weather in playedWeathers){//Itera por cada uno de esos hijos
            weather.GetComponent<WeatherCard>().TriggerSpecialEffect();//Hace que activen el efecto de clima otra vez
        }
    }
}
