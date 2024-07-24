using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Script que simula un juez, se encarga del manejo de estados, la logica de turnos, rondas y condicion de victoria
public class Judge : MonoBehaviour, IKeyboardListener
{
    private static IEnumerable<IStateListener> stateListeners;
    private static State currentState;//Estado actual del juego
    public static State CurrentState
    {
        get => currentState; private set
        {//Si se desea obtener el estado se devuelve sin problemas sin embargo solo el juez puede cambiar el estado
            currentState = value;
            stateListeners.ForEach(script => script.CheckState());
            currentState = State.WaitingPlayerAction;//Se espera la proxima accion del jugador
        }
    }
    private static bool isFirstTurnOfPlayer;//Si es el primer turno de ese jugador
    public static bool IsFirstTurnOfPlayer => isFirstTurnOfPlayer;
    private static Player playerTurn;//Jugador en turno
    public static Player GetPlayer => playerTurn;//Devuelve el jugador en turno
    public static Player GetEnemy => playerTurn == Player.P1 ? Player.P2 : Player.P1;//Devuelve el enemigo del jugador en turno
    public static bool hasPlayed;//Si se ha jugado en el turno
    public static bool HasPlayed => hasPlayed;
    private static bool isLastTurnOfRound;//Si es o no el ultimo turno antes de que acabe la ronda
    public static bool IsLastTurnOfRound => isLastTurnOfRound;
    private static bool hasGameEnded;//Si el juego se ha acabado
    public static bool CanPlay => !hasPlayed || isLastTurnOfRound;
    void Update() { GFUtils.FindGameObjectsOfType<IKeyboardListener>().ForEach(listener => listener.ListenToKeyboardPress()); }//Hace posible las respuestas ante las presiones de teclas
    public void ListenToKeyboardPress()
    {//Acaba el turno cuando se presiona espacio
        if (Input.GetKeyDown(KeyCode.Space) && !Computer.IsPlaying)
        {
            Debug.Log("Presionado ESPACIO");
            EndTurnOrRound();
        }
    }
    void Start()
    {//En el inicio de la escena se cargan las cartas y se reinicia el juego
        stateListeners = GFUtils.FindGameObjectsOfType<IStateListener>().OrderBy(script => script.GetPriority);
        CurrentState = State.LoadingCards;
        ResetGame();
    }
    public static void ResetGame()
    {//Reinicia el juego. Este metodo es llamado por un boton llamado ResetGameButton
        hasPlayed = false;
        isLastTurnOfRound = false;
        isFirstTurnOfPlayer = true;
        hasGameEnded = false;
        playerTurn = Player.P1;//El Player 1 inicia la partida siempre
        CurrentState = State.SettingUpGame;
    }
    public static void EndTurnOrRound()
    {
        if (hasGameEnded) { return; }
        if (isLastTurnOfRound) { NextRound(); } else { EndTurn(); }
        GameObject.Find("Computer").GetComponent<Computer>().TryPlay();//Se chequean las condiciones para que la computadora juegue el turno

    }
    public static void OnPlayedCard()
    {//Se llama cuando se juega una carta
        hasPlayed = true;//El jugador acaba de jugar una carta
        CurrentState = State.PlayingCard;
    }
    private static void EndTurn()
    {//Se llama con cada click en el boton PASS o cuando se presiona espacio
        if (!hasPlayed)
        {//Detecta caundo un jugador pasa sin jugar
            SwitchTurn();
            isLastTurnOfRound = true;//Activa el modo ultimo turno, cuando se presione el pass de nuevo se acabara la ronda
        }
        else
        {
            SwitchTurn();
        }
        hasPlayed = false;
        CurrentState = State.EndingTurn;
    }
    private static void NextRound()
    {//Proxima ronda
        if (Field.P1ForceValue > Field.P2ForceValue)
        {//Si P1 tiene mas poder que P2 entonces P1 comienza el proximo turno
            if (playerTurn == Player.P2) { SwitchTurn(); }//Cambiamos los turnos ya que P1 debe comenzar el proximo
            RoundPoints.AddPointToP1();//P1 gana la ronda y obtiene un punto de ronda
            UserRead.Write("P1 gano la ronda");
        }
        else if (Field.P2ForceValue > Field.P1ForceValue)
        {//Si P2 tiene mas poder que P1 entonces P2 comienza el proximo turno
            if (playerTurn == Player.P1) { SwitchTurn(); }//Cambiamos los turnos ya que P2 debe comenzar el proximo
            RoundPoints.AddPointToP2();//P2 gana la ronda y obtiene un punto de ronda
            UserRead.Write("P2 gano la ronda");
        }
        else
        {//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
            SwitchTurn();
            RoundPoints.AddPointToP1();
            RoundPoints.AddPointToP2();
            UserRead.Write("Ha ocurrido un empate");
        }
        if (CheckGameWin())
        {//Si se gana el juego
            return;
        }
        isLastTurnOfRound = false;
        hasPlayed = false;
        CurrentState = State.EndingRound;
    }
    private static void SwitchTurn()
    {//Se cambia de turno
        playerTurn = GetEnemy;//Es el turno del contrario
        if (playerTurn == Player.P1) { isFirstTurnOfPlayer = false; }
    }
    //Condicion de victoria
    private static bool CheckGameWin()
    {//Chequea quien ha ganado el juego
        if (RoundPoints.GetRPointsP1 != RoundPoints.GetRPointsP2)
        {//Si la puntuacion es diferente (esto obliga a que el juego siga hasta que haya una ventaja)
            if (RoundPoints.GetRPointsP1 > 1)
            {//El primero que llegue a 2 puntos de ronda gana
                WinsGame(Player.P1); return true;
            }
            else if (RoundPoints.GetRPointsP2 > 1)
            {
                WinsGame(Player.P2); return true;
            }
        }
        else
        {
            UserRead.Write("El proximo jugador que gane una ronda gana el juego!!");
        }
        return false;
    }
    private static void WinsGame(Player player)
    {//El jugador gana la partida
        hasGameEnded = true;
        playerTurn = player;//Se guarda en playerTurn el jugador ganador para que otros stateListeners puedan acceder al ganador del juego mediante GetPlayerTurn
        CurrentState = State.EndingGame;
    }
}