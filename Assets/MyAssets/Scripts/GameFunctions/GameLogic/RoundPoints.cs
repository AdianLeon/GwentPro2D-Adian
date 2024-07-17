using UnityEngine;
using TMPro;
//Script que representa los puntos de ronda de cada jugador
public class RoundPoints : StateListener
{
    public override int GetPriority=>1;
    private int rPoints;//Puntos de ronda de cada jugador
    public static int GetRPointsP1{get=>GameObject.Find("RoundPointsP1").GetComponent<RoundPoints>().rPoints;}
    public static void AddPointToP1(){GameObject.Find("RoundPointsP1").GetComponent<RoundPoints>().rPoints++;}
    public static int GetRPointsP2{get=>GameObject.Find("RoundPointsP2").GetComponent<RoundPoints>().rPoints;}
    public static void AddPointToP2(){GameObject.Find("RoundPointsP2").GetComponent<RoundPoints>().rPoints++;}
    private int getMarks{get=>this.GetComponent<TextMeshProUGUI>().text.Length;}//Cantidad de marcas de cada jugador
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame://Si estamos iniciando el juego reiniciamos los puntos de ronda
                rPoints=0;
                break;
        }
        switch(Judge.CurrentState){
            case State.SettingUpGame://Se actualizan los puntos
            case State.EndingRound:
            case State.EndingGame:
                UpdatePoints();
                break;
        }
    }
    private void UpdatePoints(){//Hace que la cantidad de marcas sea igual a los puntos de cada jugador, si llega un nuevo punto de ronda se hace una nueva marca
        if(rPoints==0){
            this.gameObject.GetComponent<TextMeshProUGUI>().text="";
            return;
        }
        while(getMarks<rPoints){
            this.gameObject.GetComponent<TextMeshProUGUI>().text+="X";
        }
    }
}