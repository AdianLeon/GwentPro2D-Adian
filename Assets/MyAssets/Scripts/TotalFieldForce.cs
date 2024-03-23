using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TotalFieldForce : MonoBehaviour
{
    public static int P1ForceValue=0;//Total de poder de cada jugador en la ronda
    public static int P2ForceValue=0;
    public static int P1CardsLeft=26;
    public static int P2CardsLeft=26;

    void Update()
    {
        GameObject.Find("Points").GetComponent<TextMeshProUGUI>().text=P1ForceValue.ToString();//Asigna el valor al puntaje
        GameObject.Find("EnemyPoints").GetComponent<TextMeshProUGUI>().text=P2ForceValue.ToString();
    }
    public static void UpdateP1Deck(){//Actualiza el texto debajo del deck del p1 con las cartas que le quedan
        GameObject.Find("P1Deck").GetComponent<TextMeshProUGUI>().text=P1CardsLeft.ToString();
    }
    public static void UpdateP2Deck(){//Actualiza el texto debajo del deck del p2 con las cartas que le quedan
        GameObject.Find("P2Deck").GetComponent<TextMeshProUGUI>().text=P2CardsLeft.ToString();
    }
}