using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCondition : MonoBehaviour
{
    public static int rPointsP1;//Puntos de ronda de cada jugador
    public static int rPointsP2;

    public static void Wins(string winner){
        if(winner=="P1"){//Si el jugador 1 gana la ronda se le anade un punto
            rPointsP1++;
        }else if(winner=="P2"){//Si el jugador 2 gana la ronda se le anade un punto
            rPointsP2++;
        }

        if(rPointsP1-rPointsP2>1){//Si la diferencia de puntos es 2, hay un ganador
            GameWin("Jugador 1");
        }else if(rPointsP2-rPointsP1>1){
            GameWin("Jugador 2");
        }
    }
    public static void GameWin(string player){//El jugador gana la partida
        Debug.Log("Felicidades "+player+". Has ganado la partida!!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
}
