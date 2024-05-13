using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromEffect : CardEffect
{
    override public void TriggerEffect(){//Iguala el poder de la carta jugada al promedio del poder total de todas las cartas del campo (Solo las unidades, no se incluyen climas)
        int total=0;//Total de poder de todas las cartas del campo
        
        total+=TotalFieldForce.P1ForceValue;//Se anade el poder del P1
        total+=TotalFieldForce.P2ForceValue;//Se anade el poder del P2
        int divisor=TotalFieldForce.P1PlayedCards.Count+TotalFieldForce.P2PlayedCards.Count;//El divisor es el total de cartas en el campo
        if(divisor==0){//Si no hay cartas en el campo
            this.GetComponent<Card>().power=0;//El poder es 0
        }else{//Si hay cartas en el campo
            this.GetComponent<Card>().power=total/divisor;//El poder de la carta jugada es el promedio del total de poder de todas las cartas en el campo
        }
    }
}
