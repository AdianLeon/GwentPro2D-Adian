using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para funciones de los botones de cambio de turno y finalizacion de ronda
public class PassButtonScript : MonoBehaviour
{
    public void Toggle(){//Para desactivar si esta activado y activar si esta desactivado
        this.gameObject.SetActive(!this.gameObject.activeSelf);//Setea al estado en el que no esta
    }
    public static void ClickPassButtonWithoutEndTurn(){//Clickea una copia de PassButton pero sin la funcion EndTurn
    //Cuando un jugador que deberia comenzar la ronda gana en su turno el boton normalmente hace que el que deba jugar sea el otro
    //Es por esto que existe esta funcion, la idea es que se pulse doble y asi le toca jugar al que gano la ronda
        GameObject.Find("PassButtonWithoutEndTurn").GetComponent<Button>().onClick.Invoke();
    }
}