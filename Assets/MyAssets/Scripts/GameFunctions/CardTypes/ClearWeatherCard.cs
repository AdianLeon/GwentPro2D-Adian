using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas de despeje
public class ClearWeatherCard : WeatherZoneCard
{
    public override Color GetCardViewColor(){return new Color(0.5f,1,1);}
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[D]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);
    }
    public override void TriggerSpecialEffect(){//Efecto de las cartas despeje
        ClearZoneOfWeathers(this.transform.parent.GetComponent<DZWeather>().Target1);//Deshace el efecto clima en el campo correspondiente a la zona de P1
        ClearZoneOfWeathers(this.transform.parent.GetComponent<DZWeather>().Target2);//Deshace el efecto clima en el campo correspondiente a la zona de P2
        int count=this.transform.parent.childCount;//Cantidad de hijos que tiene la zona clima
        Graveyard.SendToGraveyard(GFUtils.GetCardsIn(this.transform.parent.gameObject));//Mandando las cartas de la zona para el cementerio (incluido el despeje)
    }
    public static void ClearZoneOfWeathers(GameObject zoneTarget){//Deshace el efecto de clima en la zona pasada como parametro
        foreach(Transform card in zoneTarget.transform){
            ClearCardOfWeathers(card.gameObject);
        }
    }
    private static void ClearCardOfWeathers(GameObject affectedCard){//Deshace completamente el efecto de clima de la carta pasada como parametro
        if(affectedCard.GetComponent<IAffectable>()!=null){
            foreach(WeatherCard weatherCard in affectedCard.GetComponent<IAffectable>().AffectedByWeathers){//Cada carta clima que la afecta deshace su efecto
                if(affectedCard.GetComponent<CardWithPower>()!=null){
                    affectedCard.GetComponent<CardWithPower>().AddedPower+=weatherCard.Damage;
                }else{
                    Debug.Log("La carta afectada por efecto clima: "+affectedCard.GetComponent<Card>().CardName+" no tiene poder!");
                    throw new System.Exception();
                }
            }
            affectedCard.GetComponent<IAffectable>().AffectedByWeathers.Clear();//Esta carta ya no es afectada por ninguna carta clima
        }
    }
}
