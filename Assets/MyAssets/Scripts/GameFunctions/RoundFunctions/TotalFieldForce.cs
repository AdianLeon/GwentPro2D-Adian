using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para llevar la logica de la fuerza del campo
public class TotalFieldForce : MonoBehaviour
{
    public static int p1ForceValue;//Total de poder de cada jugador en la ronda
    public static int p2ForceValue;

    public static List <GameObject> p1PlayedCards=new List <GameObject>();//Lista de las cartas jugadas de cada jugador
    public static List <GameObject> p2PlayedCards=new List <GameObject>();
    
    void Start(){//Siempre se vacia cuando se carga la escena para evitar bugs cuando juguemos repetidas veces
        ResetPlayedCards();
    }
    public static void UpdateForce(){//Se actualizan los valores de fuerza total por campo
        int sum=0;
        for(int i=0;i<p1PlayedCards.Count;i++){//Se itera por todas las cartas jugadas
            sum+=p1PlayedCards[i].GetComponent<CardWithPower>().power+p1PlayedCards[i].GetComponent<CardWithPower>().AddedPower;//Se suma el poder total
        }
        p1ForceValue=sum;//Esta suma se guarda
        GameObject.Find("PointsP1").GetComponent<TextMeshProUGUI>().text=p1ForceValue.ToString();//Asigna el valor al puntaje
        
        sum=0;
        for(int i=0;i<p2PlayedCards.Count;i++){//Se itera por todas las cartas jugadas
            sum+=p2PlayedCards[i].GetComponent<CardWithPower>().power+p2PlayedCards[i].GetComponent<CardWithPower>().AddedPower;//Se suma el poder total
        }
        p2ForceValue=sum;//Esta suma se guarda
        GameObject.Find("PointsP2").GetComponent<TextMeshProUGUI>().text=p2ForceValue.ToString();//Asigna el valor al puntaje
    }
    public static void AddCard(GameObject thisCard){//Anade la carta segun el campo y el tipo (Solo cartas con poder)
        if(thisCard.GetComponent<CardWithPower>()!=null){
            if(thisCard.GetComponent<Card>().WhichField==fields.P1){//Si es campo de P1 anade la carta a la lista de cartas del campo del P1
                p1PlayedCards.Add(thisCard);
            }else if(thisCard.GetComponent<Card>().WhichField==fields.P2){//Si es campo de P2 anade la carta a la lista de cartas del campo del P2
                p2PlayedCards.Add(thisCard);
            }
        }
    }
    public static void RemoveCard(GameObject thisCard){//Quita la carta de la lista en la que se encuentra
        if(thisCard.GetComponent<Card>().WhichField==fields.P1){//Si es del campo de P1 quita la carta de la lista de cartas del campo del P1
            p1PlayedCards.Remove(thisCard);
        }else if(thisCard.GetComponent<Card>().WhichField==fields.P2){//Si es del campo de P2 quita la carta de la lista de cartas del campo del P2
            p2PlayedCards.Remove(thisCard);
        }
    }
    public static void ResetPlayedCards(){//Reinicia todos los valores relativos al script
        TotalFieldForce.p1PlayedCards.Clear();
        TotalFieldForce.p2PlayedCards.Clear();
        TotalFieldForce.UpdateForce();
        
        TotalFieldForce.p1ForceValue=0;
        TotalFieldForce.p2ForceValue=0;
    }
}