using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOneCardEffect : CardEffect
{
    override public void TriggerEffect(){//Roba una carta del deck propio
        GameObject PlayerArea=null;//Mano del jugador
        GameObject PlayerDeck=null;//Deck del jugador
        if(this.GetComponent<Card>().whichField==Card.fields.P1){//Si la Scarlett Overkill jugada es de P1
            PlayerArea=GameObject.Find("Hand");
            PlayerDeck=GameObject.Find("Deck");
        }else{//Si la Scarlett Overkill jugada es de P2
            PlayerArea=GameObject.Find("EnemyHand");
            PlayerDeck=GameObject.Find("EnemyDeck");
        }
        DrawCards.DrawCard(PlayerArea,PlayerDeck);
    }
}
