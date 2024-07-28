using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script que representa los puntos de ronda de cada jugador
public class RoundPoints : MonoBehaviour, IStateSubscriber
{
    private int rPoints;//Puntos de ronda de cada jugador
    public static int GetRPointsP1 => GameObject.Find("RoundPointsP1").GetComponent<RoundPoints>().rPoints;
    public static void AddPointToP1() => GameObject.Find("RoundPointsP1").GetComponent<RoundPoints>().rPoints++;
    public static int GetRPointsP2 => GameObject.Find("RoundPointsP2").GetComponent<RoundPoints>().rPoints;
    public static void AddPointToP2() => GameObject.Find("RoundPointsP2").GetComponent<RoundPoints>().rPoints++;
    private int getMarks => gameObject.GetComponent<TextMeshProUGUI>().text.Length; //Cantidad de marcas de cada jugador
    public List<StateSubscription> GetStateSubscriptions => new()
    {//Al iniciar el juego reinicia los puntos de cada jugador. Actualiza los puntos siempre y al final de las rondas chequea si hay ganador
        new(State.SettingUpGame, new Execution(stateInfo => rPoints = 0, 0)),
        new(new Execution(stateInfo => UpdatePoints(), 1 )),
        new(State.EndingRound,  new Execution(stateInfo => CheckGameWin(), 2))
    };
    private void UpdatePoints()
    {//Hace que la cantidad de marcas sea igual a los puntos de cada jugador, si llega un nuevo punto de ronda se hace una nueva marca
        if (rPoints == 0) { gameObject.GetComponent<TextMeshProUGUI>().text = ""; return; }
        while (getMarks < rPoints) { gameObject.GetComponent<TextMeshProUGUI>().text += "X"; }
    }
    private static void CheckGameWin()
    {//Si los jugadores, se guarda en el turno para que pueda ser conocido por otros IStateListeners
        if (GetRPointsP1 == GetRPointsP2) { UserRead.Write("El proximo jugador que gane una ronda gana el juego!!"); return; }
        if (GetRPointsP1 > 1 || GetRPointsP2 > 1)
        {
            if (GetRPointsP1 - GetRPointsP2 > 0) { StateManager.Publish(State.EndingGame, new StateInfo { Player = Player.P1 }); }
            else { StateManager.Publish(State.EndingGame, new StateInfo { Player = Player.P2 }); }
        }
    }
}