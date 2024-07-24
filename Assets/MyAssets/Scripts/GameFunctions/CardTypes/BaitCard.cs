using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
//Script para las cartas senuelo
public class BaitCard : PowerCard, ISpecialCard, IAffectable
{
    public string GetEffectDescription => "Esta carta se puede intercambiar con otra en el campo propio (solo con unidades de plata)";
    private List<WeatherCard> weathersAffecting = new List<WeatherCard>();//Lista de cartas clima que afectan el senuelo
    public List<WeatherCard> WeathersAffecting => weathersAffecting;
    public override Color CardViewColor => new Color(0.8f, 0.5f, 0.7f);
    public override void LoadInfo()
    {
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text = "[S]";
    }
    public override bool IsPlayable => EnteredCard ? SwapConditions(EnteredCard, true) : false;//Si estamos encima de otra carta devolvemos si cumple con las condiciones
    private bool SwapConditions(Card cardToSwap, bool showMessage = false)
    {//Si la carta a intercambiar con el senuelo es arrastrable, no esta en la mano ni en el cementerio, coincide con el campo del senuelo, no es otro senuelo y es afectable
        if (cardToSwap.GetComponent<DraggableCard>() == null) { if (showMessage) { UserRead.Write("No es valido usar un senuelo en una carta que no es arrastrable"); } return false; }
        if (cardToSwap.GetComponent<DraggableCard>().IsOnHand) { if (showMessage) { UserRead.Write("No es valido usar un senuelo sobre la mano"); } return false; }
        if (cardToSwap.transform.parent.GetComponent<Graveyard>() != null) { if (showMessage) { UserRead.Write("No es valido usar un senuelo en un cementerio"); } return false; }
        if (cardToSwap.WhichPlayer != WhichPlayer) { if (showMessage) { UserRead.Write("Esa carta no esta en tu campo"); } return false; }
        if (cardToSwap.GetComponent<BaitCard>() != null) { if (showMessage) { UserRead.Write("No es valido usar un senuelo sobre otro senuelo"); } return false; }
        if (cardToSwap.GetComponent<IAffectable>() == null) { if (showMessage) { UserRead.Write("Esa carta no es afectable por el senuelo"); } return false; }
        return true;
    }
    public override void ShowZone()
    {//Oscurece las cartas con las que el senuelo no se puede intercambiar e intenta iluminar el deck en caso de que se puedan intercambiar cartas con el
        Field.AllPlayedCards.ForEach(card => { if (!SwapConditions(card)) { card.GetComponent<Card>().OffGlow(); } });
        FindObjectsOfType<DeckTrade>().SingleOrDefault(zone => zone.IsDropValid(gameObject.GetComponent<DraggableCard>()))?.OnGlow();
    }
    public void TriggerSpecialEffect()
    {//Efecto de intercambio del senuelo
        TradeWith(EnteredCard);
    }
    private void TradeWith(Card card)
    {
        transform.SetParent(card.transform.parent);//El senuelo se pone donde esta la carta seleccionada
        transform.SetSiblingIndex(card.transform.GetSiblingIndex());

        card.transform.SetParent(GetHand.transform);//La carta seleccionada se pone en la mano
        card.transform.SetSiblingIndex(positionInHand);
    }
}
