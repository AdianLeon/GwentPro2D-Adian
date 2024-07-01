using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para la logica de los turnos
public class TurnManager : MonoBehaviour
{
    public static int playerTurn;//Turno de jugador
    public static string PTurn{get=>"P"+playerTurn;}
    public static string ETurn{get{if(playerTurn==1){return "P2";}else{return "P1";}}}
    public static int cardsPlayed;//Cant de cartas jugadas en el turno
    public static bool lastTurn;//Si es o no el ultimo turno antes de que acabe la ronda
    public static List<GameObject> playedCards=new List<GameObject>();//Lista de las cartas jugadas para enviarlas al cementerio
    public static float lastClickTime;
    void Start(){
        RoundPoints.URLongWrite("Ha comenzado una nueva partida, es el turno de P1");
        lastClickTime=0;
        cardsPlayed=0;
        lastTurn=false;
        playedCards.Clear();
        playerTurn=1;//El Player 1 inicia la partida siempre
    }
    void Update(){
        if(Time.time-RoundPoints.secCounter<1){
            RoundPoints.URWrite(RoundPoints.message);
        }
        if(Input.GetKeyDown(KeyCode.Space) && Time.time-lastClickTime>0.5f){//Clickea el passbutton cuando se presiona espacio, pero con una diferencia de tiempo de 0.5s
            GameObject.Find("PassButton").GetComponent<Button>().onClick.Invoke();
            lastClickTime=Time.time;
        }
    }
    public static void EndTurn(){//Se llama con cada pass
        if(lastTurn){//Se entra cuando se acaba la ronda 
            NextRound();
        }else if(cardsPlayed==0){//Detecta caundo un jugador pasa sin jugar
            SwitchTurn();
            lastTurn=true;//Activa el modo ultimo turno, cuando se presione el pass de nuevo se acabara la ronda
            RoundPoints.URLongWrite("Turno de "+PTurn+", es el ultimo turno antes de que se acabe la ronda");
        }else{
            SwitchTurn();
        }
        VisualEffects.PlayedLightsToColor(new Color(0,1,0,0.2f));//Las luces se ponen verdes
    }
    public static void PlayCard(GameObject card){//Juega la carta
        card.GetComponent<Dragging>().IsDraggable=false;
        if(card.GetComponent<ICardEffect>()!=null){//Si la carta tiene efectos
            ICardEffect[] cardEffects=card.GetComponents<ICardEffect>();
            foreach(ICardEffect cardEffect in cardEffects){
                cardEffect.TriggerEffect();//Ejecuta esos scripts
            }
        }
        if(card.GetComponent<Card>().OnActivationCode!=""){//Si tiene efectos en OnActivation
            ProcessEffect.ExecuteEffect(card,card.GetComponent<Card>().OnActivationCode);//Se ejecutan
        }
        //Las cartas de aumento y despeje se envian al cementerio durante su efecto, no se pueden anadir a las listas de cartas jugadas
        if(card.transform.parent.gameObject!=GameObject.Find("GraveyardP1") && card.transform.parent.gameObject!=GameObject.Find("GraveyardP2")){
            TotalFieldForce.AddCard(card);//Anade la carta segun el campo y el tipo
            playedCards.Add(card);//Anade la carta a la lista de cartas jugadas
        }
        TotalFieldForce.UpdateForce();//Se actualiza la fuerza del campo
        card.GetComponent<Card>().LoadInfo();//Recarga la info de la carta
        CompleteTurn();
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
        leaderCard.GetComponent<LeaderCard>().usedSkill=true;
        CompleteTurn();
    }
    private static void CompleteTurn(){
        cardsPlayed++;
        DeckTrade.firstAction=false;
        WeatherEffect.UpdateWeather();//Actualiza el clima
        if(!lastTurn){//Si no es el ultimo turno antes de que acabe la ronda, no se puede jugar de nuevo
            VisualEffects.PlayedLightsToColor(new Color(1,0,0,0.2f));//Las luces en el campo se ponen rojas
        }
    }
    private static void NextRound(){//Proxima ronda
        lastTurn=false;
        Graveyard.AllToGraveyard();//Manda todas las cartas al cementerio
        playedCards.Clear();
        for(int i=1;i<3;i++){
            GameObject.Find("DeckP"+i).GetComponent<DrawCards>().OnClickDraw();
            GameObject.Find("DeckP"+i).GetComponent<DrawCards>().OnClickDraw();
        }
        if(TotalFieldForce.p1ForceValue>TotalFieldForce.p2ForceValue){//Si P1 tiene mas poder que P2
            if(playerTurn==2){//P1 comienza el proximo turno
                SwitchTurn();//En este caso solo hay que arreglar a quien le toca el turno porque el campo se cambia ya de por si
            }else{
                PassButtonScript.ClickPassButtonWithoutEndTurn();//En este caso hay que clickear el passbutton porque esto deshace el cambio de campo
            }
            PlayerCondition.rPointsP1++;//P1 gana la ronda y obtiene un punto de ronda
            RoundWinner(1);
        }else if(TotalFieldForce.p2ForceValue>TotalFieldForce.p1ForceValue){//Si P2 tiene mas poder que P1
            if(playerTurn==1){//P2 comienza el proximo turno
                SwitchTurn();//En este caso solo hay que arreglar a quien le toca el turno porque el campo se cambia ya de por si
            }else{
                PassButtonScript.ClickPassButtonWithoutEndTurn();//En este caso hay que clickear el passbutton porque esto deshace el cambio de campo
            }
            PlayerCondition.rPointsP2++;//P2 gana la ronda y obtiene un punto de ronda
            RoundWinner(2);
        }else{//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
            SwitchTurn();
            PlayerCondition.rPointsP1++;
            PlayerCondition.rPointsP2++;
            PlayerCondition.WinCheck();
        }
        cardsPlayed=0;
        TotalFieldForce.ResetPlayedCards();
        RoundPoints.UpdatePoints();
    }
    private static void RoundWinner(int playerNumber){
        RoundPoints.URLongWrite("Jugador "+playerNumber+" gano la ronda");
        PlayerCondition.WinCheck();
    }
    private static void SwitchTurn(){//Se cambia de turno
        DeckTrade.twice=0;
        DeckTrade.firstAction=true;//Siempre que comienza un nuevo turno se hace posible una primera accion
        if(playerTurn==1){
               playerTurn=2;
                cardsPlayed=0;
        }else{
            playerTurn=1;
            cardsPlayed=0;
            DeckTrade.firstTurn=false;//Esto es para desactivar el uso del intercambio de cartas con el mazo al inicio del juego
        }
        RoundPoints.URLongWrite("Turno de P"+playerTurn);
    }
}