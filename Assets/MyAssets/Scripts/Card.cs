using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{//Propiedades de todas las cartas
    public int id;//Identificador unico de las cartas de clima

    public int power;//Poder

    public int addedPower;

    public bool isPlayed;//Si esta jugada o no

    public bool hasEffect;//Si tiene efecto o no

    public bool[] affected=new bool[4];//Un array que describe si la carta esta siendo afectada por un clima, la posicion del true es el id de la carta clima que la afecta
    
    public enum quality{None,Silver,Gold}//Calidad de la carta, si es plata tendra hasta 3 copias, si es oro no sera afectada por ningun efecto durante el juego
    public quality wQuality;
}
