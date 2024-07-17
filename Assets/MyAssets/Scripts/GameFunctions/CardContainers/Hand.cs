using System.Collections.Generic;
using UnityEngine;
//Script para las manos de los jugadores
public class Hand : StateListener, IContainer
{
    public override int GetPriority=>1;
    public List <GameObject> GetCards=>gameObject.CardsInside();
    public static List<GameObject> PlayerHandCards=>GameObject.Find("Hand"+Judge.GetPlayer).GetComponent<Hand>().GetCards;//Lista de las cartas de la mano del jugador en turno
    public static List<GameObject> EnemyHandCards=>GameObject.Find("Hand"+Judge.GetEnemy).GetComponent<Hand>().GetCards;//Lista de las cartas de la mano del enemigo del jugador en turno
    public override void CheckState(){ 
        switch(Judge.CurrentState){
            case State.PlayingCard://Chequea que no hayan mas de 10 cartas en la mano y limpia a esas cartas de los efectos de clima
            case State.EndingTurn:
            case State.EndingRound:
                ClearWeatherCard.ClearZoneOfWeathers(this.gameObject);
                UpdateHandLimit();
                break;
            case State.EndingGame://Al final del juego se deshace de las cartas en la mano enviandolas a la basura
                GetCards.Disappear();
                break;
        }
    }
    private void UpdateHandLimit(){
        while(this.transform.childCount>10){//Si una carta no cabe en la mano
            GameObject extraCard=this.transform.GetChild(this.transform.childCount-1).gameObject;
            Graveyard.SendToGraveyard(extraCard);//Se envia al cementerio
            UserRead.Write("No puedes tener mas de 10 cartas en la mano."+extraCard.GetComponent<Card>().CardName+" se ha enviado al cementerio!");
        }
    }
}
