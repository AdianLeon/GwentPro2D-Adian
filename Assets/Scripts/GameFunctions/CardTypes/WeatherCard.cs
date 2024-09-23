using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
//Script para las cartas clima
public class WeatherCard : WeatherZoneCard
{
    public int Damage;//Cantidad de poder restado cuando una carta es afectada por el clima
    public override string GetEffectDescription => "Reduce el poder de las cartas de la fila seleccionada (Para ambos campos)";
    public override Color CardViewColor => new Color(0.7f, 0.2f, 0.2f);
    public override void LoadInfo()
    {
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text = "[C]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text = "-" + Damage;
        GameObject.Find("BGPower").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1);
    }
    public override void TriggerSpecialEffect()
    {//Efecto de las cartas clima
        DZWeather parentZone = transform.parent.GetComponent<DZWeather>();
        List<DraggableCard> cards = parentZone.TargetP1.gameObject.CardsInside<DraggableCard>().ToList();
        cards.AddRange(parentZone.TargetP2.gameObject.CardsInside<DraggableCard>());
        AffectWithWeather(cards);
    }
    private void AffectWithWeather(List<DraggableCard> targets)
    {//Afecta la zona determinada con el efecto clima
        foreach (DraggableCard card in targets)
        {//Itera por todos los hijos
            if (card.GetComponent<IAffectable>() == null) { continue; }//Si la carta no es afectable no la consideramos
            if (card.GetComponent<IAffectable>().WeathersAffecting.Contains(this)) { continue; }//Si la carta ya ha sido afectada por este clima no la consideramos
            //La carta es afectable y todavia no ha sido afectada por este clima
            card.GetComponent<IAffectable>().WeathersAffecting.Add(this);
            if (card.GetComponent<PowerCard>() != null)
            {//Si contiene componente de carta con poder
                card.GetComponent<PowerCard>().AddedPower -= Damage;
            }
        }
    }
}
