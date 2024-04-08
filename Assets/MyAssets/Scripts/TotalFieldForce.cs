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
    
    void Start(){//Siempre se vacia cuando se carga la escena para evitar bugs cuando juguemos repetidas veces
        Empty();
    }
    public static void UpdateForce(){//Se actualizan los valores de fuerza total por campo
        int sum=0;
        for(int i=0;i<P1PlayedCards.Count;i++){//Se itera por todas las cartas jugadas
            sum+=P1PlayedCards[i].GetComponent<Card>().power+P1PlayedCards[i].GetComponent<Card>().addedPower;//Se suma el poder total
        }
        P1ForceValue=sum;//Esta suma se guarda
        sum=0;
        GameObject.Find("Points").GetComponent<TextMeshProUGUI>().text=P1ForceValue.ToString();//Asigna el valor al puntaje

        for(int i=0;i<P2PlayedCards.Count;i++){//Se itera por todas las cartas jugadas
            sum+=P2PlayedCards[i].GetComponent<Card>().power+P2PlayedCards[i].GetComponent<Card>().addedPower;//Se suma el poder total
        }
        P2ForceValue=sum;//Esta suma se guarda
        GameObject.Find("EnemyPoints").GetComponent<TextMeshProUGUI>().text=P2ForceValue.ToString();//Asigna el valor al puntaje
    }
    public static void AddCard(GameObject thisCard){//Anade la carta segun el campo y el tipo (Excluye a los climas, aumentos y despejes)
        if(thisCard.GetComponent<Dragging>().cardType!=Dragging.rank.Clima && thisCard.GetComponent<Dragging>().cardType!=Dragging.rank.Aumento && thisCard.GetComponent<Dragging>().cardType!=Dragging.rank.Despeje){
            if(thisCard.GetComponent<Dragging>().whichField==Dragging.fields.P1){//Si es campo de P1 anade la carta a la lista de cartas del campo del P2
                P1PlayedCards.Add(thisCard);
            }else if(thisCard.GetComponent<Dragging>().whichField==Dragging.fields.P2){//Si es campo de P2 anade la carta a la lista de cartas del campo del P2
                P2PlayedCards.Add(thisCard);
            }
        }
    }
    public static void Empty(){//Reinicia todos los valores relativos al script
        TotalFieldForce.P1PlayedCards.Clear();
        TotalFieldForce.P2PlayedCards.Clear();
        TotalFieldForce.UpdateForce();
        
        TotalFieldForce.P1ForceValue=0;
        TotalFieldForce.P2ForceValue=0;
    }
}