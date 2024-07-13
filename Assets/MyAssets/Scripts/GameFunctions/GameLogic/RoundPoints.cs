using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
//Script que representa los puntos de ronda de cada jugador
public class RoundPoints : StateListener
{
    //Puntos de ronda de cada jugador
    private int rPoints;
    public static int GetRPointsP1{get=>GameObject.Find("RoundPointsP1").GetComponent<RoundPoints>().rPoints;}
    public static void AddPointToP1(){GameObject.Find("RoundPointsP1").GetComponent<RoundPoints>().rPoints++;}
    public static int GetRPointsP2{get=>GameObject.Find("RoundPointsP2").GetComponent<RoundPoints>().rPoints;}
    public static void AddPointToP2(){GameObject.Find("RoundPointsP2").GetComponent<RoundPoints>().rPoints++;}
    private int getMarks{get=>this.GetComponent<TextMeshProUGUI>().text.Length;}//Cantidad de marcas de cada jugador
    public override void CheckState(){//Hace que la cantidad de marcas sea igual a los puntos de cada jugador, si llega un nuevo punto de ronda se hace una nueva marca
        switch(Judge.CurrentState){
            case State.SettingUpGame:
            case State.EndingRound:
            case State.EndingGame:
                UpdatePoints();
                break;
        }
    }
    private void UpdatePoints(){
        if(rPoints==0){
            this.gameObject.GetComponent<TextMeshProUGUI>().text="";
            return;
        }
        while(getMarks<rPoints){
            this.gameObject.GetComponent<TextMeshProUGUI>().text+="X";
        }
    }
}