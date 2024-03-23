using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static int PlayerTurn;//Turno de jugador
    public static int CardsPlayed;//Cant de cartas jugadas en el turno
    static int countPass;//Conteo de pases sin cartas jugadas
    public static List<GameObject> PlayedCards=new List<GameObject>();//Lista de las cartas jugadas

    void Start()
    {
        countPass=0;
        PlayerTurn=1;
        CardsPlayed=0;
    }

    public static void EndTurn(){//Se llama con cada pass
        if(CardsPlayed==0){//Cuenta cada vez que se acaba el turno sin jugar cartas
            countPass++;
        }
        if(countPass>1){//A las dos veces que se acaba el turno sin jugar cartas se avanza de ronda
            NextRound();
            for(int i=0;i<2;i++){
                GameObject.Find("Deck").GetComponent<Button>().onClick.Invoke();
                GameObject.Find("EnemyDeck").GetComponent<Button>().onClick.Invoke();
            }
        }else{
            SwitchTurn();
        }
Debug.Log("Turno de P"+PlayerTurn);
    }
    public static void PlayCard(GameObject card){//Juega la carta y anade la carta a la lista de cartas jugadas
        countPass=0;//Ademas reinicia el conteo de veces que se ha dado pase sin jugar una carta
        CardsPlayed++;
        PlayedCards.Add(card);
        card.GetComponent<Card>().isPlayed=true;

        Effects.CheckForEffect(card);
    }

    public static void NextRound(){//Proxima ronda
        Graveyard.AllToGraveyard();//Manda todas las cartas al cementerio
        
        if(TotalFieldForce.P1ForceValue>TotalFieldForce.P2ForceValue){//Si P1 tiene mas poder que P2
            if(PlayerTurn==2){//P1 comienza el proximo turno
                SwitchTurn();//En este caso solo hay que arreglar a quien le toca el turno porque el campo se cambia ya de por si
            }else{
                ClickPassB();//En este caso hay que clickear el passbutton porque esto deshace el cambio de campo
            }

            PlayerCondition.Wins("P1");//P1 gana la ronda y obtiene un punto de ronda
        }
        else if(TotalFieldForce.P2ForceValue>TotalFieldForce.P1ForceValue){//Si P2 tiene mas poder que P1
Debug.Log("P2>P1");
            if(PlayerTurn==1){//P2 comienza el proximo turno
                SwitchTurn();//En este caso solo hay que arreglar a quien le toca el turno porque el campo se cambia ya de por si
            }else{
                ClickPassB();//En este caso hay que clickear el passbutton porque esto deshace el cambio de campo
            }

            PlayerCondition.Wins("P2");//P2 gana la ronda y obtiene un punto de ronda
        }
        else{//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
Debug.Log("P1=P2");
            SwitchTurn();
            PlayerCondition.rPointsP1++;
            PlayerCondition.rPointsP2++;
        }
        CardsPlayed=0;
        countPass=0;
        TotalFieldForce.P1ForceValue=0;
        TotalFieldForce.P2ForceValue=0;
    }
    public static void ClickPassB(){//Clickea una copia de PassButton pero sin la funcion EndTurn
    //Cuando un jugador que deberia comenzar la ronda gana en su turno el boton normalmente hace que el que deba jugar sea el otro
    //Es por esto que existe esta funcion, la idea es que se pulse doble y asi le toca jugar al que gano la ronda
        if(GameObject.Find("PassButtonWithoutEndTurn")!=null)
            GameObject.Find("PassButtonWithoutEndTurn").GetComponent<Button>().onClick.Invoke();
    }
    public static void SwitchTurn(){//Se cambia de turno
        if(PlayerTurn==1){
               PlayerTurn=2;
                CardsPlayed=0;
        }else{
            PlayerTurn=1;
            CardsPlayed=0;
        }
    }
}
