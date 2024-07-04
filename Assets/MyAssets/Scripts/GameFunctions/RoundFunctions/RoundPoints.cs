using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
//Script que controla los puntos de ronda, la condicion de victoria y los mensajes de lo que acontece en la ronda
public class RoundPoints : MonoBehaviour
{
    //Puntos de ronda de cada jugador
    private int rPoints;
    public static int GetRPointsP1{get=>GameObject.Find("RoundPointsP1").GetComponent<RoundPoints>().rPoints;}
    public static void AddPointToP1(){GameObject.Find("RoundPointsP1").GetComponent<RoundPoints>().rPoints++;}
    public static int GetRPointsP2{get=>GameObject.Find("RoundPointsP2").GetComponent<RoundPoints>().rPoints;}
    public static void AddPointToP2(){GameObject.Find("RoundPointsP2").GetComponent<RoundPoints>().rPoints++;}

    private int getMarks{get=>this.GetComponent<TextMeshProUGUI>().text.Length;}//Cantidad de marcas de cada jugador
    private static float secCounter;//Contador de segundos
    private static string message;//Mensaje 
    void Start(){
        rPoints=0;
        secCounter=0;
    }
    void Update(){//Cuando secCounter sea seteado a el tiempo en el juego se entrara en el condicional y se escribira el string message por 2s
        if(Time.time-secCounter<2){
            WriteUserRead(message);
        }
    }
    //Mensajes en el UserRead
    public static void LongWriteUserRead(string passedMessage){//Se llama cuando se desea poner un mensaje importante en el UserRead (dura 2s)
        secCounter=Time.time;
        message=passedMessage;
    }
    public static void WriteUserRead(string passedMessage){//Se llama cuando se desea poner un mensaje en el UserRead (no puede sobreescribir LongWriteUserRead)
        GameObject.Find("UserRead").GetComponent<TextMeshProUGUI>().text=passedMessage;
    }
    public static void WriteRoundInfoUserRead(){//Se llama cuando se desea escribir la informacion de ronda
        if(Board.GetTurnActionsCount==0 && Board.IsLastTurn){//Si se puede jugar y es el ultimo turno
            WriteUserRead("Turno de "+Board.GetPlayer+", es el ultimo turno antes de que se acabe la ronda");
        }else if(Board.GetTurnActionsCount==0){//Si no es el ultimo turno pero se puede jugar
            WriteUserRead("Turno de "+Board.GetPlayer);
        }else if(Board.IsLastTurn){//Si "no se puede jugar" pero es el ultimo turno
            WriteUserRead("Puedes seguir jugando mas cartas. Presiona espacio cuando desees acabar la ronda");
        }else{//Si no se puede jugar y no es el ultimo turno
            WriteUserRead("Presiona espacio para pasar de turno");
        }
    }
    //Condicion de victoria
    public static void WinCheck(){//Chequea quien ha ganado el juego
        if(GetRPointsP1!=GetRPointsP2){//Si la puntuacion es diferente (esto obliga a que el juego siga hasta que haya una ventaja)
            if(GetRPointsP1>1){//El primero que llegue a 2 puntos de ronda gana
                WinsGame("P1");
            }else if(GetRPointsP2>1){
                WinsGame("P2");
            }
        }else{
            RoundPoints.LongWriteUserRead("El proximo jugador que gane una ronda gana el juego!!");
        }
    }
    private static void WinsGame(string player){//El jugador gana la partida
        RoundPoints.LongWriteUserRead("Felicidades "+player+". Has ganado la partida!!");
        GameObject.Find("SetRGB").GetComponent<Button>().onClick.Invoke();
        Graveyard.SendToGraveyard(Board.PlayedCards);
    }
    public static void ResetGame(){//Reinicia el juego (o sea la escena Game) Este metodo es llamado por un boton llamado ResetGameButton
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void UpdatePoints(){//Hace que para ambos jugadores se actualice la puntuacion mostrada
        GameObject.Find("RoundPointsP1").GetComponent<RoundPoints>().MakeMark();
        GameObject.Find("RoundPointsP2").GetComponent<RoundPoints>().MakeMark();
    }
    private void MakeMark(){//Hace que la cantidad de marcas sea igual a los puntos de cada jugador, si llega un nuevo punto de ronda se hace una nueva marca
        if(getMarks<rPoints){this.gameObject.GetComponent<TextMeshProUGUI>().text+="X";}
    }
}