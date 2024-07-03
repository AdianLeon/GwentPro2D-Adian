using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de robar una carta del deck
public class DrawOneEffect : MonoBehaviour, ICardEffect, IToJson
{
    public void TriggerEffect(){//Roba una carta del deck propio
        GameObject newCard=GameObject.Find("Deck"+this.gameObject.GetComponent<Card>().WhichField).GetComponent<Deck>().ForceDrawTopCard();
        if(newCard!=null){
            RoundPoints.LongWriteUserRead("Se ha robado una carta del deck. Es "+newCard.GetComponent<Card>().cardName);
        }else{
            RoundPoints.LongWriteUserRead("No se pudo activar el efecto porque no quedan cartas en el deck");
        }
    }
}
