using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void EndTurn(){//Se llama con cada pass
        if(CardsPlayed==0){//Cuenta cada vez que se acaba el turno sin jugar cartas
            countPass++;
        }
        if(countPass>1){//A las dos veces que se acaba el turno sin jugar cartas se avanza de ronda
            NextRound();
        }else{
            if(PlayerTurn==1){
               PlayerTurn=2;
                CardsPlayed=0;
            }else{
                PlayerTurn=1;
                CardsPlayed=0;
            }
        }
    }
    public static void PlayCard(GameObject card){//Juega la carta y anade la carta a la lista de cartas jugadas
            CardsPlayed++;
            PlayedCards.Add(card);
            card.GetComponent<Card>().isPlayed=true;
    }
    public static void NextRound(){//Proxima ronda
        Graveyard.AllToGraveyard();//Manda todas las cartas al cementerio
        CardsPlayed=0;
        countPass=0;
        if(TotalFieldForce.P1ForceValue>TotalFieldForce.P2ForceValue){//Si P1 tiene mas poder que P2, P1 gana un punto
            PlayerTurn=1;
            PlayerCondition.Wins("P1");
        }else if(TotalFieldForce.P2ForceValue>TotalFieldForce.P1ForceValue){//Si P2 tiene mas poder que P1, P2 gana un punto
            PlayerTurn=2;
            PlayerCondition.Wins("P2");
        }else{//Si ambos tienen igual poder ambos ganan 1 punto
            PlayerCondition.Wins("P1");
            PlayerCondition.Wins("P2");
        }
        TotalFieldForce.P1ForceValue=0;
        TotalFieldForce.P2ForceValue=0;
    }
}
