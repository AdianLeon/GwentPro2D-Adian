using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static int PlayerTurn;
    public static int CardsPlayed;
    void Start()
    {
        PlayerTurn=1;
        CardsPlayed=0;
    }

    public void EndTurn(){
        if(PlayerTurn==1){
            PlayerTurn=2;
            CardsPlayed=0;
        }else{
            PlayerTurn=1;
            CardsPlayed=0;
        }
    }
    public static void PlayCard(){
            CardsPlayed++;
            DisplayCard.played=true;
    }
}
