using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de robar una carta del deck
public class DrawOneEffect : MonoBehaviour, ICardEffect
{
    public void TriggerEffect(){//Roba una carta del deck propio
        GameObject PlayerArea=GameObject.Find("Hand"+TurnManager.PTurn);//Mano del jugador
        GameObject PlayerDeck=GameObject.Find("Deck"+TurnManager.PTurn);//Deck del jugador
        
        if(PlayerDeck.GetComponent<DrawCards>().cardsInDeck.Count!=0){//Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
            GameObject picked=PlayerDeck.GetComponent<DrawCards>().cardsInDeck[Random.Range(0,PlayerDeck.GetComponent<DrawCards>().cardsInDeck.Count)];//La escogida es aleatoria
            GameObject newCard = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            newCard.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            PlayerDeck.GetComponent<DrawCards>().cardsInDeck.Remove(picked);//Se quita de la lista
            RoundPoints.URLongWrite("Se ha robado una carta del deck. Es "+newCard.GetComponent<Card>().cardRealName);
        }else{
            RoundPoints.URLongWrite("No se pudo activar el efecto porque no quedan cartas en el deck");
        }
    }
}
