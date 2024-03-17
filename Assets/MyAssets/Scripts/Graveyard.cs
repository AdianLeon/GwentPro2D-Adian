using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    void Start(){
    }
    public static void AllToGraveyard(){
        for(int i=0;i<TurnManager.PlayedCards.Count;i++){
            ToGraveyard(TurnManager.PlayedCards[i]);
        }
    }
    public static void ToGraveyard(GameObject card){
        DisplayCard.played=false;
        Dragging d=card.GetComponent<Dragging>();
        if(d.whichField==Dragging.fields.MyField){
            card.transform.SetParent(GameObject.Find("Graveyard").transform);
        }else if(d.whichField==Dragging.fields.EnemyField){
            card.transform.SetParent(GameObject.Find("EnemyGraveyard").transform);
        }
        
    }
}
