using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Script que contiene algunos efectos visuales
public class VisualEffects : MonoBehaviour
{
    public static void ZonesGlow(GameObject card){//Encuentra las zonas del mismo tipo y campo que la carta y las ilumina
        if(TurnManager.cardsPlayed==0 || TurnManager.lastTurn){//Si el jugador puede jugar
            if(card.GetComponent<WeatherCard>()!=null){//Si la carta es de clima
                DZWeather[] zones=GameObject.FindObjectsOfType<DZWeather>();//Se crea un array de todas las zonas de clima
                for(int i=0;i<zones.Length;i++){
                    zones[i].GetComponent<Image>().color=new Color (1,1,1,0.1f);//Se iluminan
                }
            }else if(card.GetComponent<BoostCard>()!=null){//Si la carta es de aumento
                DZBoost[] zones=GameObject.FindObjectsOfType<DZBoost>();//Se crea un array de todas las zonas de aumento
                for(int i=0;i<zones.Length;i++){
                    if(zones[i].validPlayer==card.GetComponent<Card>().whichField){//Si la zona es del jugador
                        zones[i].GetComponent<Image>().color=new Color (1,1,1,0.1f);//Se ilumina
                    }
                }
            }else if(card.GetComponent<UnitCard>()!=null){//Si la carta es de unidad
                DZUnits[] zones=GameObject.FindObjectsOfType<DZUnits>();//Se crea un array con todas las zonas de cartas de unidad
                for(int i=0;i<zones.Length;i++){
                    if(zones[i].validZone==card.GetComponent<UnitCard>().whichZone && zones[i].validPlayer==card.GetComponent<Card>().whichField){
                        //La zona se ilumina solo si coincide con la zona jugable y el campo de la carta
                        zones[i].GetComponent<Image>().color=new Color (1,1,1,0.1f);
                    }
                }
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
        if(!GameObject.Find("Leader").GetComponent<LeaderEffect>().used){
            GameObject.Find("PlayedLightLeaderSkillP1").GetComponent<Image>().color=green;
        }
        if(!GameObject.Find("EnemyLeader").GetComponent<LeaderEffect>().used){
            GameObject.Find("PlayedLightLeaderSkillP2").GetComponent<Image>().color=green;
        }
    }
    public static void PlayedLightsOff(){//Afecta a unos objetos del campo y los pone rojos
        Color red=new Color(1,0,0,0.2f);
        GameObject.Find("PlayedLightUpper").GetComponent<Image>().color=red;
        GameObject.Find("PlayedLightLower").GetComponent<Image>().color=red;
        GameObject.Find("PlayedLightMiddle").GetComponent<Image>().color=red;
        GameObject.Find("PlayedLightLeft").GetComponent<Image>().color=red;
        GameObject.Find("PlayedLightRight").GetComponent<Image>().color=red;
        GameObject.Find("PlayedLightLeaderSkillP1").GetComponent<Image>().color=red;
        GameObject.Find("PlayedLightLeaderSkillP2").GetComponent<Image>().color=red;
    }
}