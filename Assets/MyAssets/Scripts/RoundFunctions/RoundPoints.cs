using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script que controla los puntos de ronda y los mensajes de lo que acontece en la ronda
public class RoundPoints : MonoBehaviour
{
    public static float secCounter;
    public static string message;
    private static int p1AmountOfX;//Puntos de ronda de cada jugador
    private static int p2AmountOfX;
    void Start(){
        secCounter=0;
        p1AmountOfX=0;
        p2AmountOfX=0;
    }
    public static void URLongWrite(string passedMessage){//Se llama cuando se desea poner un mensaje importante en el UserRead (dura 1s)
        secCounter=Time.time;
        message=passedMessage;
    }
    public static void URWrite(string passedMessage){//Se llama cuando se desea poner un mensaje en el UserRead (no puede sobreescribir URLongWrite)
        GameObject.Find("UserRead").GetComponent<TextMeshProUGUI>().text=passedMessage;
    }
    public static void URWriteRoundInfo(){//Se llama cuando se desea escribir la informacion de ronda
        if(TurnManager.cardsPlayed==0 && TurnManager.lastTurn){
            RoundPoints.URWrite("Turno de P"+TurnManager.playerTurn+", es el ultimo turno antes de que se acabe la ronda");
        }else if(TurnManager.cardsPlayed==0){
            RoundPoints.URWrite("Turno de P"+TurnManager.playerTurn);
        }else if(TurnManager.lastTurn){
            RoundPoints.URWrite("Puedes seguir jugando mas cartas. Presiona espacio cuando desees acabar la ronda");
        }else{
            RoundPoints.URWrite("Presiona espacio para pasar de turno");
        }
    }
    public static void UpdatePoints(){//Hace que la cantidad de marcas sea igual a los puntos de cada jugador, si llega un nuevo punto de ronda se hace una nueva marca
        if(p1AmountOfX<PlayerCondition.rPointsP1){
            p1AmountOfX++;
            GameObject.Find("RP1").GetComponent<TextMeshProUGUI>().text=GameObject.Find("RP1").GetComponent<TextMeshProUGUI>().text+"X";
        }
        if(p2AmountOfX<PlayerCondition.rPointsP2){
            p2AmountOfX++;
            GameObject.Find("RP2").GetComponent<TextMeshProUGUI>().text=GameObject.Find("RP2").GetComponent<TextMeshProUGUI>().text+"X";
        }
    }

}
