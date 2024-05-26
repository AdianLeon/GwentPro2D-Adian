using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para el funcionamiento del deck y la habilidad de lider
public class DrawCards : MonoBehaviour
{
    public GameObject container;//Este objeto contiene todas las cartas que van a ser anadidas a la lista
    public GameObject playerArea;//Esta es la mano del jugador dueno de este deck
    public Card.fields deckField;//Este es el campo del jugador dueno de este deck
    public List <GameObject> cards = new List <GameObject>();//Lista de cartas
    void Start(){
        for(int i=0;i<container.transform.childCount;i++){
            cards.Add(container.transform.GetChild(i).gameObject);
        }
        for(int i=0;i<cards.Count;i++){
            cards[i].GetComponent<Dragging>().hand=playerArea;
            cards[i].GetComponent<Card>().whichField=deckField;
        }
        for(int i=0;i<10;i++){
            DrawCard(playerArea,this.gameObject);
        }
    }
    public void OnClickDraw(){//Toma una carta aleatoria y la coloca en la mano correspondiente
        DrawCard(playerArea,this.gameObject);
    }
    public static void DrawCard(GameObject playerArea,GameObject playerDeck){
        if(playerDeck.GetComponent<DrawCards>().cards.Count!=0){//Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
            GameObject picked=playerDeck.GetComponent<DrawCards>().cards[Random.Range(0,playerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
            GameObject newCard = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            newCard.transform.SetParent(playerArea.transform,false);//Se pone en la mano
            playerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista
            if(playerArea.transform.childCount>10){//Si la carta nueva no cabe en la mano
                Graveyard.ToGraveyard(newCard);//Se envia al cementerio
            }
        }
    }
}