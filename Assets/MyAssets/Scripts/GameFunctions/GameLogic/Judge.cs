using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
//Script que simula un juez, se encarga de la logica de turnos, rondas y condicion de victoria
public class Judge : MonoBehaviour
{
    private static int turnNumber;//Numero de turnos
    public static int GetTurnNumber{get=>turnNumber;}
    private static Player playerTurn;//Turno de jugador
    public static Player GetPlayer{get=>playerTurn;}
    public static Player GetEnemy{get=>playerTurn==Player.P1? Player.P2 : Player.P1;}
    private static int turnActionsCount;//Cant de acciones realizadas en el turno
    public static int GetTurnActionsCount{get=>turnActionsCount;}
    private static bool isLastTurn;//Si es o no el ultimo turno antes de que acabe la ronda
    public static bool IsLastTurn{get=>isLastTurn;}
    public static bool CanPlay{get=>turnActionsCount==0 || isLastTurn;}
    private static float lastClickTime;
    void Start(){
        ResetGame();
    }
    void Update(){
        ListenToSpaceBarPress();
    }
    public static void ResetGame(){//Reinicia el juego. Este metodo es llamado por un boton llamado ResetGameButton
        LeaderCard.ResetAllLeaderSkills();
        DeckTrade.ResetTradeCount();
        turnActionsCount=0;
        isLastTurn=false;
        turnNumber=1;
        playerTurn=Player.P1;//El Player 1 inicia la partida siempre
        GFUtils.AllInitialize();
    }
    private void ListenToSpaceBarPress(){
        if(Input.GetKeyDown(KeyCode.Space) && Time.time-lastClickTime>0.3f){//Clickea el passbutton cuando se presiona espacio, pero con una diferencia de tiempo de 0.3s
            EndTurn();
            lastClickTime=Time.time;
        }
    }
    public static void EndTurn(){//Se llama con cada pase
        if(isLastTurn){//Se entra cuando se acaba la ronda 
            NextRound();
        }else if(turnActionsCount==0){//Detecta caundo un jugador pasa sin jugar
            SwitchTurn();
            isLastTurn=true;//Activa el modo ultimo turno, cuando se presione el pass de nuevo se acabara la ronda
            GFUtils.UserRead.LongWrite("Turno de "+GetPlayer+", es el ultimo turno antes de que se acabe la ronda");
        }else{
            SwitchTurn();
        }
        GFUtils.CallNextUpdate();
    }
    public static void PlayCard(GameObject card){//Juega la carta
        //Si la carta tiene efecto de carta especial, que se active
        card.GetComponent<ISpecialCard>()?.TriggerSpecialEffect();
        Execute.DoEffect(card,card.GetComponent<Card>().OnActivationName);//Se ejecuta el efecto
        turnActionsCount++;
        GFUtils.CallNextUpdate();
        card.GetComponent<Card>().LoadInfo();
    }
    private static void NextRound(){//Proxima ronda
        if(Field.P1ForceValue>Field.P2ForceValue){//Si P1 tiene mas poder que P2 entonces P1 comienza el proximo turno
            if(playerTurn==Player.P2){SwitchTurn();}//Cambiamos los turnos ya que P1 debe comenzar el proximo
            RoundPoints.AddPointToP1();//P1 gana la ronda y obtiene un punto de ronda
            WinsRound(Player.P1);
        }else if(Field.P2ForceValue>Field.P1ForceValue){//Si P2 tiene mas poder que P1 entonces P2 comienza el proximo turno
            if(playerTurn==Player.P1){SwitchTurn();}//Cambiamos los turnos ya que P2 debe comenzar el proximo
            RoundPoints.AddPointToP2();//P2 gana la ronda y obtiene un punto de ronda
            WinsRound(Player.P2);
        }else{//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
            SwitchTurn();
            RoundPoints.AddPointToP1();
            RoundPoints.AddPointToP2();
            GFUtils.UserRead.LongWrite("Ha ocurrido un empate");
            WinCheck();
        }

        isLastTurn=false;
        Graveyard.SendToGraveyard(Field.AllPlayedCards);//Manda todas las cartas al cementerio
        for(int i=1;i<3;i++){//Reparte dos cartas a los jugadores
            GameObject.Find("DeckP"+i).GetComponent<Deck>().DrawTopCard();
            GameObject.Find("DeckP"+i).GetComponent<Deck>().DrawTopCard();
        }

        GFUtils.CallNextUpdate();
        turnActionsCount=0;
    }
    private static void SwitchTurn(){//Se cambia de turno
        playerTurn=GetEnemy;
        if(playerTurn==Player.P1){turnNumber++;}
        turnActionsCount =0;
        GFUtils.UserRead.Write("Turno de "+playerTurn);
    }
    private static void WinsRound(Player player){
        GFUtils.UserRead.LongWrite(player+" gano la ronda");
        WinCheck();
    }
    //Condicion de victoria
    public static void WinCheck(){//Chequea quien ha ganado el juego
        if(RoundPoints.GetRPointsP1!=RoundPoints.GetRPointsP2){//Si la puntuacion es diferente (esto obliga a que el juego siga hasta que haya una ventaja)
            if(RoundPoints.GetRPointsP1>1){//El primero que llegue a 2 puntos de ronda gana
                WinsGame(Player.P1);
            }else if(RoundPoints.GetRPointsP2>1){
                WinsGame(Player.P2);
            }
        }else{
            GFUtils.UserRead.LongWrite("El proximo jugador que gane una ronda gana el juego!!");
        }
    }
    private static void WinsGame(Player player){//El jugador gana la partida
        playerTurn=player;
        GFUtils.AllFinish();
    }
}