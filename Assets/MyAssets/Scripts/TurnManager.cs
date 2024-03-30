using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static int PlayerTurn;//Turno de jugador
    public static int CardsPlayed;//Cant de cartas jugadas en el turno
    public static bool lastTurn;//Si es o no el ultimo turno antes de que acabe la ronda
    public static List<GameObject> PlayedCards=new List<GameObject>();//Lista de las cartas jugadas
    public static float lastClickTime=0;

    void Start(){
        PlayerTurn=1;//El Player 1 inicia la partida siempre
        RoundPoints.URWrite("Turno de P1");
    }
    void Update(){
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1)) && Time.time-lastClickTime>0.5f){
            GameObject.Find("PassButton").GetComponent<Button>().onClick.Invoke();
            lastClickTime=Time.time;
        }
    }

    public static void EndTurn(){//Se llama con cada pass
        if(lastTurn){//Se entra cuando se acaba la ronda 
            NextRound();
        }else if(CardsPlayed==0){//Detecta caundo un jugador pasa sin jugar
            SwitchTurn();
            lastTurn=true;//Activa el modo ultimo turno, cuando se presione el pass de nuevo se acabara la ronda
        }else{
            SwitchTurn();
        }
    RoundPoints.URWrite("Turno de P"+PlayerTurn);
    }
    public static void PlayCard(GameObject card){//Juega la carta y anade la carta a la lista de cartas jugadas
        card.GetComponent<Dragging>().isDraggable=false;
        ExtraDrawCard.firstAction=false;
        CardsPlayed++;
        PlayedCards.Add(card);
        card.GetComponent<Card>().isPlayed=true;

        Effects.CheckForEffect(card);
        Effects.UpdateClima();
        RoundPoints.URWrite("Presiona espacio para pasar el turno");
    }

    public static void NextRound(){//Proxima ronda
        lastTurn=false;
        Graveyard.AllToGraveyard();//Manda todas las cartas al cementerio

        for(int i=0;i<2;i++){
            GameObject.Find("Deck").GetComponent<Button>().onClick.Invoke();
            GameObject.Find("EnemyDeck").GetComponent<Button>().onClick.Invoke();
        }
        
        if(TotalFieldForce.P1ForceValue>TotalFieldForce.P2ForceValue){//Si P1 tiene mas poder que P2
            if(PlayerTurn==2){//P1 comienza el proximo turno
                SwitchTurn();//En este caso solo hay que arreglar a quien le toca el turno porque el campo se cambia ya de por si
            }else{
                ClickPassB();//En este caso hay que clickear el passbutton porque esto deshace el cambio de campo
            }

            PlayerCondition.Wins("P1");//P1 gana la ronda y obtiene un punto de ronda
        }
        else if(TotalFieldForce.P2ForceValue>TotalFieldForce.P1ForceValue){//Si P2 tiene mas poder que P1
            if(PlayerTurn==1){//P2 comienza el proximo turno
                SwitchTurn();//En este caso solo hay que arreglar a quien le toca el turno porque el campo se cambia ya de por si
            }else{
                ClickPassB();//En este caso hay que clickear el passbutton porque esto deshace el cambio de campo
            }

            PlayerCondition.Wins("P2");//P2 gana la ronda y obtiene un punto de ronda
        }
        else{//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
            SwitchTurn();
            PlayerCondition.rPointsP1++;
            PlayerCondition.rPointsP2++;
        }
        CardsPlayed=0;
        TotalFieldForce.Empty();
        RoundPoints.UpdatePoints();
    }
    public static void ClickPassB(){//Clickea una copia de PassButton pero sin la funcion EndTurn
    //Cuando un jugador que deberia comenzar la ronda gana en su turno el boton normalmente hace que el que deba jugar sea el otro
    //Es por esto que existe esta funcion, la idea es que se pulse doble y asi le toca jugar al que gano la ronda
        if(GameObject.Find("PassButtonWithoutEndTurn")!=null)
            GameObject.Find("PassButtonWithoutEndTurn").GetComponent<Button>().onClick.Invoke();
    }
    public static void SwitchTurn(){//Se cambia de turno
        ExtraDrawCard.twice=0;
        ExtraDrawCard.firstAction=true;//Siempre que comienza un nuevo turno se hace posible una primera accion
        if(PlayerTurn==1){
               PlayerTurn=2;
                CardsPlayed=0;
        }else{
            PlayerTurn=1;
            CardsPlayed=0;
            ExtraDrawCard.firstTurn=false;//Esto es para desactivar el uso del intercambio de cartas con el mazo al inicio del juego
        }
    }
}
