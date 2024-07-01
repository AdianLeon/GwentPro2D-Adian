using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para el funcionamiento del deck y la habilidad de lider
public class DrawCards : MonoBehaviour
{
    public GameObject container;//Este objeto contiene todas las cartas que van a ser anadidas a la lista
    public GameObject playerArea;//Esta es la mano del jugador dueno de este deck
    public fields deckField;//Este es el campo del jugador dueno de este deck
    public List <GameObject> cardsInDeck = new List <GameObject>();//Lista de cartas
    void Start(){
        for(int i=0;i<container.transform.childCount;i++){
            cardsInDeck.Add(container.transform.GetChild(i).gameObject);
        }
        foreach(GameObject card in cardsInDeck){
            card.GetComponent<Dragging>().hand=playerArea;
            card.GetComponent<Card>().WhichField=deckField;
        }
        for(int i=0;i<10;i++){
            DrawCard();
        }
    }
    public void OnClickDraw(){//Se llama cuando se pulsa el boton
        Debug.Log("El deck: "+this.name+" ha sido presionado como boton");
        DrawCard();
    }
    public void DrawCard(){
        if(this.GetComponent<DrawCards>().cardsInDeck.Count!=0){//Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
            GameObject picked=this.GetComponent<DrawCards>().cardsInDeck[Random.Range(0,this.GetComponent<DrawCards>().cardsInDeck.Count)];//La escogida es aleatoria
            GameObject newCard = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            newCard.transform.SetParent(playerArea.transform,false);//Se pone en la mano
            this.GetComponent<DrawCards>().cardsInDeck.Remove(picked);//Se quita de la lista
            if(playerArea.transform.childCount>10){//Si la carta nueva no cabe en la mano
                Graveyard.ToGraveyard(newCard);//Se envia al cementerio
            }
        }
    }
    //public void Shuffle
}