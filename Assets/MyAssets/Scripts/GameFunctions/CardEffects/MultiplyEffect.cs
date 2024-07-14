using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de multiplicar por n el poder
public class MultiplyEffect : MonoBehaviour, ICardEffect
{
    private int originalPower;//Se guarda el poder original de la carta
    //Esto es para evitar que si se activara el efecto dos o mas veces por uso del efecto del senuelo el poder de esta carta sea demasiado alto
    public int OriginalPower{get => originalPower;}
    void Awake(){
        originalPower=this.GetComponent<CardWithPower>().Power;
    }
    public void TriggerEffect(){//Multiplica por n su ataque, siendo n la cantidad de cartas iguales a ella en el campo.
        int n=0;//Contador de cuantas cartas del mismo tipo hay (n al menos sera 1 despues del conteo ya que la carta siempre se contara a si misma)
        List<GameObject> field=Field.PlayedCardsWithoutWeathers;
        foreach(GameObject card in field){
            if(card.GetComponent<Card>().CardName==this.GetComponent<Card>().CardName){n++;}
        }
        this.GetComponent<CardWithPower>().Power=originalPower*n;//Se iguala el poder de la carta jugada a n veces su propio poder
        GFUtils.UserRead.Write("Hay "+n+" cartas iguales a "+GetComponent<Card>().CardName+" y tiene "+originalPower+" de poder. Luego del efecto posee "+GetComponent<CardWithPower>().Power);
    }
}
