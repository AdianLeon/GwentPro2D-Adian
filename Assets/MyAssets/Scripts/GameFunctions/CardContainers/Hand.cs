using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour, IContainer
{
    public List <GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}
    public List<GameObject> P1Hand{get=>GameObject.Find("HandP1").GetComponent<Deck>().GetCards;}
    public List<GameObject> P2Hand{get=>GameObject.Find("HandP2").GetComponent<Deck>().GetCards;}
    public static void CheckHands(){
        GameObject.Find("HandP1").GetComponent<Hand>().CheckHand();
        GameObject.Find("HandP2").GetComponent<Hand>().CheckHand();
    }
    public void CheckHand(){
        while(this.transform.childCount>10){//Si una carta no cabe en la mano
            Graveyard.SendToGraveyard(this.transform.GetChild(this.transform.childCount-1).gameObject);//Se envia al cementerio
        }
    }
}
