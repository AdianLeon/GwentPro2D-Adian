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
        }else if(this.GetComponent<Card>().whichField==Card.fields.P2){//Si la Scarlett Overkill jugada es de P2
            PlayerArea=GameObject.Find("EnemyHand");
            PlayerDeck=GameObject.Find("EnemyDeck");
        }
        if(PlayerArea!=null && PlayerDeck!=null){
            GameObject picked=PlayerDeck.GetComponent<DrawCards>().cards[Random.Range(0,PlayerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            PlayerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista
        }
    }
}
