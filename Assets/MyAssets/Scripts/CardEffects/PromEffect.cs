using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para el efecto del promedio
public class PromEffect : CardEffect
{
    override public void TriggerEffect(){//Iguala el poder de la carta jugada al promedio del poder total de todas las cartas del campo (Solo las unidades (y senuelos), no se incluyen climas)
        int total=0;//Total de poder de todas las cartas del campo
        total+=TotalFieldForce.p1ForceValue;//Se anade el poder del P1
        total+=TotalFieldForce.p2ForceValue;//Se anade el poder del P2
        int divisor=TotalFieldForce.p1PlayedCards.Count+TotalFieldForce.p2PlayedCards.Count;//El divisor es el total de cartas en el campo
        if(divisor>0){//Si hay cartas en el campo
            this.GetComponent<UnitCard>().power=total/divisor;//El poder de la carta jugada es el promedio del total de poder de todas las cartas en el campo
            RoundPoints.URLongWrite("El total de poder en el campo es "+total+" y hay "+divisor+" cartas. El poder promedio es de: "+total/divisor);
        }else{
            RoundPoints.URLongWrite("No se pudo activar el efecto porque no se han jugado cartas");
        }
    }
}
