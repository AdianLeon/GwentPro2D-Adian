using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static int PlayerTurn;
    public static int CardsPlayed;
    static int countPass;
    public static List<GameObject> PlayedCards=new List<GameObject>();

    void Start()
    {
        countPass=0;
        PlayerTurn=1;
        CardsPlayed=0;
    }

    public void EndTurn(){
        if(CardsPlayed==0){
            countPass++;
        }
        if(countPass>1){
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
    public static void PlayCard(GameObject card){
            CardsPlayed++;
            PlayedCards.Add(card);
            DisplayCard.played=true;
    }
    public static void NextRound(){
        Graveyard.AllToGraveyard();
        CardsPlayed=0;
        countPass=0;
        if(P1TotalFieldForce.P1ForceValue>P2TotalFieldForce.P2ForceValue){
            PlayerTurn=1;
            PlayerCondition.Wins("P1");
        }else if(P2TotalFieldForce.P2ForceValue>P1TotalFieldForce.P1ForceValue){
            PlayerTurn=2;
            PlayerCondition.Wins("P2");
        }else{
            PlayerCondition.Wins("P1");
            PlayerCondition.Wins("P2");
        }
        P1TotalFieldForce.P1ForceValue=0;
        P2TotalFieldForce.P2ForceValue=0;
    }
}
