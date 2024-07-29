using System.Collections.Generic;
using UnityEngine;
//Script que simula un juez, se encarga del manejo de estados, la logica de turnos, rondas y condicion de victoria
public class Judge : MonoBehaviour, IStateSubscriber, IKeyboardListener
{
    private static int turnNumber;//Si es el primer turno de ese jugador
    public static int TurnNumber => turnNumber;
    private static Player playerTurn;//Jugador en turno
    public static Player GetPlayer => playerTurn;//Devuelve el jugador en turno
    public static Player GetEnemy => playerTurn == Player.P1 ? Player.P2 : Player.P1;//Devuelve el enemigo del jugador en turno
    public static bool hasPlayed;//Si se ha jugado en el turno
    public static bool HasPlayed => hasPlayed;
    private static bool isLastTurnOfRound;//Si es o no el ultimo turno antes de que acabe la ronda
    public static bool IsLastTurnOfRound => isLastTurnOfRound;
    private static bool hasGameEnded;//Si el juego se ha acabado
    public static bool CanPlay => !hasGameEnded && (!hasPlayed || isLastTurnOfRound);
    public void ListenToKeyboardPress() { if (Input.GetKeyDown(KeyCode.Space) && !Computer.IsPlaying) { EndTurnOrRound(); } }//Acaba el turno o la ronda cuando se presiona espacio
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (State.SettingUpGame, new Execution (stateInfo => ResetVars(), 0) ),
        new (State.PlayingCard, new Execution (stateInfo => hasPlayed=true, 0)),
        new (State.EndingTurn, new Execution (stateInfo => EndTurn(), 0)),
        new (State.EndingGame, new Execution (stateInfo => hasGameEnded = true, 0))
    };
    public static void EndTurnOrRound()
    {
        if (hasGameEnded) { return; }
        if (isLastTurnOfRound) { EndRound(); } else { StateManager.Publish(State.EndingTurn); }
    }
    private static void ResetVars() { hasGameEnded = false; hasPlayed = false; isLastTurnOfRound = false; turnNumber = 1; playerTurn = Player.P1; }
    private static void EndTurn()
    {//Si no se ha jugado el proximo pase terminara la ronda, en cualquier caso cambia el turno
        if (!hasPlayed) { isLastTurnOfRound = true; UserRead.Write(GetPlayer + " ha pasado el turno sin jugar!!"); }
        SwitchTurn(); hasPlayed = false;
    }
    private static void EndRound()
    {//Proxima ronda
        Player winner = Player.None;
        if (Field.P1ForceValue > Field.P2ForceValue)
        {//Si P1 tiene mas poder que P2 entonces P1 comienza el proximo turno
            winner = Player.P1;
            if (playerTurn == Player.P2) { SwitchTurn(); }//Cambiamos los turnos ya que P1 debe comenzar el proximo
            RoundPoints.AddPointToP1();//P1 gana la ronda y obtiene un punto de ronda
        }
        else if (Field.P2ForceValue > Field.P1ForceValue)
        {//Si P2 tiene mas poder que P1 entonces P2 comienza el proximo turno
            winner = Player.P2;
            if (playerTurn == Player.P1) { SwitchTurn(); }//Cambiamos los turnos ya que P2 debe comenzar el proximo
            RoundPoints.AddPointToP2();//P2 gana la ronda y obtiene un punto de ronda
        }
        else
        {//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
            SwitchTurn();
            RoundPoints.AddPointToP1();
            RoundPoints.AddPointToP2();
        }
        isLastTurnOfRound = false;
        hasPlayed = false;
        StateManager.Publish(State.EndingRound, new StateInfo { Player = winner });
    }
    private static void SwitchTurn() { playerTurn = GetEnemy; turnNumber++; }//Cede el turno al contrario y aumenta el conteo
}