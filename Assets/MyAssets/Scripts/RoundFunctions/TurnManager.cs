using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para la logica de los turnos
public class TurnManager : MonoBehaviour
{
    public static int playerTurn;//Turno de jugador
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
            RoundPoints.URLongWrite("Turno de P"+playerTurn+", es el ultimo turno antes de que se acabe la ronda");
        }else{
            SwitchTurn();
        }
        VisualEffects.PlayedLightsOn();//Las luces se ponen verdes para el proximo jugador
    }
    public static void PlayCard(GameObject card){//Juega la carta y anade la carta a la lista de cartas jugadas
        card.GetComponent<Dragging>().isDraggable=false;
        DeckTrade.firstAction=false;
        cardsPlayed++;
        if(card.GetComponent<CardEffect>()!=null){//Si la carta tiene efecto
            card.GetComponent<CardEffect>().TriggerEffect();//Activa el efecto
        }
        TotalFieldForce.AddCard(card);//Anade la carta segun el campo y el tipo
        playedCards.Add(card);//Anade la carta a la lista de cartas jugadas
        WeatherEffect.UpdateWeather();//Actualiza el clima
        card.GetComponent<Card>().LoadInfo();//Recarga la info de la carta
        if(!lastTurn){//Si no es el ultimo turno antes de que acabe la ronda, no se puede jugar de nuevo
            VisualEffects.PlayedLightsOff();//Las luces en el campo se ponen rojas
        }
    }
    private static void NextRound(){//Proxima ronda
        lastTurn=false;
        Graveyard.AllToGraveyard();//Manda todas las cartas al cementerio
        playedCards.Clear();
        for(int i=0;i<2;i++){
            GameObject.Find("Deck").GetComponent<Button>().onClick.Invoke();
            GameObject.Find("EnemyDeck").GetComponent<Button>().onClick.Invoke();
        }
        if(TotalFieldForce.P1ForceValue>TotalFieldForce.P2ForceValue){//Si P1 tiene mas poder que P2
            if(playerTurn==2){//P1 comienza el proximo turno
                SwitchTurn();//En este caso solo hay que arreglar a quien le toca el turno porque el campo se cambia ya de por si
            }else{
                PassButtonScript.ClickPassButtonWithoutEndTurn();//En este caso hay que clickear el passbutton porque esto deshace el cambio de campo
            }
            PlayerCondition.rPointsP1++;//P1 gana la ronda y obtiene un punto de ronda
            RoundWinner(1);
        }else if(TotalFieldForce.P2ForceValue>TotalFieldForce.P1ForceValue){//Si P2 tiene mas poder que P1
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