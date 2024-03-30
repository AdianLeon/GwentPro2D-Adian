using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TotalFieldForce : MonoBehaviour
{
    public static int P1ForceValue=0;//Total de poder de cada jugador en la ronda
    public static int P2ForceValue=0;

    public static List <GameObject> P1PlayedCards=new List <GameObject>();
    public static List <GameObject> P2PlayedCards=new List <GameObject>();
    
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
    public static void Empty(){
        TotalFieldForce.P1PlayedCards.Clear();
        TotalFieldForce.P2PlayedCards.Clear();
        TotalFieldForce.UpdateForce();
        
        TotalFieldForce.P1ForceValue=0;
        TotalFieldForce.P2ForceValue=0;
    }
}