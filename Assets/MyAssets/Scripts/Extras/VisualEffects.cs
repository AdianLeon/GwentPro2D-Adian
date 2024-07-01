using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Script que contiene algunos efectos visuales
public class VisualEffects : MonoBehaviour
{
    public static void ZonesGlow(GameObject card){//Encuentra las zonas del mismo tipo y campo que la carta y las ilumina
        if(card.GetComponent<WeatherCard>()!=null || card.GetComponent<ClearWeatherCard>()!=null){//Si la carta es de clima o despeje
            DZWeather[] zones=GameObject.FindObjectsOfType<DZWeather>();//Se crea un array de todas las zonas de clima
            for(int i=0;i<zones.Length;i++){
                zones[i].GetComponent<Image>().color=new Color (1,1,1,0.1f);//Se iluminan
            }
        }else if(card.GetComponent<BoostCard>()!=null){//Si la carta es de aumento
            DZBoost[] boostZones=GameObject.FindObjectsOfType<DZBoost>();//Se crea un array de todas las zonas de aumento
            foreach(DZBoost boostZone in boostZones){
                if(boostZone.validPlayer==card.GetComponent<Card>().WhichField){//Si la zona es del jugador
                    boostZone.GetComponent<Image>().color=new Color (1,1,1,0.1f);//Se ilumina
                }
            }
        }else if(card.GetComponent<UnitCard>()!=null){//Si la carta es de unidad
            DZUnits[] unitZones=GameObject.FindObjectsOfType<DZUnits>();//Se crea un array con todas las zonas de cartas de unidad
            foreach(DZUnits unitZone in unitZones){
                if(unitZone.GetComponent<DZUnits>().isDropValid(card) && unitZone.validPlayer==card.GetComponent<Card>().WhichField){
                    //La zona se ilumina solo si coincide con la zona jugable y el campo de la carta
                    unitZone.GetComponent<Image>().color=new Color (1,1,1,0.1f);
                }
            }
        }
        //Se actualiza si se puede intercambiar cartas con el deck
        if(DeckTrade.UpdateRedraw()){//Si se puede
            GameObject.Find("DeckZone"+card.GetComponent<Card>().WhichField).GetComponent<Image>().color=new Color (0,1,0,0.1f);//El deck de P1 "brilla"
        }
    }
    public static void OffZonesGlow(){//Resetea la invisibilidad de todas las dropzone del campo
        DropZone[] zones=GameObject.FindObjectsOfType<DropZone>();
        foreach(DropZone zone in zones){zone.GetComponent<Image>().color=new Color (1,1,1,0);}//Hace las zonas invisibles nuevamente
    }
    public static void PlayedLightsToColor(Color color){//Afecta a unos objetos del campo y los pone del color pasado como parametro
        GameObject.Find("PlayedLightUpper").GetComponent<Image>().color=color;
        GameObject.Find("PlayedLightLower").GetComponent<Image>().color=color;
        GameObject.Find("PlayedLightMiddle").GetComponent<Image>().color=color;
        GameObject.Find("PlayedLightLeft").GetComponent<Image>().color=color;
        GameObject.Find("PlayedLightRight").GetComponent<Image>().color=color;
    }
    public static void ValidSwapsGlow(GameObject card){//Ilumina las cartas con las que el senuelo pasado como parametro se puede intercambiar
        //En realidad oscurece las cartas con las que el senuelo no se puede intercambiar
        Debug.Log("ValidSwapsGlow:");
        foreach(GameObject cardPlayed in TurnManager.playedCards){
            Debug.Log(cardPlayed);
            if(cardPlayed.GetComponent<IAffectable>()==null || cardPlayed.GetComponent<Card>().WhichField!=card.GetComponent<Card>().WhichField){
                //Si no es afectable o si no coincide con el campo del senuelo
                Debug.Log("Darkened");
                cardPlayed.GetComponent<Image>().color=new Color (0.5f,0.5f,0.5f,1);
            }else{Debug.Log("Left Undarkened");}
        }
    }
    public static void OffCardsGlow(){
        for(int i=0;i<TurnManager.playedCards.Count;i++){
            TurnManager.playedCards[i].GetComponent<Image>().color=new Color (1,1,1,1);//Las cartas se dessombrean
        }
    }
}