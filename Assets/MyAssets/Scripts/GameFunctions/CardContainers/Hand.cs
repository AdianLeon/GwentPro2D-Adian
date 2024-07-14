using System.Collections.Generic;
using UnityEngine;
//Script para las manos de los jugadores
public class Hand : StateListener, IContainer
{
    public List <GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}
    public static List<GameObject> PlayerHandCards{get=>GameObject.Find("Hand"+Judge.GetPlayer).GetComponent<Hand>().GetCards;}//Lista de las cartas de la mano del jugador en turno
    public static List<GameObject> EnemyHandCards{get=>GameObject.Find("Hand"+Judge.GetEnemy).GetComponent<Hand>().GetCards;}//Lista de las cartas de la mano del enemigo del jugador en turno
    public override void CheckState(){ 
        switch(Judge.CurrentState){
            case State.PlayingCard://Chequea que no hayan mas de 10 cartas en la mano y limpia a esas cartas de los efectos de clima
            case State.EndingTurn:
            case State.EndingRound:
                UpdateHandActions();
                break;
            case State.EndingGame://Al final del juego se deshace de las cartas en la mano enviandolas a la basura
                GFUtils.GetRidOf(GetCards);
                break;
        }
    }
    private void UpdateHandActions(){
        ClearWeatherCard.ClearZoneOfWeathers(this.gameObject);
        UpdateHandLimit();
    }
    private void UpdateHandLimit(){
        while(this.transform.childCount>10){//Si una carta no cabe en la mano
            GameObject extraCard=this.transform.GetChild(this.transform.childCount-1).gameObject;
            Graveyard.SendToGraveyard(extraCard);//Se envia al cementerio
            GFUtils.UserRead.Write("No puedes tener mas de 10 cartas en la mano."+extraCard.GetComponent<Card>().CardName+" se ha enviado al cementerio!");
        }
    }
}
