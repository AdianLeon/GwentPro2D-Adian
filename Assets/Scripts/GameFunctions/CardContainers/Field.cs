using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
//Script para llevar la logica de la fuerza de cada jugador y las cartas jugadas en el campo
public class Field : MonoBehaviour, IStateSubscriber, IContainer
{
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (State.PlayingCard, new Execution (stateInfo => ReactivateAllWeathers(), 1)),
        new (State.EndingRound, new Execution (stateInfo => Graveyard.SendToGraveyard(AllPlayedCards), 1)),
        new (State.EndingGame, new Execution (stateInfo => AllPlayedCards.Disappear(), 1)),
        new (new Execution(stateInfo => UpdateForce(), 1))
    };
    public IEnumerable<DraggableCard> GetCards => gameObject.CardsInside<DraggableCard>();//Lista de las cartas jugadas en el campo
    public static IEnumerable<DraggableCard> AllPlayedCards => GameObject.Find("Board").CardsInside<DraggableCard>();//Lista de todas las cartas jugadas en el tablero
    public static IEnumerable<WeatherCard> PlayedWeatherCards => GameObject.Find("WeatherZonesSet").CardsInside<WeatherCard>();//Lista de las cartas clima jugadas
    public static IEnumerable<PowerCard> PlayedFieldCards => GameObject.Find("PlayerFieldsSet").CardsInside<PowerCard>();//Lista de todas las cartas en los campos (excluyendo climas)
    public static IEnumerable<PowerCard> PlayerCards => GameObject.Find("Field" + Judge.GetPlayer).CardsInside<PowerCard>();//Lista de las cartas jugadas del jugador en turno
    public static IEnumerable<PowerCard> EnemyCards => GameObject.Find("Field" + Judge.GetEnemy).CardsInside<PowerCard>();//Lista de las cartas jugadas del enemigo del jugador en turno
    private int playerForceValue => GetCards.Sum(card => card.GetComponent<PowerCard>().TotalPower);//Suma cada vez que se llame el poder total de las cartas del campo
    public static int P1ForceValue => GameObject.Find("FieldP1").GetComponent<Field>().playerForceValue;//Fuerza total de P1
    public static int P2ForceValue => GameObject.Find("FieldP2").GetComponent<Field>().playerForceValue;//Fuerza total de P2
    private void UpdateForce() => GameObject.Find("Points" + gameObject.Field()).GetComponent<TextMeshProUGUI>().text = playerForceValue.ToString();//Calcula y actualiza la fuerza del jugador
    private static void ReactivateAllWeathers() => PlayedWeatherCards.ForEach(weather => weather.GetComponent<WeatherCard>().TriggerSpecialEffect());//Reactiva los efectos de todas las cartas clima jugadas
}