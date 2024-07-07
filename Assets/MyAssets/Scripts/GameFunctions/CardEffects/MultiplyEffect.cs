using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para el efecto de multiplicar por n el poder
public class MultiplyEffect : MonoBehaviour, ICardEffect
{
    private int originalPower;//Se guarda el poder original de la carta
    //Esto es para evitar que si se activara el efecto dos o mas veces por uso del efecto del senuelo el poder de esta carta sea demasiado alto
    public int OriginalPower{get => originalPower;}
    void Awake(){
        Debug.Log("Multiply effect");
        Debug.Log("P:"+this.GetComponent<CardWithPower>().power);
        originalPower=this.GetComponent<CardWithPower>().power;
        Debug.Log("OP:"+originalPower);
    }
    public void TriggerEffect(){//Multiplica por n su ataque, siendo n la cantidad de cartas iguales a ella en el campo.
        int n=0;//Contador de cuantas cartas del mismo tipo hay (n al menos sera 1 despues del conteo ya que la carta siempre se contara a si misma)
        List<GameObject> field=Field.PlayedCardsWithoutWeathers;
        foreach(GameObject card in field){
            if(card.GetComponent<Card>().CardName==this.GetComponent<Card>().CardName){n++;}
        }
        this.GetComponent<CardWithPower>().power=originalPower*n;//Se iguala el poder de la carta jugada a n veces su propio poder
    }
}
