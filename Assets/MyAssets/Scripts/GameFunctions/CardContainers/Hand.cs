using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Script para las manos de los jugadores
public class Hand : MonoBehaviour, IStateSubscriber, IContainer
{
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (State.EndingGame, new Execution(stateInfo => GetCards.Disappear(), 0)),
        new (new Execution (stateInfo => { UpdateHandActions();}, 1))
    };
    public IEnumerable<DraggableCard> GetCards => gameObject.CardsInside<DraggableCard>();
    public static IEnumerable<DraggableCard> PlayerCards => GameObject.Find("Hand" + Judge.GetPlayer).CardsInside<DraggableCard>();//Lista de las cartas de la mano del jugador en turno
    public static IEnumerable<DraggableCard> EnemyCards => GameObject.Find("Hand" + Judge.GetEnemy).CardsInside<DraggableCard>();//Lista de las cartas de la mano del enemigo del jugador en turno
    private void UpdateHandActions()
    {
        GetCards.ForEach(card => card.Owner = gameObject.Field());//Para que aquellos efectos que roban cartas no deban hacerlo
        ClearWeatherCard.ClearZoneOfWeathers(gameObject);//Limpia esta zona de efectos de clima
        UpdateHandLimit();//Chequea que no hayan mas de 10 cartas en la mano
    }
    private void UpdateHandLimit()
    {
        while (GetCards.Count() > 10)
        {//Si una carta no cabe en la mano
            DraggableCard extraCard = transform.GetChild(transform.childCount - 1).GetComponent<DraggableCard>();
            Graveyard.SendToGraveyard(extraCard);//Se envia al cementerio
            UserRead.Write("No puedes tener mas de 10 cartas en la mano." + extraCard.CardName + " se ha enviado al cementerio!");
        }
    }
}
