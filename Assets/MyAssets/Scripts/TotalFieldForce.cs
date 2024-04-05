using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para llevar la logica de la fuerza del campo
public class TotalFieldForce : MonoBehaviour
{
    public static int P1ForceValue;//Total de poder de cada jugador en la ronda
    public static int P2ForceValue;

    public static List <GameObject> P1PlayedCards=new List <GameObject>();
    public static List <GameObject> P2PlayedCards=new List <GameObject>();
    
    void Start(){
        Empty();
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
    public static void AddCard(GameObject thisCard){//Anade la carta segun el campo y el tipo (Excluye a los climas)
        if(thisCard.GetComponent<Dragging>().cardType!=Dragging.rank.Clima){
            if(thisCard.GetComponent<Dragging>().whichField==Dragging.fields.P1){//Si es campo de P1 anade la carta a la lista de cartas del campo del P2
                P1PlayedCards.Add(thisCard);
            }else if(thisCard.GetComponent<Dragging>().whichField==Dragging.fields.P2){//Si es campo de P2 anade la carta a la lista de cartas del campo del P2
                P2PlayedCards.Add(thisCard);
            }
        }
    }
    public static void Empty(){
        TotalFieldForce.P1PlayedCards.Clear();
        TotalFieldForce.P2PlayedCards.Clear();
        TotalFieldForce.UpdateForce();
        
        TotalFieldForce.P1ForceValue=0;
        TotalFieldForce.P2ForceValue=0;
    }
}