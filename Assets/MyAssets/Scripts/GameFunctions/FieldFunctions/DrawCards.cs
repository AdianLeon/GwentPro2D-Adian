using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        //Anadiendo las cartas del contenedor del jugador al deck
        for(int i=0;i<container.transform.childCount;i++){
            cardsInDeck.Add(container.transform.GetChild(i).gameObject);
        }
        //Asignando a todas las cartas la mano y el campo correctos
        foreach(GameObject card in cardsInDeck){
            card.GetComponent<Dragging>().Hand=playerArea;
            card.GetComponent<Card>().WhichField=deckField;
        }

        Shuffle(cardsInDeck);
        for(int i=0;i<10;i++){
            DrawTopCard();
        }
    }
    public void OnClickDraw(){//Se llama cuando se pulsa el boton
        Debug.Log("El deck: "+this.name+" ha sido presionado como boton");
        DrawTopCard();
    }
    private void DrawTopCard(){
        if(cardsInDeck.Count>0){//Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
            GameObject newCard = Instantiate(cardsInDeck.Last(),new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            newCard.transform.SetParent(playerArea.transform,false);//Se pone en la mano
            cardsInDeck.RemoveAt(cardsInDeck.Count-1);//Se quita de la lista
            if(playerArea.transform.childCount>10){//Si la carta nueva no cabe en la mano
                Graveyard.ToGraveyard(newCard);//Se envia al cementerio
            }
        }
    }
    public void Shuffle(List<GameObject> cardsInDeck){
        for(int i=0;i<cardsInDeck.Count;i++){
            int randomPos=Random.Range(0,cardsInDeck.Count);
            GameObject aux=cardsInDeck[i];
            cardsInDeck[i]=cardsInDeck[randomPos];
            cardsInDeck[randomPos]=aux;
        }
        Debug.Log("Se ha llamado a la funcion Shuffle en el deck de "+deckField);
    }
}