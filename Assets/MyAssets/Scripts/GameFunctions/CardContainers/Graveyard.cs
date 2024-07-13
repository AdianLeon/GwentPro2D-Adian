using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Diagnostics;
//Script para la funcionalidad de los cementerios
public class Graveyard : CustomBehaviour, IContainer
{
    public List<GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}
    public static List<GameObject> PlayerGraveyardCards{get=>GameObject.Find("Graveyard"+Judge.GetPlayer).GetComponent<Graveyard>().GetCards;}
    public static List<GameObject> EnemyGraveyardCards{get=>GameObject.Find("Graveyard"+Judge.GetEnemy).GetComponent<Graveyard>().GetCards;}
    private int deadCount{get=>this.transform.childCount;}//Contadores de cartas en los respectivos cementerios
    public override void Initialize(){
        NextUpdate();
    }
    public override void Finish(){
        GFUtils.GetRidOf(GetCards);
        NextUpdate();
    }
    public override void NextUpdate(){
        GameObject.Find("GText"+GFUtils.GetField(this.name)).GetComponent<TextMeshProUGUI>().text=deadCount.ToString();
    }
    public static void SendToGraveyard(List<GameObject> souls){//Analiza una por una las cartas de la lista para enviarlas al cementerio
        foreach(GameObject soul in souls){
            SendToGraveyard(soul);
        }
    }
    public static void SendToGraveyard(GameObject soul){//Contacta al cementerio correspondiente 
        GameObject.Find("Graveyard"+soul.GetComponent<Card>().WhichPlayer).GetComponent<Graveyard>().ToGraveyard(soul);
    }
    private void ToGraveyard(GameObject card){//Manda la carta al cementerio
        card.GetComponent<IAffectable>()?.AffectedByWeathers.Clear();
        if(card.GetComponent<CardWithPower>()!=null){
            card.GetComponent<CardWithPower>().AddedPower=0;
        }
        GameObject.Find("GText"+card.GetComponent<Card>().WhichPlayer).GetComponent<TextMeshProUGUI>().text=deadCount.ToString();
        card.GetComponent<Dragging>().DropCardOnZone(GameObject.Find("Graveyard"+card.GetComponent<Card>().WhichPlayer));
    }
}
