using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para el efecto de multiplicar por n el poder
public class MultiplyEffect : MonoBehaviour, ICardEffect, IToJson
{
    private int originalPower;//Se guarda el poder original de la carta
    //Esto es para evitar que si se activara el efecto dos o mas veces por uso del efecto del senuelo el poder de esta carta sea demasiado alto
    public int OriginalPower{get => originalPower;}
    void Start(){
        originalPower=this.GetComponent<CardWithPower>().power;
    }
    public void TriggerEffect(){//Multiplica por n su ataque, siendo n la cantidad de cartas iguales a ella en el campo.
        int n=1;//Contador de cuantas cartas del mismo tipo hay, contando la propia carta ya que todavia no se ha anadido a Field
        List<GameObject> field=Field.PlayedCardsWithoutWeathers;
        foreach(GameObject card in field){
            if(card.GetComponent<Card>().cardName==this.GetComponent<Card>().cardName){n++;}
        }
        this.GetComponent<CardWithPower>().power=originalPower*n;//Se iguala el poder de la carta jugada a n veces su propio poder
    }
}
