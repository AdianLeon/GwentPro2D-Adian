using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de robar una carta del deck
public class DrawOneEffect : CardEffect
{
    override public void TriggerEffect(){//Roba una carta del deck propio
        GameObject PlayerArea=null;//Mano del jugador
        GameObject PlayerDeck=null;//Deck del jugador
        if(this.GetComponent<Card>().whichField==Card.fields.P1){//Si la carta jugada es de P1
            PlayerArea=GameObject.Find("Hand");
            PlayerDeck=GameObject.Find("Deck");
        }else{//Si la carta jugada es de P2
            PlayerArea=GameObject.Find("EnemyHand");
            PlayerDeck=GameObject.Find("EnemyDeck");
        }
        if(PlayerDeck.GetComponent<DrawCards>().cards.Count!=0){//Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
            GameObject picked=PlayerDeck.GetComponent<DrawCards>().cards[Random.Range(0,PlayerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
            GameObject newCard = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            newCard.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            PlayerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista
            RoundPoints.URLongWrite("Se ha robado una carta del deck. Es "+newCard.GetComponent<Card>().cardRealName);
        }else{
            RoundPoints.URLongWrite("No se pudo activar el efecto porque no quedan cartas en el deck");
        }
    }
}
