using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para el funcionamiento del deck y la habilidad de lider
public class DrawCards : MonoBehaviour
{
    public GameObject PlayerArea;
    public List <GameObject> cards = new List <GameObject>();//Lista de cartas
    void Start(){
        for(int i=0;i<10;i++){
            this.gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }
    public void OnClickP1(){//Toma una carta aleatoria y la coloca en la mano de P1
        DrawCard(GameObject.Find("Hand"),this.gameObject);
    }
    public void OnClickP2(){//Toma una carta aleatoria y la coloca en la mano de P2
        DrawCard(GameObject.Find("EnemyHand"),this.gameObject);
    }
    public static void DrawCard(GameObject PlayerArea,GameObject PlayerDeck){
        if(PlayerDeck.GetComponent<DrawCards>().cards.Count!=0){//Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
            GameObject picked=PlayerDeck.GetComponent<DrawCards>().cards[Random.Range(0,PlayerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
            GameObject newCard = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            newCard.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            PlayerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista
            if(PlayerArea.transform.childCount>10){//Si la carta nueva no cabe en la mano
                Graveyard.ToGraveyard(newCard);//Se envia al cementerio
            }
        }
    }
}