using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
//Script para la funcionalidad de los cementerios
public class Graveyard : MonoBehaviour, IStateSubscriber, IContainer
{
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (new Execution(stateInfo => UpdateGraveyardActions(), 2)),
        new (State.EndingGame, new Execution(stateInfo => GetCards.Disappear(), 2))
    };
    public IEnumerable<DraggableCard> GetCards => gameObject.CardsInside<DraggableCard>();
    public static IEnumerable<DraggableCard> PlayerCards => GameObject.Find("Graveyard" + Judge.GetPlayer).CardsInside<DraggableCard>();//Lista de las cartas en el cementerio del jugador en turno
    public static IEnumerable<DraggableCard> EnemyCards => GameObject.Find("Graveyard" + Judge.GetEnemy).CardsInside<DraggableCard>();//Lista de las cartas en el cementerio del enemigo del jugador en turno
    private int deadCount { get => transform.childCount; }//Contador de carta en este cementerio
    private void UpdateGraveyardActions()
    {
        ClearWeatherCard.ClearZoneOfWeathers(gameObject);//Limpia esta zona de efectos de clima
        UpdateDeadCount();//Actualiza el conteo de cartas
    }
    private void UpdateDeadCount() => GameObject.Find("GText" + gameObject.Field()).GetComponent<TextMeshProUGUI>().text = deadCount.ToString();//Cuenta y actualiza cuantas cartas hay en el cementerio
    public static void SendToGraveyard(IEnumerable<DraggableCard> cards) => cards.ForEach(card => SendToGraveyard(card));//Envia a todas las cartas de la lista al cementerio
    public static void SendToGraveyard(DraggableCard card) => GameObject.Find("Graveyard" + card.Owner).GetComponent<Graveyard>().ToGraveyard(card);//Envia la carta al cementerio correspondiente
    private void ToGraveyard(DraggableCard card)
    {//Se limpia la lista de cartas de clima si es afectable, se resetea el poder anadido si es de poder y se mueve para el cementerio
        card.GetComponent<IAffectable>()?.WeathersAffecting.Clear();
        if (card.GetComponent<PowerCard>() != null) { card.GetComponent<PowerCard>().AddedPower = 0; }
        card.GetComponent<DraggableCard>().MoveCardTo(gameObject);
    }
}
