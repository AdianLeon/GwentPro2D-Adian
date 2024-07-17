using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para llevar la logica de la fuerza de cada jugador y las cartas jugadas en el campo
public class Field : StateListener, IContainer
{
    public override int GetPriority=>1;
    public List <GameObject> GetCards=>gameObject.CardsInside();//Lista de las cartas jugadas en el campo
    public static List<GameObject> AllPlayedCards=>GameObject.Find("Field").CardsInside();//Lista de todas las cartas jugadas en el tablero
    public static List<GameObject> PlayedCardsWithoutWeathers=>GameObject.Find("PlayerFieldsSet").CardsInside();//Lista de todas las cartas excluyendo climas
    public static List<GameObject> PlayedWeatherCards=>GameObject.Find("WeatherZonesSet").CardsInside();//Lista de las cartas clima jugadas
    public static List<GameObject> PlayerPlayedCards=>GameObject.Find("Field"+Judge.GetPlayer).CardsInside();//Lista de las cartas jugadas del jugador en turno
    public static List<GameObject> EnemyPlayedCards=>GameObject.Find("Field"+Judge.GetEnemy).CardsInside();//Lista de las cartas jugadas del enemigo del jugador en turno
    private int playerForceValue{get{//Total de poder de cada jugador en la ronda
            int aux=0;
            foreach(GameObject playerCard in GetCards){//Se itera por todas las cartas jugadas
                aux+=playerCard.GetComponent<PowerCard>().TotalPower;//Se suma el poder total
            }
            return aux;
        }
    }
    public static int P1ForceValue=>GameObject.Find("FieldP1").GetComponent<Field>().playerForceValue;//Fuerza total de P1
    public static int P2ForceValue=>GameObject.Find("FieldP2").GetComponent<Field>().playerForceValue;//Fuerza total de P2
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.PlayingCard://Si jugamos una carta reactivamos todos los climas
                ReactivateAllWeathers();
                break;
            case State.EndingRound:
                Graveyard.SendToGraveyard(AllPlayedCards);//Cuando se acaba la ronda todas las cartas del campo son enviadas al cementerio
                break;
            case State.EndingGame://Si se acabo el juego limpiamos todo el tablero de cartas
                AllPlayedCards.Disappear();
                break;
        }
        switch(Judge.CurrentState){
            case State.SettingUpGame://Calcula y asigna el valor al puntaje
            case State.PlayingCard:
            case State.EndingTurn:
            case State.EndingRound:
            case State.EndingGame:
                GameObject.Find("Points"+gameObject.Field()).GetComponent<TextMeshProUGUI>().text=playerForceValue.ToString();
                break;
        }
    }
    private static void ReactivateAllWeathers(){//Reactiva los efectos de todas las cartas clima jugadas
        foreach(GameObject weather in PlayedWeatherCards){//Itera por cada uno de esos hijos
            weather.GetComponent<WeatherCard>().TriggerSpecialEffect();//Hace que activen el efecto de clima otra vez
        }
    }
}