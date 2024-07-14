using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para llevar la logica de la fuerza de cada jugador y las cartas jugadas en el campo
public class Field : StateListener, IContainer
{
    public List <GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}//Lista de las cartas jugadas en el campo
    public static List<GameObject> AllPlayedCards{get=>GFUtils.GetCardsIn(GameObject.Find("Field"));}//Lista de todas las cartas jugadas en el tablero
    public static List<GameObject> PlayedCardsWithoutWeathers{get=>GFUtils.GetCardsIn(GameObject.Find("PlayerFieldsSet"));}//Lista de todas las cartas excluyendo climas
    public static List<GameObject> PlayedWeatherCards{get=>GFUtils.GetCardsIn(GameObject.Find("WeatherZonesSet"));}//Lista de las cartas clima jugadas
    public static List<GameObject> PlayerPlayedCards{get=>GFUtils.GetCardsIn(GameObject.Find("Field"+Judge.GetPlayer));}//Lista de las cartas jugadas del jugador en turno
    public static List<GameObject> EnemyPlayedCards{get=>GFUtils.GetCardsIn(GameObject.Find("Field"+Judge.GetEnemy));}//Lista de las cartas jugadas del enemigo del jugador en turno
    private int playerForceValue{get{//Total de poder de cada jugador en la ronda
            int aux=0;
            foreach(GameObject playerCard in GetCards){//Se itera por todas las cartas jugadas
                aux+=playerCard.GetComponent<CardWithPower>().TotalPower;//Se suma el poder total
            }
            return aux;
        }
    }
    public static int P1ForceValue{get=>GameObject.Find("FieldP1").GetComponent<Field>().playerForceValue;}//Fuerza total de P1
    public static int P2ForceValue{get=>GameObject.Find("FieldP2").GetComponent<Field>().playerForceValue;}//Fuerza total de P2
    public override void CheckState(){
        if(Judge.CurrentState==State.EndingGame){//Si estamos terminando el juego las cartas que queden en el campo se iran a la basura
            GFUtils.GetRidOf(AllPlayedCards);
        }
        switch(Judge.CurrentState){
            case State.SettingUpGame:
            case State.EndingRound:
            case State.EndingGame:
                GameObject.Find("Points"+GFUtils.GetField(this.name)).GetComponent<TextMeshProUGUI>().text="0";//Resetea el valor del puntaje
                break;
            case State.PlayingCard:
                WeatherCard.RectivateAllWeathers();//Actualiza el clima
                GameObject.Find("Points"+GFUtils.GetField(this.name)).GetComponent<TextMeshProUGUI>().text=playerForceValue.ToString();//Calcula y asigna el valor al puntaje
                break;
        }
    }
}