using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para la logica de los turnos
public class TurnManager : MonoBehaviour
{
    private static int playerTurn;//Turno de jugador
    public static string GetPlayerTurn{get=>"P"+playerTurn;}
    public static string GetEnemyTurn{get{if(playerTurn==1){return "P2";}else{return "P1";}}}
    private static int turnActionsCount;//Cant de acciones realizadas en el turno
    public static int GetTurnActionsCount{get=>turnActionsCount;}
    private static bool isLastTurn;//Si es o no el ultimo turno antes de que acabe la ronda
    public static bool IsLastTurn{get=>isLastTurn;}
    public static bool CanPlay{get=>turnActionsCount==0 || isLastTurn;}
    public static List<GameObject> PlayedCards=new List<GameObject>();//Lista de las cartas jugadas en la ronda
    private static float lastClickTime;
    void Start(){
        RoundPoints.LongWriteUserRead("Ha comenzado una nueva partida, es el turno de P1");
        lastClickTime=0;
        turnActionsCount=0;
        isLastTurn=false;
        PlayedCards.Clear();
        playerTurn=1;//El Player 1 inicia la partida siempre
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Space) && Time.time-lastClickTime>0.3f){//Clickea el passbutton cuando se presiona espacio, pero con una diferencia de tiempo de 0.3s
            GameObject.Find("PassButton").GetComponent<Button>().onClick.Invoke();
            lastClickTime=Time.time;
        }
    }
    public static void EndTurn(){//Se llama con cada pass
        Debug.Log("Ending turn");
        Debug.Log(turnActionsCount);
        Debug.Log(isLastTurn);
        if(isLastTurn){//Se entra cuando se acaba la ronda 
            NextRound();
        }else if(turnActionsCount==0){//Detecta caundo un jugador pasa sin jugar
            SwitchTurn();
            isLastTurn=true;//Activa el modo ultimo turno, cuando se presione el pass de nuevo se acabara la ronda
            RoundPoints.LongWriteUserRead("Turno de "+GetPlayerTurn+", es el ultimo turno antes de que se acabe la ronda");
        }else{
            SwitchTurn();
        }
        VisualEffects.SetPlayedLights=true;
    }
    public static void PlayCard(GameObject card){//Juega la carta
        card.GetComponent<Dragging>().IsDraggable=false;
        if(card.GetComponent<ICardEffect>()!=null){//Si la carta tiene efectos
            ICardEffect[] cardEffects=card.GetComponents<ICardEffect>();
            foreach(ICardEffect cardEffect in cardEffects){
                Debug.Log("TurnManager.Play Card: "+card.transform.parent.name);
                cardEffect.TriggerEffect();//Ejecuta esos scripts
            }
        }
        if(card.GetComponent<Card>().OnActivationCode!=""){//Si tiene efectos en OnActivation
            ProcessEffect.ExecuteEffect(card,card.GetComponent<Card>().OnActivationCode);//Se ejecutan
        }
        //Las cartas de aumento y despeje se envian al cementerio durante su efecto, no se pueden anadir a las listas de cartas jugadas
        if(card.transform.parent.gameObject!=GameObject.Find("GraveyardP1") && card.transform.parent.gameObject!=GameObject.Find("GraveyardP2")){
            TotalFieldForce.AddCard(card);//Anade la carta segun el campo y el tipo
            PlayedCards.Add(card);//Anade la carta a la lista de cartas jugadas
        }
        CompleteTurn();
        card.GetComponent<Card>().LoadInfo();//Recarga la info de la carta
    }
    public static void PlayLeaderCard(GameObject leaderCard){//Juega la carta lider
        if(leaderCard.gameObject.GetComponent<ILeaderEffect>()!=null){//Si tiene efectos en scripts
            ILeaderEffect[] leaderEffects=leaderCard.GetComponents<ILeaderEffect>();
            foreach(ILeaderEffect leaderEffect in leaderEffects){
                leaderEffect.TriggerEffect();//Ejecuta esos scripts
            }
        }
        if(leaderCard.gameObject.GetComponent<Card>().OnActivationCode!=""){//Si tiene efectos en OnActivation
            ProcessEffect.ExecuteEffect(leaderCard,leaderCard.GetComponent<LeaderCard>().OnActivationCode);//Se ejecutan
        }
        leaderCard.GetComponent<LeaderCard>().UsedSkill=true;
        CompleteTurn();
    }
    private static void CompleteTurn(){
        Debug.Log("Completing Turn");
        turnActionsCount++;
        DeckTrade.SetFirstAction=false;
        TotalFieldForce.UpdateForce();//Se actualiza la fuerza del campo
        WeatherCard.UpdateWeather();//Actualiza el clima
        if(!isLastTurn){//Si no es el ultimo turno antes de que acabe la ronda, no se puede jugar de nuevo
            VisualEffects.SetPlayedLights=false;//Las luces en el campo se ponen rojas
        }
    }
    private static void NextRound(){//Proxima ronda
        isLastTurn=false;
        Graveyard.AllToGraveyard();//Manda todas las cartas al cementerio
        PlayedCards.Clear();
        for(int i=1;i<3;i++){
            GameObject.Find("DeckP"+i).GetComponent<Deck>().DrawTopCard();
            GameObject.Find("DeckP"+i).GetComponent<Deck>().DrawTopCard();
        }
        if(TotalFieldForce.GetP1ForceValue>TotalFieldForce.GetP2ForceValue){//Si P1 tiene mas poder que P2
            if(playerTurn==2){//P1 comienza el proximo turno
                SwitchTurn();//En este caso solo hay que arreglar a quien le toca el turno porque el campo se cambia ya de por si
            }else{
                PassButtonScript.ClickPassButtonWithoutEndTurn();//En este caso hay que clickear el passbutton porque esto deshace el cambio de campo
            }
            PlayerCondition.AddPointToP1();//P1 gana la ronda y obtiene un punto de ronda
            WinsRound(1);
        }else if(TotalFieldForce.GetP2ForceValue>TotalFieldForce.GetP1ForceValue){//Si P2 tiene mas poder que P1
            if(playerTurn==1){//P2 comienza el proximo turno
                SwitchTurn();//En este caso solo hay que arreglar a quien le toca el turno porque el campo se cambia ya de por si
            }else{
                PassButtonScript.ClickPassButtonWithoutEndTurn();//En este caso hay que clickear el passbutton porque esto deshace el cambio de campo
            }
            PlayerCondition.AddPointToP2();//P2 gana la ronda y obtiene un punto de ronda
            WinsRound(2);
        }else{//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
            SwitchTurn();
            PlayerCondition.AddPointToP1();
            PlayerCondition.AddPointToP2();
            PlayerCondition.WinCheck();
        }
        turnActionsCount=0;
        TotalFieldForce.ResetPlayedCards();
        RoundPoints.UpdatePoints();
    }
    private static void WinsRound(int playerNumber){
        RoundPoints.LongWriteUserRead("Jugador "+playerNumber+" gano la ronda");
        PlayerCondition.WinCheck();
    }
    private static void SwitchTurn(){//Se cambia de turno
        DeckTrade.ResetTradeCount();
        DeckTrade.SetFirstAction=true;//Siempre que comienza un nuevo turno se hace posible una primera accion
        if(playerTurn==1){
            playerTurn=2;
        }else{
            playerTurn=1;
            DeckTrade.SetFirstTurn=false;//Desactivar el uso del intercambio de cartas con el mazo al inicio del juego
        }
        turnActionsCount=0;
        RoundPoints.LongWriteUserRead("Turno de P"+playerTurn);
    }
}