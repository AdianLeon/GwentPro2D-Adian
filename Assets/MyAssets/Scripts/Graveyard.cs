using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    void Start(){
    }
    public static void AllToGraveyard(){//Manda una por una todas las cartas jugadas al cementerio
        for(int i=0;i<TurnManager.PlayedCards.Count;i++){
            ToGraveyard(TurnManager.PlayedCards[i]);
        }
    }
    public static void ToGraveyard(GameObject card){//Manda la carta al cementerio
        card.GetComponent<Card>().isPlayed=false;
        Dragging d=card.GetComponent<Dragging>();
        if(d.whichField==Dragging.fields.P1){//Si el campo es de P1 manda la carta al cementerio de P1
            card.transform.SetParent(GameObject.Find("Graveyard").transform);
        }else if(d.whichField==Dragging.fields.P2){//Si el campo es de P2 manda la carta al cementerio de P2
            card.transform.SetParent(GameObject.Find("EnemyGraveyard").transform);
        }
    }
}
