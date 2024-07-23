using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas de despeje
public class ClearWeatherCard : WeatherZoneCard
{
    public override string GetEffectDescription => "Se deshace de todas las cartas de clima y sus efectos en la fila seleccionada";
    public override Color GetCardViewColor => new Color(0.5f, 1, 1);
    public override void LoadInfo()
    {
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text = "[D]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text = "";
        GameObject.Find("BGPower").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0);
    }
    public override void TriggerSpecialEffect()
    {//Efecto de las cartas despeje
        ClearZoneOfWeathers(transform.parent.GetComponent<DZWeather>().Target1.gameObject);//Deshace el efecto clima en el campo correspondiente a la zona de P1
        ClearZoneOfWeathers(transform.parent.GetComponent<DZWeather>().Target2.gameObject);//Deshace el efecto clima en el campo correspondiente a la zona de P2
        Graveyard.SendToGraveyard(transform.parent.gameObject.CardsInside<DraggableCard>());//Mandando las cartas de la zona para el cementerio (incluido el despeje)
    }
    public static void ClearZoneOfWeathers(GameObject zoneTarget)
    {//Deshace el efecto de clima en la zona pasada como parametro
        zoneTarget.transform.TransformToIEnumerable<PowerCard>().ForEach(card => ClearCardOfWeathers(card));
    }
    private static void ClearCardOfWeathers(PowerCard affectedCard)
    {//Deshace completamente el efecto de clima de la carta pasada como parametro
        if (affectedCard.GetComponent<IAffectable>() != null)
        {
            affectedCard.GetComponent<IAffectable>().WeathersAffecting.ForEach(weatherCard => affectedCard.GetComponent<PowerCard>().AddedPower += weatherCard.Damage);
            affectedCard.GetComponent<IAffectable>().WeathersAffecting.Clear();//Esta carta ya no es afectada por ninguna carta clima
        }
    }
}
