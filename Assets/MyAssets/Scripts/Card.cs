using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script que contiene las propiedades de todas las cartas
public class Card : MonoBehaviour
{
    public int id;//Identificador unico de las cartas de clima y las de efecto
    public int power;//Poder propio de la carta
    public int addedPower;//Poder anadido por efectos durante el juego

    public string cardRealName;
    public string description;

    public bool hasEffect;//Si tiene efecto o no
    public string effectDescription;

    public Sprite artwork;

    public bool[] affected=new bool[4];//Un array que describe si la carta esta siendo afectada por un clima, la posicion del true es el id de la carta clima que la afecta
    
    public enum quality{None,Silver,Gold}//Calidad de la carta, si es plata tendra hasta 3 copias, si es oro no sera afectada por ningun efecto durante el juego
    public quality wQuality;
}
