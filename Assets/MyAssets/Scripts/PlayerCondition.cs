using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public static int rPointsP1;
    public static int rPointsP2;
    void Start()
    {
        rPointsP1=0;
        rPointsP2=0;
    }
    public static void Wins(string winner){
        if(winner=="P1"){
            rPointsP1++;
        }else if(winner=="P2"){
            rPointsP2++;
        }else{
            rPointsP1++;
            rPointsP2++;
        }
        if(rPointsP1-rPointsP2>1){
            GameWin("Jugador 1");
        }else if(rPointsP2-rPointsP1>1){
            GameWin("Jugador 2");
        }
    }
    public static void GameWin(string player){
        Debug.Log("Felicidades "+player+". Has ganado la partida!!");
    }
}
