using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Script que contiene algunos efectos visuales
public class VisualEffects : MonoBehaviour
{
    public static void ZonesGlow(GameObject card){//Encuentra las zonas del mismo tipo y campo que la carta y las ilumina
        DropZone[] zones=GameObject.FindObjectsOfType<DropZone>();
        for(int i=0;i<zones.Length;i++){
            bool generalCase=(zones[i].GetComponent<DropZone>().validZone==card.GetComponent<Card>().whichZone) && (zones[i].GetComponent<DropZone>().validPlayer==card.GetComponent<Card>().whichField);//Caso general es cualquier carta que no sea de clima, debe coincidir en tipo y campo
            bool climaCase=(Card.zones.Weather==card.GetComponent<Card>().whichZone) && (Card.zones.Weather==zones[i].GetComponent<DropZone>().validZone);//Caso clima es que tanto la carta como la zona sean tipo clima
            bool usualCase=(generalCase || climaCase) && TurnManager.CardsPlayed==0;//El caso usual es cuando solo se puede jugar una carta y esta carta puede ser de caso general o clima
            bool afterPassCase=(generalCase || climaCase) && TurnManager.lastTurn;//El caso afterPass es cuando un jugador pasa y ahora el otro puede jugar tantas cartas como quiera de caso general o clima
            if(afterPassCase || usualCase){//Para cualquiera de los dos casos usual o afterPass iluminaremos la(s) zona(s) donde el jugador puede poner la carta
                zones[i].GetComponent<Image>().color=new Color (1,1,1,0.1f);
            }
        }
        DeckTrade.UpdateRedraw();//Se actualiza si se puede intercambiar cartas con el deck
        if(DeckTrade.redrawable){//Si se puede
            if(card.GetComponent<Card>().whichField==Card.fields.P1){//La carta es de P1
                GameObject.Find("MyDeckZone").GetComponent<Image>().color=new Color (0,1,0,0.1f);//El deck de P1 "brilla"
            }else if(card.GetComponent<Card>().whichField==Card.fields.P2){//La carta es de P2
                GameObject.Find("EnemyDeckZone").GetComponent<Image>().color=new Color (0,1,0,0.1f);//El deck de P2 "brilla"
            }
        }
    }
    public static void OffZonesGlow(){//Resetea la invisibilidad de todas las dropzone del campo
        DropZone[] zones=GameObject.FindObjectsOfType<DropZone>();
        for(int i=0;i<zones.Length;i++){
            zones[i].GetComponent<Image>().color=new Color (1,1,1,0);
        }
        GameObject.Find("MyDeckZone").GetComponent<Image>().color=new Color (1,1,1,0);//Incluye los decks
        GameObject.Find("EnemyDeckZone").GetComponent<Image>().color=new Color (1,1,1,0);
    }
    public static void PlayedLightsOn(){//Afecta a unos objetos del campo y los pone verdes
        Color green=new Color(0,1,0,0.2f);
        GameObject.Find("PlayedLightUpper").GetComponent<Image>().color=green;
        GameObject.Find("PlayedLightLower").GetComponent<Image>().color=green;
        GameObject.Find("PlayedLightMiddle").GetComponent<Image>().color=green;
        GameObject.Find("PlayedLightLeft").GetComponent<Image>().color=green;
        GameObject.Find("PlayedLightRight").GetComponent<Image>().color=green;
    }
    public static void PlayedLightsOff(){//Afecta a unos objetos del campo y los pone rojos
        Color red=new Color(1,0,0,0.2f);
        GameObject.Find("PlayedLightUpper").GetComponent<Image>().color=red;
        GameObject.Find("PlayedLightLower").GetComponent<Image>().color=red;
        GameObject.Find("PlayedLightMiddle").GetComponent<Image>().color=red;
        GameObject.Find("PlayedLightLeft").GetComponent<Image>().color=red;
        GameObject.Find("PlayedLightRight").GetComponent<Image>().color=red;
    }
}