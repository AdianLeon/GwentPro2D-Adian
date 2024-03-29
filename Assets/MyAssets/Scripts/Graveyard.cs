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
            if(TurnManager.PlayedCards[i]!=null)//Si se ha jugado cartas
                ToGraveyard(TurnManager.PlayedCards[i]);
        }
    }
    public static void ToGraveyard(GameObject card){//Manda la carta al cementerio
        card.GetComponent<Card>().isPlayed=false;//La carta ya no esta jugada
        for(int i=0;i<4;i++){//Deshace el efecto clima
            if(card.GetComponent<Card>().affected[i]){
                card.GetComponent<Card>().affected[i]=false;
                card.GetComponent<Card>().addedPower++;
            }
        }
        if(card.GetComponent<Dragging>()!=null){
            if(card.GetComponent<Dragging>().whichField==Dragging.fields.P1){//Si el campo es de P1 manda la carta al cementerio de P1
                card.transform.SetParent(GameObject.Find("Graveyard").transform);
                gCount++;
                GameObject.Find("GText").GetComponent<TextMeshProUGUI>().text=gCount.ToString();
            }else if(card.GetComponent<Dragging>().whichField==Dragging.fields.P2){//Si el campo es de P2 manda la carta al cementerio de P2
                card.transform.SetParent(GameObject.Find("EnemyGraveyard").transform);
                egCount++;
                GameObject.Find("EGText").GetComponent<TextMeshProUGUI>().text=egCount.ToString();
            }
            Destroy(card.GetComponent<Dragging>());//No necesitaremos esto de nuevo
        }
    }
}
