using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Script que lleva la condicion de victoria del jugador
public class PlayerCondition : MonoBehaviour
{
    public static int rPointsP1;//Puntos de ronda de cada jugador
    public static int rPointsP2;

    void Start(){
        rPointsP1=0;
        rPointsP2=0;
    }

    public static void WinCheck(){//Chequea quien ha ganado el juego
        if(rPointsP1!=rPointsP2){//Si la puntuacion es diferente (esto obliga a que el juego siga hasta que haya una ventaja)
            if(rPointsP1>1){//El primero que llegue a 2 puntos de ronda gana
                GameWin("P1");
            }else if(rPointsP2>1){
                GameWin("P2");
            }
        }else{
            RoundPoints.URWrite("El proximo jugador que gane una ronda gana el juego!!");
        }
    }
    public static void GameWin(string player){//El jugador gana la partida
        RoundPoints.URWrite("Felicidades "+player+". Has ganado la partida!!");
        GameObject.Find("SetRGB").GetComponent<Button>().onClick.Invoke();
        Graveyard.AllToGraveyard();
    }
    public static void ResetGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
