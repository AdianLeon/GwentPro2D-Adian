using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para la funcionalidad de los cementerios
public class Graveyard : MonoBehaviour
{
    public static int gCount,egCount;
    void Start(){
        gCount=0;
        egCount=0;
        GameObject.Find("GText").GetComponent<TextMeshProUGUI>().text=gCount.ToString();
        GameObject.Find("EGText").GetComponent<TextMeshProUGUI>().text=egCount.ToString();
    }
    public static void AllToGraveyard(){//Manda una por una todas las cartas jugadas al cementerio
        for(int i=0;i<TurnManager.PlayedCards.Count;i++){
            if(TurnManager.PlayedCards[i]!=null)//Si se ha jugado cartas
                ToGraveyard(TurnManager.PlayedCards[i]);
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
            Field=TotalFieldForce.P1PlayedCards;
            gCount++;
            GameObject.Find("GText").GetComponent<TextMeshProUGUI>().text=gCount.ToString();
        }else if(card.GetComponent<Card>().whichField==Card.fields.P2){//Si el campo es de P2 manda la carta al cementerio de P2
            GraveyardName="EnemyGraveyard";
            Field=TotalFieldForce.P2PlayedCards;
            egCount++;
            GameObject.Find("EGText").GetComponent<TextMeshProUGUI>().text=egCount.ToString();
        }
        card.transform.SetParent(GameObject.Find(GraveyardName).transform);
        Field.Remove(card);
        Destroy(card.GetComponent<Dragging>());//No necesitaremos esto de nuevo
    }
}
