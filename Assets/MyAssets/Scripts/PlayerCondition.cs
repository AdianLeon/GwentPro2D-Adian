using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Script que lleva la condicion de victoria del jugador
public class PlayerCondition : MonoBehaviour
{
    public static int rPointsP1;//Puntos de ronda de cada jugador
    public static int rPointsP2;

    public static void WinCheck(){//Chequea quien ha ganado el juego
        if(rPointsP1!=rPointsP2){//Si la puntuacion es diferente (esto obliga a que el juego siga hasta que haya un ganador)
            if(rPointsP1>2){//El primero que llegue a 3 gana
                GameWin("P1");
            }else if(rPointsP2>2){
                GameWin("P2");
            }
        }
    }
    public static void GameWin(string player){//El jugador gana la partida
        RoundPoints.URWrite("Felicidades "+player+". Has ganado la partida!!");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
