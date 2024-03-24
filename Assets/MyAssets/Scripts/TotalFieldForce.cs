using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TotalFieldForce : MonoBehaviour
{
    public static int P1ForceValue=0;//Total de poder de cada jugador en la ronda
    public static int P2ForceValue=0;

    public static int P1CardsLeft=25;
    public static int P2CardsLeft=25;

    public static List <GameObject> P1PlayedCards=new List <GameObject>();
    public static List <GameObject> P2PlayedCards=new List <GameObject>();

    public static void UpdateP1Deck(){//Actualiza el texto debajo del deck del p1 con las cartas que le quedan
        GameObject.Find("P1Deck").GetComponent<TextMeshProUGUI>().text=P1CardsLeft.ToString();
    }
    public static void UpdateP2Deck(){//Actualiza el texto debajo del deck del p2 con las cartas que le quedan
        GameObject.Find("P2Deck").GetComponent<TextMeshProUGUI>().text=P2CardsLeft.ToString();
    }
    public static void UpdateForce(){
        int sum=0;
        for(int i=0;i<P1PlayedCards.Count;i++){
            sum+=P1PlayedCards[i].GetComponent<Card>().power+P1PlayedCards[i].GetComponent<Card>().addedPower;
        }
        P1ForceValue=sum;
        sum=0;
        GameObject.Find("Points").GetComponent<TextMeshProUGUI>().text=P1ForceValue.ToString();//Asigna el valor al puntaje
        for(int i=0;i<P2PlayedCards.Count;i++){
            sum+=P2PlayedCards[i].GetComponent<Card>().power+P2PlayedCards[i].GetComponent<Card>().addedPower;
        }
        P2ForceValue=sum;
        GameObject.Find("EnemyPoints").GetComponent<TextMeshProUGUI>().text=P2ForceValue.ToString();
    }
}