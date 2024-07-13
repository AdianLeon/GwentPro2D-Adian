using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para las manos de los jugadores
public class Hand : CustomBehaviour, IContainer
{
    public List <GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}
    public static List<GameObject> PlayerHandCards{get=>GameObject.Find("Hand"+Judge.GetPlayer).GetComponent<Hand>().GetCards;}
    public static List<GameObject> EnemyHandCards{get=>GameObject.Find("Hand"+Judge.GetEnemy).GetComponent<Hand>().GetCards;}
    public override void Initialize(){}
    public override void Finish(){//Desactiva todas las manos
        GFUtils.GetRidOf(GetCards);
    }
    public override void NextUpdate(){
        while(this.transform.childCount>10){//Si una carta no cabe en la mano
            GFUtils.UserRead.LongWrite("No puedes tener mas de 10 cartas en la mano."+this.transform.GetChild(this.transform.childCount-1).gameObject.GetComponent<Card>().CardName+" se ha enviado al cementerio!");
            Graveyard.SendToGraveyard(this.transform.GetChild(this.transform.childCount-1).gameObject);//Se envia al cementerio
        }
    }
}
