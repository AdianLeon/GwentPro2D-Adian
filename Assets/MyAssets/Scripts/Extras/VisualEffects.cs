using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Script que contiene algunos efectos visuales
public static class VisualEffects
{
    public static void ZonesGlow(GameObject card){//Hace que las cartas iluminen sus zonas y que el deck brille si se pueden intercambiar cartas
        //Si la carta puede ensenar su zona
        card.GetComponent<IShowZone>()?.ShowZone();//Que la ensene
        
        //Se actualiza si se puede intercambiar cartas con el deck
        if(DeckTrade.UpdateRedraw()){//Si se puede
            GameObject.Find("DeckZone"+card.GetComponent<Card>().WhichField).GetComponent<Image>().color=new Color (0,1,0,0.1f);//El deck de P1 "brilla"
        }
    }
    public static void OffZonesGlow(){//Resetea la invisibilidad de todas las dropzone del campo
        DropZone[] zones=GameObject.FindObjectsOfType<DropZone>();
        foreach(DropZone zone in zones){zone.GetComponent<Image>().color=new Color (1,1,1,0);}//Hace las zonas invisibles nuevamente
    }
    public static void OffCardsGlow(){//Restaura la iluminacion de las cartas jugadas
        foreach(GameObject playedCard in TurnManager.playedCards){
            playedCard.GetComponent<Image>().color=new Color (1,1,1,1);//Las cartas se dessombrean
        }
    }
    public static void PlayedLightsToColor(Color color){//Afecta a unos objetos del campo y los pone del color pasado como parametro
        GameObject.Find("PlayedLightUpper").GetComponent<Image>().color=color;
        GameObject.Find("PlayedLightLower").GetComponent<Image>().color=color;
        GameObject.Find("PlayedLightMiddle").GetComponent<Image>().color=color;
        GameObject.Find("PlayedLightLeft").GetComponent<Image>().color=color;
        GameObject.Find("PlayedLightRight").GetComponent<Image>().color=color;
    }
}