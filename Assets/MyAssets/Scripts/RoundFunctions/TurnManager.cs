using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para la logica de los turnos
public class TurnManager : MonoBehaviour
{
    public static int PlayerTurn;//Turno de jugador
    public static int CardsPlayed;//Cant de cartas jugadas en el turno
    public static bool lastTurn;//Si es o no el ultimo turno antes de que acabe la ronda
    public static List<GameObject> PlayedCards=new List<GameObject>();//Lista de las cartas jugadas para enviarlas al cementerio
    public static float lastClickTime;

    void Start(){
        lastClickTime=0;
        CardsPlayed=0;
        lastTurn=false;
        PlayedCards.Clear();
        PlayerTurn=1;//El Player 1 inicia la partida siempre
        RoundPoints.URWrite("Ha comenzado una nueva partida, es el turno de P1");
    }
    void Update(){
        if((Input.GetMouseButtonDown(0) && DropZone.pointerInZone) && Time.time-DropZone.lastClickTime>0.5f){//Cuando se presiona el boton del mouse y el puntero esta dentro de una dropzone
            RoundPoints.URWrite("Esta zona es de tipo "+DropZone.zoneEntered.GetComponent<DropZone>().validZone);//Se muestra su nombre en el UserRead
            DropZone.lastClickTime=Time.time;
        }
        if(Input.GetKeyDown(KeyCode.Space) && Time.time-lastClickTime>0.5f){//Clickea el passbutton cuando se presiona espacio, pero con una diferencia de tiempo de 0.5s
            GameObject.Find("PassButton").GetComponent<Button>().onClick.Invoke();
            lastClickTime=Time.time;
        }
        if(CardsPlayed==0 || lastTurn){//Pone las luces del campo verdes cuando se puede jugar una carta o rojas cuando se tiene que pasar 
            VisualEffects.PlayedLightsOn();
        }else{
            VisualEffects.PlayedLightsOff();
        }
    }

    public static void EndTurn(){//Se llama con cada pass
        if(lastTurn){//Se entra cuando se acaba la ronda 
            NextRound();
        }else if(CardsPlayed==0){//Detecta caundo un jugador pasa sin jugar
            SwitchTurn();
            lastTurn=true;//Activa el modo ultimo turno, cuando se presione el pass de nuevo se acabara la ronda
            RoundPoints.URWrite("Turno de P"+PlayerTurn+", es el ultimo turno antes de que se acabe la ronda");
        }else{
            SwitchTurn();
        }
    }
    public static void PlayCard(GameObject card){//Juega la carta y anade la carta a la lista de cartas jugadas
        card.GetComponent<Dragging>().isDraggable=false;
        DeckTrade.firstAction=false;
        CardsPlayed++;
        if(card.GetComponent<CardEffect>()!=null){//Si la carta tiene efecto
            card.GetComponent<CardEffect>().TriggerEffect();//Activa el efecto
        }
        TotalFieldForce.AddCard(card);//Anade la carta segun el campo y el tipo
        PlayedCards.Add(card);//Anade la carta a la lista de cartas jugadas
        WeatherEffect.UpdateWeather();
        if(!lastTurn)
            RoundPoints.URWrite("Presiona espacio para pasar el turno");
    }

    public static void NextRound(){//Proxima ronda
        lastTurn=false;
        Graveyard.AllToGraveyard();//Manda todas las cartas al cementerio
        PlayedCards.Clear();
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

            PlayerCondition.rPointsP1++;//P1 gana la ronda y obtiene un punto de ronda
            RoundPoints.URWrite("Jugador 1 gano la ronda");
            PlayerCondition.WinCheck();
        }
        else if(TotalFieldForce.P2ForceValue>TotalFieldForce.P1ForceValue){//Si P2 tiene mas poder que P1
            if(PlayerTurn==1){//P2 comienza el proximo turno
                SwitchTurn();//En este caso solo hay que arreglar a quien le toca el turno porque el campo se cambia ya de por si
            }else{
                ClickPassB();//En este caso hay que clickear el passbutton porque esto deshace el cambio de campo
            }

            PlayerCondition.rPointsP2++;//P2 gana la ronda y obtiene un punto de ronda
            RoundPoints.URWrite("Jugador 2 gano la ronda");
            PlayerCondition.WinCheck();
        }
        else{//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
            SwitchTurn();
            PlayerCondition.rPointsP1++;
            PlayerCondition.rPointsP2++;
            PlayerCondition.WinCheck();
        }
        CardsPlayed=0;
        TotalFieldForce.ResetPlayedCards();
        RoundPoints.UpdatePoints();
    }
    public static void ClickPassB(){//Clickea una copia de PassButton pero sin la funcion EndTurn
    //Cuando un jugador que deberia comenzar la ronda gana en su turno el boton normalmente hace que el que deba jugar sea el otro
    //Es por esto que existe esta funcion, la idea es que se pulse doble y asi le toca jugar al que gano la ronda
        if(GameObject.Find("PassButtonWithoutEndTurn")!=null)
            GameObject.Find("PassButtonWithoutEndTurn").GetComponent<Button>().onClick.Invoke();
    }
    public static void SwitchTurn(){//Se cambia de turno
        DeckTrade.twice=0;
        DeckTrade.firstAction=true;//Siempre que comienza un nuevo turno se hace posible una primera accion
        if(PlayerTurn==1){
               PlayerTurn=2;
                CardsPlayed=0;
        }else{
            PlayerTurn=1;
            CardsPlayed=0;
            DeckTrade.firstTurn=false;//Esto es para desactivar el uso del intercambio de cartas con el mazo al inicio del juego
        }
        RoundPoints.URWrite("Turno de P"+PlayerTurn);
    }
}
