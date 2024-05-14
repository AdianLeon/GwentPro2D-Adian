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
    public void OnClickP1()//Toma una carta aleatoria y la coloca en la mano de P1
    {
        DrawCard(GameObject.Find("Hand"),this.gameObject);
    }

    public void OnClickP2()//Toma una carta aleatoria y la coloca en la mano de P2
    {
        DrawCard(GameObject.Find("EnemyHand"),this.gameObject);
    }
    public static void DrawCard(GameObject PlayerArea,GameObject PlayerDeck){
        if(PlayerArea.transform.childCount<10 && PlayerDeck.GetComponent<DrawCards>().cards.Count!=0){//Solo si el deck no esta vacio y si hay menos de 10 cartas en la mano
            GameObject picked=PlayerDeck.GetComponent<DrawCards>().cards[Random.Range(0,PlayerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            PlayerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista

        }else if(PlayerArea.transform.childCount>=10 && PlayerDeck.GetComponent<DrawCards>().cards.Count!=0){
            RoundPoints.URWrite("Has robado una carta, pero como tienes la mano llena se ha enviado al cementerio");
            GameObject picked=PlayerDeck.GetComponent<DrawCards>().cards[Random.Range(0,PlayerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano para que tenga el tamano establecido
            Graveyard.ToGraveyard(Card);//Se envia al cementerio
            PlayerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista
        }
    }
}
