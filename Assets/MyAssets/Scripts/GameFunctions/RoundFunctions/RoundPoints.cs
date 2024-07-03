using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script que controla los puntos de ronda y los mensajes de lo que acontece en la ronda
public class RoundPoints : MonoBehaviour
{
    private static float secCounter;//Contador de segundos
    private static string message;//Mensaje 
    void Update(){//Si en algun momento secCounter es seteado a el tiempo en el juego se entrara en el condicional y se escribira el string message por 2s
        if(Time.time-secCounter<2){
            WriteUserRead(message);
        }
    }
    private static int p1AmountOfX;//Puntos de ronda de cada jugador
    private static int p2AmountOfX;
    void Start(){
        secCounter=0;
        p1AmountOfX=0;
        p2AmountOfX=0;
    }
    public static void LongWriteUserRead(string passedMessage){//Se llama cuando se desea poner un mensaje importante en el UserRead (dura 2s)
        secCounter=Time.time;
        message=passedMessage;
    }
    public static void WriteUserRead(string passedMessage){//Se llama cuando se desea poner un mensaje en el UserRead (no puede sobreescribir LongWriteUserRead)
        GameObject.Find("UserRead").GetComponent<TextMeshProUGUI>().text=passedMessage;
    }
    public static void WriteRoundInfoUserRead(){//Se llama cuando se desea escribir la informacion de ronda
        if(TurnManager.GetTurnActionsCount==0 && TurnManager.IsLastTurn){//Si se puede jugar y es el ultimo turno
            RoundPoints.WriteUserRead("Turno de "+TurnManager.GetPlayerTurn+", es el ultimo turno antes de que se acabe la ronda");
        }else if(TurnManager.GetTurnActionsCount==0){//Si no es el ultimo turno pero se puede jugar
            RoundPoints.WriteUserRead("Turno de "+TurnManager.GetPlayerTurn);
        }else if(TurnManager.IsLastTurn){//Si no se puede jugar pero es el ultimo turno
            RoundPoints.WriteUserRead("Puedes seguir jugando mas cartas. Presiona espacio cuando desees acabar la ronda");
        }else{//Si no se puede jugar y no es el ultimo turno
            RoundPoints.WriteUserRead("Presiona espacio para pasar de turno");
        }
    }
    public static void UpdatePoints(){//Hace que la cantidad de marcas sea igual a los puntos de cada jugador, si llega un nuevo punto de ronda se hace una nueva marca
        if(p1AmountOfX<PlayerCondition.GetRPointsP1){
            p1AmountOfX++;
            GameObject.Find("RP1").GetComponent<TextMeshProUGUI>().text=GameObject.Find("RP1").GetComponent<TextMeshProUGUI>().text+"X";
        }
        if(p2AmountOfX<PlayerCondition.GetRPointsP2){
            p2AmountOfX++;
            GameObject.Find("RP2").GetComponent<TextMeshProUGUI>().text=GameObject.Find("RP2").GetComponent<TextMeshProUGUI>().text+"X";
        }
    }

}
