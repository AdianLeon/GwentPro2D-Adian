using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para las manos de los jugadores
public class Hand : StateListener, IContainer
{
    public List <GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}
    public static List<GameObject> PlayerHandCards{get=>GameObject.Find("Hand"+Judge.GetPlayer).GetComponent<Hand>().GetCards;}
    public static List<GameObject> EnemyHandCards{get=>GameObject.Find("Hand"+Judge.GetEnemy).GetComponent<Hand>().GetCards;}
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.PlayingCard:
            case State.EndingTurn:
            case State.EndingRound:
                UpdateHandLimit();
                break;
            case State.EndingGame:
                GFUtils.GetRidOf(GetCards);
                break;
        }
    }
    private void UpdateHandLimit(){
        while(this.transform.childCount>10){//Si una carta no cabe en la mano
            GameObject extraCard=this.transform.GetChild(this.transform.childCount-1).gameObject;
            Graveyard.SendToGraveyard(extraCard);//Se envia al cementerio
            GFUtils.UserRead.LongWrite("No puedes tener mas de 10 cartas en la mano."+extraCard.GetComponent<Card>().CardName+" se ha enviado al cementerio!");
        }
    }
}
