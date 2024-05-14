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

    public static List <GameObject> P1PlayedCards=new List <GameObject>();//Lista de las cartas jugadas de cada jugador
    public static List <GameObject> P2PlayedCards=new List <GameObject>();
    
    void Start(){//Siempre se vacia cuando se carga la escena para evitar bugs cuando juguemos repetidas veces
        ResetPlayedCards();
    }
    public static void UpdateForce(){//Se actualizan los valores de fuerza total por campo
        int sum=0;
        for(int i=0;i<P1PlayedCards.Count;i++){//Se itera por todas las cartas jugadas
            sum+=P1PlayedCards[i].GetComponent<UnitCard>().power+P1PlayedCards[i].GetComponent<UnitCard>().addedPower;//Se suma el poder total
        }
        P1ForceValue=sum;//Esta suma se guarda
        sum=0;
        GameObject.Find("Points").GetComponent<TextMeshProUGUI>().text=P1ForceValue.ToString();//Asigna el valor al puntaje

        for(int i=0;i<P2PlayedCards.Count;i++){//Se itera por todas las cartas jugadas
            sum+=P2PlayedCards[i].GetComponent<UnitCard>().power+P2PlayedCards[i].GetComponent<UnitCard>().addedPower;//Se suma el poder total
        }
        P2ForceValue=sum;//Esta suma se guarda
        GameObject.Find("EnemyPoints").GetComponent<TextMeshProUGUI>().text=P2ForceValue.ToString();//Asigna el valor al puntaje
    }
    public static void AddCard(GameObject thisCard){//Anade la carta segun el campo y el tipo (Excluye a los climas, aumentos y despejes)
        if(thisCard.GetComponent<Card>().whichZone!=Card.zones.Weather && thisCard.GetComponent<Card>().whichZone!=Card.zones.Boost){
            if(thisCard.GetComponent<Card>().whichField==Card.fields.P1){//Si es campo de P1 anade la carta a la lista de cartas del campo del P1
                P1PlayedCards.Add(thisCard);
            }else if(thisCard.GetComponent<Card>().whichField==Card.fields.P2){//Si es campo de P2 anade la carta a la lista de cartas del campo del P2
                P2PlayedCards.Add(thisCard);
            }
        }
    }
    public static void RemoveCard(GameObject thisCard){//Quita la carta de la lista en la que se encuentra
        if(thisCard.GetComponent<Card>().whichField==Card.fields.P1){//Si es del campo de P1 quita la carta de la lista de cartas del campo del P1
            P1PlayedCards.Remove(thisCard);
        }else if(thisCard.GetComponent<Card>().whichField==Card.fields.P2){//Si es del campo de P2 quita la carta de la lista de cartas del campo del P2
            P2PlayedCards.Remove(thisCard);
        }
    }
    public static void ResetPlayedCards(){//Reinicia todos los valores relativos al script
        TotalFieldForce.P1PlayedCards.Clear();
        TotalFieldForce.P2PlayedCards.Clear();
        TotalFieldForce.UpdateForce();
        
        TotalFieldForce.P1ForceValue=0;
        TotalFieldForce.P2ForceValue=0;
    }
}