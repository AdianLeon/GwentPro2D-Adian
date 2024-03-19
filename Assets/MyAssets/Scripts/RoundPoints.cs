using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundPoints : MonoBehaviour
{
    static int p1rPoints;//Puntos de ronda de cada jugador
    static int p2rPoints;

    public TextMeshProUGUI P1RPoints;//Puntaje de cada jugador
    public TextMeshProUGUI P2RPoints;
    void Start()
    {
        P1RPoints=GameObject.Find("RP1").GetComponent<TextMeshProUGUI>();
        P2RPoints=GameObject.Find("RP2").GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {//Hace que la cantidad de marcas sea igual a los puntos de cada jugador, si llega un nuevo punto de ronda se hace una nueva marca
        if(p1rPoints<PlayerCondition.rPointsP1){
            p1rPoints=PlayerCondition.rPointsP1;
            P1RPoints.text=P1RPoints.text+"X";
        }
        if(p2rPoints<PlayerCondition.rPointsP2){
            p2rPoints=PlayerCondition.rPointsP2;
            P2RPoints.text=P2RPoints.text+"X";
        }
    }
}
