using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para el efecto de multiplicar por n el poder
public class MultiplyEffect : CardEffect
{
    public int originalPower;//Se guarda el poder original de la carta
    //Esto es para evitar que si se activara el efecto dos o mas veces por uso del efecto del senuelo el poder de esta carta sea demasiado alto
    void Start(){
        originalPower=this.GetComponent<CardWithPower>().power;
    }
    override public void TriggerEffect(){//Multiplica por n su ataque, siendo n la cantidad de cartas iguales a ella en el campo.
        int n=1;//Contador de cuantas cartas del mismo tipo hay, contando la propia carta ya que todavia no se ha anadido a TotalFieldForce
        n+=CountSelfIn(TotalFieldForce.p1PlayedCards);//Se cuenta en el campo de P1 
        n+=CountSelfIn(TotalFieldForce.p2PlayedCards);//Se cuenta en el campo de P2
        this.GetComponent<CardWithPower>().power=originalPower*n;//Se iguala el poder de la carta jugada a n veces su propio poder
    }
    private int CountSelfIn(List<GameObject> PlayedCardsList){//Devuelve el conteo de cuantas veces se encuentra la carta en la lista
        int n=0;
        for(int i=0;i<PlayedCardsList.Count;i++){
            if(PlayedCardsList[i].GetComponent<Card>().cardRealName==this.GetComponent<Card>().cardRealName){
                //Si se encuentra una carta igual, se cuenta
                n++;
            }
        }
        return n;
    }
}
