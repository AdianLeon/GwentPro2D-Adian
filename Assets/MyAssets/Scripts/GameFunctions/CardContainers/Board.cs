using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para la logica de los turnos
public class Board : MonoBehaviour, IContainer
{
    public List<GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}//Lista de las cartas jugadas en la ronda
    public static List<GameObject> PlayedCards{get=>GameObject.Find("Board").GetComponent<Board>().GetCards;}//Lista de las cartas jugadas en la ronda
    private static int turnNumber;
    public static int GetTurnNumber{get=>turnNumber;}
    private static int playerTurn;//Turno de jugador
    public static string GetPlayer{get=>"P"+playerTurn;}
    public static string GetEnemy{get{if(playerTurn==1){return "P2";}else{return "P1";}}}
    private static int turnActionsCount;//Cant de acciones realizadas en el turno
    public static int GetTurnActionsCount{get=>turnActionsCount;}
    private static bool isLastTurn;//Si es o no el ultimo turno antes de que acabe la ronda
    public static bool IsLastTurn{get=>isLastTurn;}
    public static bool CanPlay{get=>turnActionsCount==0 || isLastTurn;}
    private static float lastClickTime;
    void Start(){
        RoundPoints.WriteUserRead("Ha comenzado una nueva partida, es el turno de P1");
        lastClickTime=0;
        turnActionsCount=0;
        isLastTurn=false;
        turnNumber=1;
        playerTurn=1;//El Player 1 inicia la partida siempre
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Space) && Time.time-lastClickTime>0.3f){//Clickea el passbutton cuando se presiona espacio, pero con una diferencia de tiempo de 0.3s
            EndTurn();
            lastClickTime=Time.time;
        }
    }
    public static void EndTurn(){//Se llama con cada pass
        if(isLastTurn){//Se entra cuando se acaba la ronda 
            NextRound();
        }else if(turnActionsCount==0){//Detecta caundo un jugador pasa sin jugar
            SwitchTurn();
            isLastTurn=true;//Activa el modo ultimo turno, cuando se presione el pass de nuevo se acabara la ronda
            RoundPoints.LongWriteUserRead("Turno de "+GetPlayer+", es el ultimo turno antes de que se acabe la ronda");
        }else{
            SwitchTurn();
        }
        VisualEffects.PlayedLights=true;
    }
    public static void PlayCard(GameObject card){//Juega la carta
        if(card.GetComponent<ISpecialCard>()!=null){//Si la carta tiene efectos de carta especial
            ISpecialCard[] cardEffects=card.GetComponents<ISpecialCard>();
            foreach(ISpecialCard cardEffect in cardEffects){
                cardEffect.TriggerSpecialEffect();//Ejecuta esos scripts
            }
        }
        if(card.GetComponent<Card>().OnActivationName!=""){//Si tiene OnActivation
            Execute.DoEffect(card,card.GetComponent<Card>().OnActivationName);//Se ejecutan
        }
        card.GetComponent<Card>().LoadInfo();//Recarga la info de la carta
        CompleteTurn();
    }
    public static void PlayLeaderCard(GameObject leaderCard){//Juega la carta lider
        if(leaderCard.gameObject.GetComponent<Card>().OnActivationName!=""){//Si tiene el nombre de algun efecto en OnActivation
            Execute.DoEffect(leaderCard,leaderCard.GetComponent<LeaderCard>().OnActivationName);//Se ejecuta
        }
        leaderCard.GetComponent<LeaderCard>().UsedSkill=true;
        CompleteTurn();
    }
    private static void CompleteTurn(){
        turnActionsCount++;
        Field.UpdateAllForces();//Se actualiza la fuerza del campo
        WeatherCard.UpdateWeather();//Actualiza el clima
        if(!isLastTurn){//Si no es el ultimo turno antes de que acabe la ronda, no se puede jugar de nuevo
            VisualEffects.PlayedLights=false;//Las luces en el campo se ponen rojas
        }
    }
    private static void NextRound(){//Proxima ronda
        isLastTurn=false;
        Graveyard.SendToGraveyard(PlayedCards);//Manda todas las cartas al cementerio
        for(int i=1;i<3;i++){
            GameObject.Find("DeckP"+i).GetComponent<Deck>().DrawTopCard();
            GameObject.Find("DeckP"+i).GetComponent<Deck>().DrawTopCard();
        }
        if(Field.P1ForceValue>Field.P2ForceValue){//Si P1 tiene mas poder que P2 entonces P1 comienza el proximo turno
            if(playerTurn==2){SwitchTurn();}//Cambiamos los turnos ya que P1 debe comenzar el proximo
            RoundPoints.AddPointToP1();//P1 gana la ronda y obtiene un punto de ronda
            WinsRound(1);
        }else if(Field.P2ForceValue>Field.P1ForceValue){//Si P2 tiene mas poder que P1 entonces P2 comienza el proximo turno
            if(playerTurn==1){SwitchTurn();}//Cambiamos los turnos ya que P2 debe comenzar el proximo
            RoundPoints.AddPointToP2();//P2 gana la ronda y obtiene un punto de ronda
            WinsRound(2);
        }else{//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
            SwitchTurn();
            RoundPoints.AddPointToP1();
            RoundPoints.AddPointToP2();
            RoundPoints.WinCheck();
        }
        turnActionsCount=0;
        RoundPoints.UpdatePoints();
    }
    private static void WinsRound(int playerNumber){
        RoundPoints.LongWriteUserRead("P"+playerNumber+" gano la ronda");
        RoundPoints.WinCheck();
    }
    private static void SwitchTurn(){//Se cambia de turno
        playerTurn++;
        if(playerTurn>2){
            playerTurn=1;
            turnNumber++;//Es un nuevo turno
        }
        turnActionsCount=0;
        RoundPoints.WriteUserRead("Turno de P"+playerTurn);
        HandCover.UpdateCovers();//Se actualizan los covers de las manos de los jugadores
    }
}