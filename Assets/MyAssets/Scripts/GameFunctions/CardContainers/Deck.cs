using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//Script para el funcionamiento del deck y la habilidad de lider
public class Deck : StateListener, IContainer
{
    public List<GameObject> GetCards{get=>DeckCards;}
    public static List<GameObject> PlayerDeckCards{get=>GameObject.Find("Deck"+Judge.GetPlayer).GetComponent<Deck>().GetCards;}
    public static List<GameObject> EnemyDeckCards{get=>GameObject.Find("Deck"+Judge.GetEnemy).GetComponent<Deck>().GetCards;}
    private GameObject container;//Este objeto contiene todas las cartas que van a ser anadidas a la lista
    private GameObject playerArea;//Esta es la mano del jugador dueno de este deck
    private List <GameObject> DeckCards = new List <GameObject>();//Lista de cartas
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame:
                ReceiveAndDealCards();
                break;
            case State.EndingGame:
                DeckCards.Clear();
                break;
        }
    }
    private void ReceiveAndDealCards(){
        container=GameObject.Find("Cards"+GFUtils.GetField(this.name));
        playerArea=GameObject.Find("Hand"+GFUtils.GetField(this.name));
        
        for(int i=0;i<container.transform.childCount;i++){//Anadiendo las cartas del contenedor del jugador al deck y asignandoles el jugador correcto
            DeckCards.Add(container.transform.GetChild(i).gameObject);
            DeckCards[i].GetComponent<Card>().WhichPlayer=GFUtils.GetField(this.name);
        }
        ShuffleDeck(DeckCards);//Barajeando el deck
        for(int i=0;i<10;i++){DrawTopCard();}//Repartiendo 10 cartas
    }
    public GameObject DrawTopCard(){//Roba una carta del deck sin importar el espacio en la mano
        if(DeckCards.Count>0){//Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
            GameObject newCard = Instantiate(DeckCards.Last(),new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            newCard.transform.SetParent(playerArea.transform,false);//Se pone en la mano
            DeckCards.RemoveAt(DeckCards.Count-1);//Se quita de la lista
            return newCard;
        }
        return null;
    }
    public void AddCardRandomly(GameObject newCard){//Anade una carta en una posicion random del deck
        DeckCards.Add(newCard);
        SwapCardToRandomPosition(DeckCards.Count-1);
    }
    public void ShuffleDeck(List<GameObject> DeckCards){//Barajea el deck
        for(int i=0;i<DeckCards.Count;i++){//Cada carta se inserta en una posicion aleatoria
            SwapCardToRandomPosition(i);
        }
    }
    private void SwapCardToRandomPosition(int posOfCard){
        int randomPos=UnityEngine.Random.Range(0,DeckCards.Count);
        GameObject aux=DeckCards[posOfCard];
        DeckCards[posOfCard]=DeckCards[randomPos];
        DeckCards[randomPos]=aux;
    }
}