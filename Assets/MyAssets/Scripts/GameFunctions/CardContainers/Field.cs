using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Diagnostics;
//Script para llevar la logica de la fuerza del campo
public class Field : MonoBehaviour, IContainer
{
    public List <GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}//Lista de las cartas jugadas del jugador
    public static List<GameObject> P1PlayedCards{get=>GameObject.Find("FieldP1").GetComponent<Field>().GetCards;}
    public static List<GameObject> P2PlayedCards{get=>GameObject.Find("FieldP2").GetComponent<Field>().GetCards;}
    public static List<GameObject> PlayedCardsWithoutWeathers{
        get{
            List<GameObject> all=new List<GameObject>();
            all.AddRange(P1PlayedCards);
            all.AddRange(P2PlayedCards);
            return all;
        }
    }
    private int playerForceValue;//Total de poder de cada jugador en la ronda
    public static int P1ForceValue{get=>GameObject.Find("FieldP1").GetComponent<Field>().playerForceValue;}
    public static int P2ForceValue{get=>GameObject.Find("FieldP2").GetComponent<Field>().playerForceValue;}
    void Start(){//Siempre se vacia cuando se carga la escena para evitar bugs cuando juguemos repetidas veces
        UpdateForce();
    }
    public static void UpdateAllForces(){
        GameObject.Find("FieldP1").GetComponent<Field>().UpdateForce();
        GameObject.Find("FieldP2").GetComponent<Field>().UpdateForce();
    }
    private void UpdateForce(){//Se actualizan los valores de fuerza total por campo
        playerForceValue=0;
        for(int i=0;i<GetCards.Count;i++){//Se itera por todas las cartas jugadas
            playerForceValue+=GetCards[i].GetComponent<CardWithPower>().TotalPower;//Se suma el poder total
        }
        GameObject.Find("Points"+GFUtils.GetField(this.name)).GetComponent<TextMeshProUGUI>().text=playerForceValue.ToString();//Asigna el valor al puntaje
    }
}