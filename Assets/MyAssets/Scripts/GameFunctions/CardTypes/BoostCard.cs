using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
//Script para las cartas de aumento
public class BoostCard : DraggableCard, ISpecialCard
{
    public string GetEffectDescription => "Aumenta el poder de las cartas de la fila seleccionada";
    public override Color GetCardViewColor => new Color(0.4f, 1, 0.3f);
    public int Boost;//Cantidad de poder aumentado cuando una carta es afectada por el aumento
    public override void LoadInfo()
    {
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text = "[A]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text = "+" + Boost;
        GameObject.Find("BGPower").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text = "";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
    public override bool IsPlayable { get => transform.parent.gameObject.GetComponent<DZBoost>() != null; }
    public void TriggerSpecialEffect()
    {
        //Zona de las cartas a las que anadirle poder
        DZUnit targetZone = transform.parent.GetComponent<DZBoost>().Target;
        //Las cartas de esa zona que sean afectables se les suma el aumento
        targetZone.transform.TransformToIEnumerable<PowerCard>().Where(card => card.GetComponent<IAffectable>() != null).ForEach(card => card.AddedPower += Boost);
        //Envia la carta de aumento al cementerio
        Graveyard.SendToGraveyard(gameObject.GetComponent<DraggableCard>());
    }
}
