using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Script para el funcionamiento del deck
public class Deck : StateListener, IContainer
{
    public override int GetPriority=>0;
    private int tradedCardsCount;
    public int GetTradeCount=>tradedCardsCount;
    public void OnCardTrade(){tradedCardsCount++;}
    public List<GameObject> GetCards=>DeckCards;
    public static List<GameObject> PlayerDeckCards=>GameObject.Find("Deck"+Judge.GetPlayer).GetComponent<Deck>().GetCards;//Lista de las cartas del deck del jugador en turno
    public static List<GameObject> EnemyDeckCards=>GameObject.Find("Deck"+Judge.GetEnemy).GetComponent<Deck>().GetCards;//Lista de las cartas del deck del enemigo del jugador en turno
    private List <GameObject> DeckCards = new List<GameObject>();//Lista de cartas de este deck
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame://Cuando inicia el juego se reinicia el contador de las cartas intercambiadas con el deck
                tradedCardsCount=0;//y se reparten 10 cartas a cada jugador de las cartas seleccionadas por ese jugador
                ReceiveAndDealCards();
                break;
            case State.EndingRound://Cuando se empieza una ronda se reparten dos cartas a cada jugador
                DrawTopCard();
                DrawTopCard();
                break;
            case State.EndingGame://Cuando se acaba el juego se limpia la lista
                DeckCards.Clear();
                break;
        }
    }
    private void ReceiveAndDealCards(){
        GameObject container=GameObject.Find("Cards"+gameObject.Field());
        foreach(Transform card in container.transform){//Anadiendo las cartas del contenedor del jugador al deck
            DeckCards.Add(card.gameObject);
        }
        ShuffleDeck(DeckCards);//Barajeando el deck
        for(int i=0;i<10;i++){DrawTopCard();}//Repartiendo 10 cartas
    }
    public GameObject DrawTopCard(){//Roba una carta del deck sin importar el espacio en la mano
        if(DeckCards.Count>0){//Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
            GameObject newCard = Instantiate(DeckCards.Last(),new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            newCard.transform.SetParent(GameObject.Find("Hand"+gameObject.Field()).transform,false);//Se pone en la mano
            DeckCards.RemoveAt(DeckCards.Count-1);//Se quita de la lista
            return newCard;
        }
        UserRead.Write("Se ha intentado robar una carta del deck, pero ya no quedan cartas!");
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
    private void SwapCardToRandomPosition(int posOfCard){//Cambia la carta en esa posicion con otra random
        int randomPos=UnityEngine.Random.Range(0,DeckCards.Count);
        GameObject aux=DeckCards[posOfCard];
        DeckCards[posOfCard]=DeckCards[randomPos];
        DeckCards[randomPos]=aux;
    }
}