using UnityEngine;
//Script que simula un juez, se encarga del manejo de estados, la logica de turnos, rondas y condicion de victoria
public class Judge : MonoBehaviour
{
    private static State currentSate;//Estado actual del juego
    public static State CurrentState{get=>currentSate;private set{//Si se desea obtener el estado se devuelve sin problemas sin embargo solo el juez puede cambiar el estado
        currentSate=value;
        Debug.Log("State changed to "+Judge.CurrentState);
        StateListener[] scripts=Resources.FindObjectsOfTypeAll<StateListener>();//Se buscan todos los StateListener
        foreach(StateListener script in scripts){script.CheckState();}//Se hace reaccionar a todos los StateListener ante el nuevo estado
        currentSate=State.WaitingPlayerAction;//Se espera la proxima accion del jugador
    }}
    private static int turnNumber;//Numero de turnos
    public static int GetTurnNumber{get=>turnNumber;}
    private static Player playerTurn;//Jugador en turno
    public static Player GetPlayer{get=>playerTurn;}//Devuelve el jugador en turno
    public static Player GetEnemy{get=>playerTurn==Player.P1? Player.P2 : Player.P1;}//Devuelve el enemigo del jugador en turno
    private static int turnActionsCount;//Cantidad de acciones realizadas en el turno
    public static int GetTurnActionsCount{get=>turnActionsCount;}
    private static bool isLastTurn;//Si es o no el ultimo turno antes de que acabe la ronda
    public static bool IsLastTurn{get=>isLastTurn;}
    public static bool CanPlay{get=>turnActionsCount==0 || isLastTurn;}
    private static float lastClickTime;
    void Start(){//En el inicio de la escena se cargan las cartas y se reinicia el juego
        CurrentState=State.LoadingCards;
        ResetGame();
    }
    public static void ResetGame(){//Reinicia el juego. Este metodo es llamado por un boton llamado ResetGameButton y al inicio del juego
        LeaderCard.ResetAllLeaderSkills();//Reinicia las habilidades de los lideres
        turnActionsCount=0;
        isLastTurn=false;
        turnNumber=1;
        playerTurn=Player.P1;//El Player 1 inicia la partida siempre
        CurrentState=State.SettingUpGame;
    }
    void Update(){
        ListenToSpaceBarPress();
    }
    private void ListenToSpaceBarPress(){//Acaba el turno cuando se presiona espacio, pero con una diferencia de tiempo de 0.3s entre pulsaciones
        if(Input.GetKeyDown(KeyCode.Space) && Time.time-lastClickTime>0.3f){
            EndTurn();
            lastClickTime=Time.time;
        }
    }
    public static void EndTurn(){//Se llama con cada pase
        if(isLastTurn){//Se entra cuando se acaba la ronda 
            NextRound();
            return;
        }
        if(turnActionsCount==0){//Detecta caundo un jugador pasa sin jugar
            SwitchTurn();
            isLastTurn=true;//Activa el modo ultimo turno, cuando se presione el pass de nuevo se acabara la ronda
        }else{
            SwitchTurn();
        }
        CurrentState=State.EndingTurn;
    }
    public static void PlayCard(GameObject card){//Juega la carta
        //Si la carta tiene efecto de carta especial, que se active
        card.GetComponent<ISpecialCard>()?.TriggerSpecialEffect();

        Execute.DoEffect(card,card.GetComponent<Card>().OnActivationName);//Se ejecuta el efecto en del OnActivation
        turnActionsCount++;//Se anade una accion de turno
        CurrentState=State.PlayingCard;
        card.GetComponent<Card>().LoadInfo();
    }
    private static void NextRound(){//Proxima ronda
        if(Field.P1ForceValue>Field.P2ForceValue){//Si P1 tiene mas poder que P2 entonces P1 comienza el proximo turno
            if(playerTurn==Player.P2){SwitchTurn();}//Cambiamos los turnos ya que P1 debe comenzar el proximo
            RoundPoints.AddPointToP1();//P1 gana la ronda y obtiene un punto de ronda
            UserRead.Write("P1 gano la ronda");
        }else if(Field.P2ForceValue>Field.P1ForceValue){//Si P2 tiene mas poder que P1 entonces P2 comienza el proximo turno
            if(playerTurn==Player.P1){SwitchTurn();}//Cambiamos los turnos ya que P2 debe comenzar el proximo
            RoundPoints.AddPointToP2();//P2 gana la ronda y obtiene un punto de ronda
            UserRead.Write("P2 gano la ronda");
        }else{//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
            SwitchTurn();
            RoundPoints.AddPointToP1();
            RoundPoints.AddPointToP2();
            UserRead.Write("Ha ocurrido un empate");
        }
        if(CheckGameWin()){//Si se gana el juego
            return;
        }
        isLastTurn=false;
        turnActionsCount=0;
        CurrentState=State.EndingRound;
    }
    private static void SwitchTurn(){//Se cambia de turno
        playerTurn=GetEnemy;//Es el turno del contrario
        if(playerTurn==Player.P1){turnNumber++;}
        turnActionsCount=0;
    }
    //Condicion de victoria
    public static bool CheckGameWin(){//Chequea quien ha ganado el juego
        if(RoundPoints.GetRPointsP1!=RoundPoints.GetRPointsP2){//Si la puntuacion es diferente (esto obliga a que el juego siga hasta que haya una ventaja)
            if(RoundPoints.GetRPointsP1>1){//El primero que llegue a 2 puntos de ronda gana
                WinsGame(Player.P1);return true;
            }else if(RoundPoints.GetRPointsP2>1){
                WinsGame(Player.P2);return true;
            }
        }else{
            UserRead.Write("El proximo jugador que gane una ronda gana el juego!!");
        }
        return false;
    }
    private static void WinsGame(Player player){//El jugador gana la partida
        playerTurn=player;//Se guarda en playerTurn el jugador ganador para que otros scripts puedan acceder al ganador del juego mediante GetPlayerTurn
        CurrentState=State.EndingGame;
    }
}