using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Script que lleva la condicion de victoria del jugador
public class PlayerCondition : MonoBehaviour
{
    private static int rPointsP1;//Puntos de ronda de cada jugador
    public static int GetRPointsP1{get=>rPointsP1;}
    public static void AddPointToP1(){rPointsP1++;}
    private static int rPointsP2;
    public static int GetRPointsP2{get=>rPointsP2;}
    public static void AddPointToP2(){rPointsP2++;}
    void Start(){
        rPointsP1=0;
        rPointsP2=0;
    }
    public static void WinCheck(){//Chequea quien ha ganado el juego
        if(rPointsP1!=rPointsP2){//Si la puntuacion es diferente (esto obliga a que el juego siga hasta que haya una ventaja)
            if(rPointsP1>1){//El primero que llegue a 2 puntos de ronda gana
                WinsGame("P1");
            }else if(rPointsP2>1){
                WinsGame("P2");
            }
        }else{
            RoundPoints.LongWriteUserRead("El proximo jugador que gane una ronda gana el juego!!");
        }
    }
    public static void WinsGame(string player){//El jugador gana la partida
        RoundPoints.LongWriteUserRead("Felicidades "+player+". Has ganado la partida!!");
        GameObject.Find("SetRGB").GetComponent<Button>().onClick.Invoke();
        Graveyard.AllToGraveyard();
    }
    public static void ResetGame(){//Reinicia el juego (o sea la escena Game)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
