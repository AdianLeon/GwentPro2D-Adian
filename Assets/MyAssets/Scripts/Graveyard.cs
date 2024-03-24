using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Graveyard : MonoBehaviour
{
    public static int gCount=0,egCount=0;
    public static void AllToGraveyard(){//Manda una por una todas las cartas jugadas al cementerio
        for(int i=0;i<TurnManager.PlayedCards.Count;i++){
            if(TurnManager.PlayedCards[i]!=null)
                ToGraveyard(TurnManager.PlayedCards[i]);
        }
    }
    public static void ToGraveyard(GameObject card){//Manda la carta al cementerio
        card.GetComponent<Card>().isPlayed=false;
        Dragging d=card.GetComponent<Dragging>();
        if(d!=null){
            if(d.whichField==Dragging.fields.P1){//Si el campo es de P1 manda la carta al cementerio de P1
                card.transform.SetParent(GameObject.Find("Graveyard").transform);
                gCount++;
                GameObject.Find("GText").GetComponent<TextMeshProUGUI>().text=gCount.ToString();
            }else if(d.whichField==Dragging.fields.P2){//Si el campo es de P2 manda la carta al cementerio de P2
                card.transform.SetParent(GameObject.Find("EnemyGraveyard").transform);
                egCount++;
                GameObject.Find("EGText").GetComponent<TextMeshProUGUI>().text=egCount.ToString();
            }
        }
        Destroy(card.GetComponent<Dragging>());
    }
}
