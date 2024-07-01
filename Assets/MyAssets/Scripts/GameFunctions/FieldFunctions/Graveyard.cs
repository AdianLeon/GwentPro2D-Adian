using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
//Script para la funcionalidad de los cementerios
public class Graveyard : MonoBehaviour
{
    //Este script es completamente static
    public static int gP1Count,gP2Count;//Contadores de cartas en los respectivos cementerios
    void Start(){
        gP1Count=0;gP2Count=0;
        GameObject.Find("GTextP1").GetComponent<TextMeshProUGUI>().text=gP1Count.ToString();
        GameObject.Find("GTextP2").GetComponent<TextMeshProUGUI>().text=gP2Count.ToString();
    }
    public static void AllToGraveyard(){//Manda una por una todas las cartas jugadas al cementerio
        int count=TurnManager.PlayedCards.Count;
        for(int i=0;i<count;i++){
            ToGraveyard(TurnManager.PlayedCards[0]);
        }
    }
    public static void ToGraveyard(GameObject card){//Manda la carta al cementerio
        card.GetComponent<IAffectable>()?.AffectedByWeathers.Clear();
        if(card.GetComponent<CardWithPower>()!=null){
            card.GetComponent<CardWithPower>().AddedPower=0;
        }
        List<GameObject> Field=new List<GameObject>();
        if(card.GetComponent<Card>().WhichField==fields.P1){//Si el campo es de P1 manda la carta al cementerio de P1
            Field=TotalFieldForce.p1PlayedCards;
            gP1Count++;
        }else if(card.GetComponent<Card>().WhichField==fields.P2){//Si el campo es de P2 manda la carta al cementerio de P2
            Field=TotalFieldForce.p2PlayedCards;
            gP2Count++;
        }
        GameObject.Find("GText"+card.GetComponent<Card>().WhichField).GetComponent<TextMeshProUGUI>().text=gP1Count.ToString();
        card.transform.SetParent(GameObject.Find("Graveyard"+card.GetComponent<Card>().WhichField).transform);
        card.GetComponent<Dragging>().ParentToReturnTo=GameObject.Find("Graveyard"+card.GetComponent<Card>().WhichField).transform;
        Field.Remove(card);
        TurnManager.PlayedCards.Remove(card);
        card.GetComponent<Dragging>().IsDraggable=false;
    }
}
