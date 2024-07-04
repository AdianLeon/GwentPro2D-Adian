using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Script que contiene algunos efectos visuales
public static class VisualEffects
{
    public static bool SetPlayedLights{
        set{
            PlayedLight[] playedLights=GameObject.FindObjectsOfType<PlayedLight>();
            foreach(PlayedLight light in playedLights){
                if(value){light.OnGlow();}else{light.OffGlow();}
            }
        }
    }
    public static void ZonesGlow(GameObject card){//Hace que las cartas iluminen sus zonas y que el deck brille si se pueden intercambiar cartas
        //Si la carta puede ensenar su zona
        card.GetComponent<IShowZone>()?.ShowZone();//Que la ensene
        
        //Se actualiza si se puede intercambiar cartas con el deck
        if(DeckTrade.CheckValidTrade){//Si se puede
            GameObject.Find("DeckZone"+card.GetComponent<Card>().WhichField).GetComponent<DeckTrade>().OnGlow();//El deck de P1 "brilla"
        }
    }
    public static void OffZonesGlow(){//Resetea la invisibilidad de todas las dropzone del campo
        DropZone[] zones=GameObject.FindObjectsOfType<DropZone>();
        foreach(DropZone zone in zones){zone.OffGlow();}//Hace las zonas invisibles nuevamente
    }
    public static void OffCardsGlow(){//Restaura la iluminacion de las cartas jugadas
        foreach(GameObject playedCard in Board.PlayedCards){
            playedCard.GetComponent<Image>().color=new Color (1,1,1,1);//Las cartas se dessombrean
        }
    }
}