using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Diagnostics;
//Script para la funcionalidad de los cementerios
public class Graveyard : MonoBehaviour, IContainer
{
    public List<GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}
    public List<GameObject> P1Graveyard{get=>GameObject.Find("GraveyardP1").GetComponent<Deck>().GetCards;}
    public List<GameObject> P2Graveyard{get=>GameObject.Find("GraveyardP1").GetComponent<Deck>().GetCards;}
    private int deadCount;//Contadores de cartas en los respectivos cementerios
    void Start(){
        deadCount=0;
        GameObject.Find("GText"+GFUtils.GetField(this.name)).GetComponent<TextMeshProUGUI>().text=deadCount.ToString();
    }
    public static void SendToGraveyard(List<GameObject> souls){//Analiza una por una las cartas de la lista para enviarlas al cementerio
        foreach(GameObject soul in souls){
            SendToGraveyard(soul);
        }
    }
    public static void SendToGraveyard(GameObject soul){//Contacta al cementerio correspondiente 
        GameObject.Find("Graveyard"+soul.GetComponent<Card>().WhichField).GetComponent<Graveyard>().ToGraveyard(soul);
    }
    private void ToGraveyard(GameObject card){//Manda la carta al cementerio
        card.GetComponent<IAffectable>()?.AffectedByWeathers.Clear();
        if(card.GetComponent<CardWithPower>()!=null){
            card.GetComponent<CardWithPower>().AddedPower=0;
        }
        deadCount++;
        GameObject.Find("GText"+card.GetComponent<Card>().WhichField).GetComponent<TextMeshProUGUI>().text=deadCount.ToString();
        card.GetComponent<Dragging>().DropCardOnZone(GameObject.Find("Graveyard"+card.GetComponent<Card>().WhichField));
    }
}
