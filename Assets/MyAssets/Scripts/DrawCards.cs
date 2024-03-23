using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para el deck
public class DrawCards : MonoBehaviour
{
    public GameObject Card1,Card2,Card3,Card4,Card5,Card6,Card7,Card8,Card9,Card10,Card11,Card12,Card13,Card14,Card15,
    Card16,Card17,Card18,Card19,Card20,Card21,Card22,Card23,Card24,Card25,Card26;
    public GameObject PlayerArea;
    List <GameObject> cards = new List <GameObject>();//Lista de cartas
    static int n=0;
    void Start()
    {
        //Anadiendo cartas al deck
        n++;
        cards.Add(Card1);cards.Add(Card2);cards.Add(Card3);cards.Add(Card4);cards.Add(Card5);cards.Add(Card6);cards.Add(Card7);
        cards.Add(Card8);cards.Add(Card9);cards.Add(Card10);cards.Add(Card11);cards.Add(Card12);cards.Add(Card13);cards.Add(Card14);
        cards.Add(Card15);cards.Add(Card16);cards.Add(Card17);cards.Add(Card18);cards.Add(Card19);cards.Add(Card20);cards.Add(Card21);
        cards.Add(Card22);cards.Add(Card23);cards.Add(Card24);cards.Add(Card25);cards.Add(Card26);
        if(n==2){//Este Start es ejecutado por dos decks, por eso se reparten las cartas la segunda vez porque es entonces cuando
        //ambos tienen todas sus cartas
            for(int i=0;i<10;i++){
                GameObject.Find("Deck").GetComponent<Button>().onClick.Invoke();
                GameObject.Find("EnemyDeck").GetComponent<Button>().onClick.Invoke();
            }
        }
    }
    //Toma una carta aleatoria y la coloca en la mano del jugador
    public void OnClickP1()
    {
        PlayerArea=GameObject.Find("Hand");
        if(PlayerArea.transform.childCount<10 && cards.Count!=0)//Solo si el deck no esta vacio y si hay menos de 10 cartas en la mano
        {
            GameObject picked=cards[Random.Range(0,cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            cards.Remove(picked);//Se quita de la lista
            TotalFieldForce.P1CardsLeft--;//Se reduce en uno el total de cartas en el deck
            TotalFieldForce.UpdateP1Deck();//Asigna cuantas quedan al texto debajo del deck(P1)

        }else if(PlayerArea.transform.childCount>=10 && cards.Count!=0){
            Debug.Log("Has robado una carta, pero como tienes la mano llena se ha enviado al cementerio (P1)");
            GameObject picked=cards[Random.Range(0,cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano para que tenga el tamano establecido
            Graveyard.ToGraveyard(Card);//Se envia al cementerio
            cards.Remove(picked);//Se quita de la lista
            TotalFieldForce.P1CardsLeft--;//Se reduce en uno el total de cartas en el deck
            TotalFieldForce.UpdateP1Deck();//Asigna cuantas quedan al texto debajo del deck(P1)
        }
    }

    public void OnClickP2()
    {
        PlayerArea=GameObject.Find("EnemyHand");
        if(PlayerArea.transform.childCount<10 && cards.Count!=0)//Solo si el deck no esta vacio y si hay menos de 10 cartas en la mano
        {
            GameObject picked=cards[Random.Range(0,cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            cards.Remove(picked);//Se quita de la lista
            TotalFieldForce.P2CardsLeft--;//Se reduce en uno el total de cartas en el deck
            TotalFieldForce.UpdateP2Deck();//Asigna cuantas quedan al texto debajo del deck(P2)
        }else if(PlayerArea.transform.childCount>=10 && cards.Count!=0){
            Debug.Log("Has robado una carta, pero como tienes la mano llena se ha enviado al cementerio (P2)");
            GameObject picked=cards[Random.Range(0,cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano para que tenga el tamano establecido
            Graveyard.ToGraveyard(Card);//Se envia al cementerio
            cards.Remove(picked);//Se quita de la lista
            TotalFieldForce.P2CardsLeft--;//Se reduce en uno el total de cartas en el deck
            TotalFieldForce.UpdateP2Deck();//Asigna cuantas quedan al texto debajo del deck(P2)
        }
    }
}
