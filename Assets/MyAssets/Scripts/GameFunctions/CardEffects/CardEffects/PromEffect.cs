using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para el efecto del promedio
public class PromEffect : MonoBehaviour, ICardEffect, IToJson
{
    public void TriggerEffect(){//Iguala el poder de la carta jugada al promedio del poder total de todas las cartas del campo (Solo las unidades (y senuelos), no se incluyen climas)
        
        int total=0;//Total de poder de todas las cartas del campo
        total+=Field.P1ForceValue;//Se anade el poder del P1
        total+=Field.P2ForceValue;//Se anade el poder del P2
        int divisor=Field.P1PlayedCards.Count+Field.P2PlayedCards.Count;//El divisor es el total de cartas en el campo
        if(divisor>0){//Si hay cartas en el campo
            this.GetComponent<UnitCard>().power=total/divisor;//El poder de la carta jugada es el promedio del total de poder de todas las cartas en el campo
            RoundPoints.LongWriteUserRead("El total de poder en el campo es "+total+" y hay "+divisor+" cartas. El poder promedio es de: "+total/divisor);
        }else{
            RoundPoints.LongWriteUserRead("No se pudo activar el efecto porque no se han jugado cartas");
        }
    }
}
