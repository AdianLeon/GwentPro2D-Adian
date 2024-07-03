using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//Script para el funcionamiento del deck y la habilidad de lider
public class Deck : MonoBehaviour
{
    private GameObject container;//Este objeto contiene todas las cartas que van a ser anadidas a la lista
    private GameObject playerArea;//Esta es la mano del jugador dueno de este deck
    private Fields deckField;//Este es el campo del jugador dueno de este deck
    public List <GameObject> DeckCards = new List <GameObject>();//Lista de cartas
    void Start(){
        string player=this.name[this.name.Length-2].ToString()+name[this.name.Length-1].ToString();
        container=GameObject.Find("Cards"+player);
        playerArea=GameObject.Find("Hand"+player);
        deckField=(Fields)Enum.Parse(typeof(Fields),player);
        
        //Anadiendo las cartas del contenedor del jugador al deck
        for(int i=0;i<container.transform.childCount;i++){
            DeckCards.Add(container.transform.GetChild(i).gameObject);
        }
        //Asignando a todas las cartas la mano y el campo correctos
        foreach(GameObject card in DeckCards){
            card.GetComponent<Dragging>().Hand=playerArea;
            card.GetComponent<Card>().WhichField=deckField;
        }

        ShuffleDeck(DeckCards);
        for(int i=0;i<10;i++){
            DrawTopCard();
        }
    }
    public void DrawTopCard(){//Roba una carta del deck y si no cabe en la mano la envia a el cementerio
        if(DeckCards.Count>0){//Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
            GameObject newCard = Instantiate(DeckCards.Last(),new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            newCard.transform.SetParent(playerArea.transform,false);//Se pone en la mano
            DeckCards.RemoveAt(DeckCards.Count-1);//Se quita de la lista
            if(playerArea.transform.childCount>10){//Si la carta nueva no cabe en la mano
                Graveyard.ToGraveyard(newCard);//Se envia al cementerio
            }
        }
    }
    public GameObject ForceDrawTopCard(){//Roba una carta del deck sin importar el espacio en la mano
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
        Debug.Log("Se ha llamado a la funcion Shuffle en el deck de "+deckField);
    }
    private void SwapCardToRandomPosition(int posOfCard){
        int randomPos=UnityEngine.Random.Range(0,DeckCards.Count);
        GameObject aux=DeckCards[posOfCard];
        DeckCards[posOfCard]=DeckCards[randomPos];
        DeckCards[randomPos]=aux;
    }
}