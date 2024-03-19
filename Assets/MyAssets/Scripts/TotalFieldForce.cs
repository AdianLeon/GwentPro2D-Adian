using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TotalFieldForce : MonoBehaviour
{
    public static int P1ForceValue=0;//Total de poder de cada jugador en la ronda
    public static int P2ForceValue=0;

    public TextMeshProUGUI P1totalForce;//Puntaje de cada jugador
    public TextMeshProUGUI P2totalForce;

    void Start()
    {
        P1totalForce=GameObject.Find("Points").GetComponent<TextMeshProUGUI>();
        P2totalForce=GameObject.Find("EnemyPoints").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        P1totalForce.text=P1ForceValue.ToString();//Asigna el valor al puntaje
        P2totalForce.text=P2ForceValue.ToString();
    }
}