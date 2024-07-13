using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Diagnostics;
//Script para llevar la logica de la fuerza del campo
public class Field : StateListener, IContainer
{
    public List <GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}//Lista de las cartas jugadas del jugador
    public static List<GameObject> AllPlayedCards{get=>GFUtils.GetCardsIn(GameObject.Find("Field"));}//Lista de las cartas jugadas en el tablero
    public static List<GameObject> PlayedCardsWithoutWeathers{get=>GFUtils.GetCardsIn(GameObject.Find("PlayerFieldsSet"));}
    public static List<GameObject> PlayedWeatherCards{get=>GFUtils.GetCardsIn(GameObject.Find("WeatherZonesSet"));}
    public static List<GameObject> PlayerPlayedCards{get=>GFUtils.GetCardsIn(GameObject.Find("Field"+Judge.GetPlayer));}
    public static List<GameObject> EnemyPlayedCards{get=>GFUtils.GetCardsIn(GameObject.Find("Field"+Judge.GetEnemy));}
    private int playerForceValue{get{//Total de poder de cada jugador en la ronda
            int aux=0;
            foreach(GameObject playerCard in GetCards){//Se itera por todas las cartas jugadas
                aux+=playerCard.GetComponent<CardWithPower>().TotalPower;//Se suma el poder total
            }
            return aux;
        }
    }
    public static int P1ForceValue{get=>GameObject.Find("FieldP1").GetComponent<Field>().playerForceValue;}
    public static int P2ForceValue{get=>GameObject.Find("FieldP2").GetComponent<Field>().playerForceValue;}
    public override void CheckState(){
        if(Judge.CurrentState==State.EndingRound || Judge.CurrentState==State.EndingGame){
            GFUtils.GetRidOf(AllPlayedCards);
        }
        switch(Judge.CurrentState){
            case State.SettingUpGame:
            case State.PlayingCard:
            case State.EndingRound:
            case State.EndingGame:
                WeatherCard.RectivateAllWeathers();//Actualiza el clima
                GameObject.Find("Points"+GFUtils.GetField(this.name)).GetComponent<TextMeshProUGUI>().text=playerForceValue.ToString();//Asigna el valor al puntaje
                break;
        }
    }
}