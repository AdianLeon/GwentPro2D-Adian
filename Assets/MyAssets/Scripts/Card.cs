using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{//Propiedades de todas las cartas
    public int id;//Identificador unico de cada carta
    public int power;//Poder
    public bool isPlayed;//Si esta jugada o no
    public bool hasEffect;//Si tiene efecto o no
    
    public enum quality{None,Silver,Gold}//Quality
    public quality wQuality;

    /*public enum rank{Melee,Ranged,Siege,Aumento,Clima,Despeje,Senuelo,Dead}//Rank
    public rank wRank;
    
    public enum field{None,P1,P2}//Campo
    public field wField;*/

    public void Start(){
        isPlayed=false;
    }
}
