using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostPowerEffect : CardEffect
{
    override public void TriggerEffect(){//Elimina la carta con mas poder del campo
        
        //La carta todavia no se ha anadido a TotalFieldForce
        GameObject Card=null;
        GameObject CardP1=null;//Esta sera la carta de mayor poder de P1
        GameObject CardP2=null;//Esta sera la carta de mayor poder de P2
        int cardP1TotalPower=int.MinValue;//Este sera el poder de P1
        int cardP2TotalPower=int.MinValue;//Este sera el poder de P2
        
        if(TotalFieldForce.P1PlayedCards.Count!=0){//Si se han jugado cartas en el campo 1
            CardP1=TotalFieldForce.P1PlayedCards[TotalFieldForce.P1PlayedCards.Count-1];//La carta de mayor poder es la ultima jugada
            cardP1TotalPower=CardP1.GetComponent<Card>().power+CardP1.GetComponent<Card>().addedPower;//Poder de la ultima carta jugada
            for(int i=0;i<TotalFieldForce.P1PlayedCards.Count-1;i++){//Comparamos todas las cartas excepto la ultima pues ya la consideramos
                if(TotalFieldForce.P1PlayedCards[i].GetComponent<Card>().power+TotalFieldForce.P1PlayedCards[i].GetComponent<Card>().addedPower>cardP1TotalPower){//Si el poder es mayor
                    CardP1=TotalFieldForce.P1PlayedCards[i];//Tenemos una nueva carta de mayor poder
                    cardP1TotalPower=CardP1.GetComponent<Card>().power+CardP1.GetComponent<Card>().addedPower;//Actualizamos el mayor poder
                }
            }
        }
        if(TotalFieldForce.P2PlayedCards.Count!=0){//Si se han jugado cartas en el campo 2
            CardP2=TotalFieldForce.P2PlayedCards[TotalFieldForce.P2PlayedCards.Count-1];//La carta de mayor poder es la ultima jugada
            cardP2TotalPower=CardP2.GetComponent<Card>().power+CardP2.GetComponent<Card>().addedPower;//Poder de la ultima carta jugada
            for(int i=0;i<TotalFieldForce.P2PlayedCards.Count-1;i++){//Comparamos todas las cartas excepto la ultima pues ya la consideramos
                if(TotalFieldForce.P2PlayedCards[i].GetComponent<Card>().power+TotalFieldForce.P2PlayedCards[i].GetComponent<Card>().addedPower>cardP2TotalPower){//Si el poder es mayor
                    CardP2=TotalFieldForce.P2PlayedCards[i];//Tenemos una nueva carta de mayor poder
                    cardP2TotalPower=CardP2.GetComponent<Card>().power+CardP2.GetComponent<Card>().addedPower;//Actualizamos el mayor poder
                }
            }
        }
        //Tenemos las cartas de mayor poder de ambos campos
        if(cardP1TotalPower>cardP2TotalPower){//Si la de mayor poder es de P1
            Card=CardP1;//La carta elegida es la de P1
        }else if(cardP1TotalPower<cardP2TotalPower){//Si la de mayor poder es de P2
            Card=CardP2;//La carta elegida es la de P2
        }else{//Si tienen igual poder la carta elegida es la del rival
            if(this.GetComponent<Dragging>().whichField==Dragging.fields.P1){//Si el Macho jugado es de P1
                Card=CardP2;//La carta elegida es la de P2
            }else if(this.GetComponent<Dragging>().whichField==Dragging.fields.P2){//Si el Macho jugado es de P2
                Card=CardP1;//La carta elegida es la de P1
            }
        }
        if(Card!=null)//Si elegimos una carta
            Graveyard.ToGraveyard(Card);//Se envia al cementerio
        TotalFieldForce.UpdateForce();//Se actualiza la fuerza del campo
    }
}
