using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{//Propiedades de todas las cartas
    public int id;//Identificador unico de cada carta
    public int power;//Poder
    public int addedPower;
    public bool isPlayed;//Si esta jugada o no
    public bool hasEffect;//Si tiene efecto o no
    public bool[] affected=new bool[4];//Un array que describe si la carta esta siendo afectada por un clima, la posicion del true es
    //el id de la carta que la afecta
    
    public enum quality{None,Silver,Gold}//Quality
    public quality wQuality;

    /*public enum rank{Melee,Ranged,Siege,Aumento,Clima,Despeje,Senuelo,Dead}//Rank
    public rank wRank;
    
    public enum field{None,P1,P2}//Campo
    public field wField;*/
}
