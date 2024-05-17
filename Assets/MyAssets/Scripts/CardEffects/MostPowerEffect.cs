using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de eliminar la carta de mayor poder del campo
public class MostPowerEffect : CardEffect
{
    override public void TriggerEffect(){//Elimina la carta con mas poder del campo
        
        //La carta todavia no se ha anadido a TotalFieldForce
        GameObject ChosenCard=null;
        GameObject CardP1=null;//Esta sera la carta de mayor poder de P1
        GameObject CardP2=null;//Esta sera la carta de mayor poder de P2
        int cardP1TotalPower=int.MinValue;//Este sera el poder de CardP1
        int cardP2TotalPower=int.MinValue;//Este sera el poder de CardP2
        
        GetMostPowerInField(TotalFieldForce.P1PlayedCards,out CardP1,out cardP1TotalPower);
        GetMostPowerInField(TotalFieldForce.P2PlayedCards,out CardP2,out cardP2TotalPower);
        //Tenemos las cartas de mayor poder de ambos campos

        ChosenCard=GetMostPowerCard(CardP1,cardP1TotalPower,CardP2,cardP2TotalPower);
        
        if(ChosenCard!=null){//Si elegimos una carta
            Graveyard.ToGraveyard(ChosenCard);//Se envia al cementerio
            RoundPoints.URLongWrite("Se ha eliminado a "+ChosenCard.GetComponent<Card>().cardRealName);
            TotalFieldForce.UpdateForce();//Se actualiza la fuerza del campo
        }else{
            RoundPoints.URLongWrite("No se pudo activar el efecto porque no se han jugado cartas");
        }
    }
    private static void GetMostPowerInField(List<GameObject> field,out GameObject Card,out int cardTotalPower){//Elige la carta de mayor poder
        if(field.Count!=0){//Si se han jugado cartas en el campo 1
            Card=field[field.Count-1];//La carta de mayor poder es la ultima jugada
            cardTotalPower=Card.GetComponent<UnitCard>().power+Card.GetComponent<UnitCard>().addedPower;//Poder de la ultima carta jugada
            for(int i=0;i<field.Count-1;i++){//Comparamos todas las cartas excepto la ultima pues ya la consideramos
                if(field[i].GetComponent<UnitCard>().power+field[i].GetComponent<UnitCard>().addedPower>cardTotalPower){//Si el poder es mayor
                    Card=field[i];//Tenemos una nueva carta de mayor poder
                    cardTotalPower=Card.GetComponent<UnitCard>().power+Card.GetComponent<UnitCard>().addedPower;//Actualizamos el mayor poder
                }
            }
        }else{
            Card=null;
            cardTotalPower=int.MinValue;
        }
    }
    private GameObject GetMostPowerCard(GameObject CardP1,int cardP1TotalPower,GameObject CardP2,int cardP2TotalPower){//Devuelve la carta de mayor poder entre las dos de cada campo
        GameObject ChosenCard=null;
        if(cardP1TotalPower>cardP2TotalPower){//Si la de mayor poder es de P1
            ChosenCard=CardP1;//La carta elegida es la de P1
        }else if(cardP1TotalPower<cardP2TotalPower){//Si la de mayor poder es de P2
            ChosenCard=CardP2;//La carta elegida es la de P2
        }else{//Si tienen igual poder la carta elegida es la del rival
            if(this.GetComponent<Card>().whichField==Card.fields.P1){//Si el Macho jugado es de P1
                ChosenCard=CardP2;//La carta elegida es la de P2
            }else if(this.GetComponent<Card>().whichField==Card.fields.P2){//Si el Macho jugado es de P2
                ChosenCard=CardP1;//La carta elegida es la de P1
            }
        }
        return ChosenCard;
    }
}
