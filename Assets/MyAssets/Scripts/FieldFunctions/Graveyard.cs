using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para la funcionalidad de los cementerios
public class Graveyard : MonoBehaviour
{
    //Este script es completamente static
    public static int gCount,egCount;//Contadores de cartas en los respectivos cementerios
    void Start(){
        gCount=0;
        egCount=0;
        GameObject.Find("GText").GetComponent<TextMeshProUGUI>().text=gCount.ToString();
        GameObject.Find("EGText").GetComponent<TextMeshProUGUI>().text=egCount.ToString();
    }
    public static void AllToGraveyard(){//Manda una por una todas las cartas jugadas al cementerio
        int count=TurnManager.playedCards.Count;
        for(int i=0;i<count;i++){
            ToGraveyard(TurnManager.playedCards[0]);
        }
    }
    public static void ToGraveyard(GameObject card){//Manda la carta al cementerio
        if(card.GetComponent<UnitCard>()!=null){
            for(int i=0;i<4;i++){//Deshace el efecto clima
                card.GetComponent<UnitCard>().affected[i]=false;
            }
            card.GetComponent<UnitCard>().addedPower=0;
        }
        string GraveyardName="";
        List<GameObject> Field=new List<GameObject>();
        if(card.GetComponent<Card>().whichField==Card.fields.P1){//Si el campo es de P1 manda la carta al cementerio de P1
            GraveyardName="Graveyard";
            Field=TotalFieldForce.p1PlayedCards;
            gCount++;
            GameObject.Find("GText").GetComponent<TextMeshProUGUI>().text=gCount.ToString();
        }else if(card.GetComponent<Card>().whichField==Card.fields.P2){//Si el campo es de P2 manda la carta al cementerio de P2
            GraveyardName="EnemyGraveyard";
            Field=TotalFieldForce.p2PlayedCards;
            egCount++;
            GameObject.Find("EGText").GetComponent<TextMeshProUGUI>().text=egCount.ToString();
        }
        card.transform.SetParent(GameObject.Find(GraveyardName).transform);
        card.GetComponent<Dragging>().parentToReturnTo=GameObject.Find(GraveyardName).transform;
        Field.Remove(card);
        TurnManager.playedCards.Remove(card);
        card.GetComponent<Dragging>().isDraggable=false;
        //Destroy(card.GetComponent<Dragging>());//No necesitaremos esto de nuevo
    }
}
