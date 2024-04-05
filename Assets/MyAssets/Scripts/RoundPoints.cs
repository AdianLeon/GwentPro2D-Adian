using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script que controla los puntos de ronda
public class RoundPoints : MonoBehaviour
{
    static int p1AmountOfX;//Puntos de ronda de cada jugador
    static int p2AmountOfX;
    void Start(){
        p1AmountOfX=0;
        p2AmountOfX=0;
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
    public static void URWrite(string message){
        GameObject.Find("UserRead").GetComponent<TextMeshProUGUI>().text=message;
    }
}
