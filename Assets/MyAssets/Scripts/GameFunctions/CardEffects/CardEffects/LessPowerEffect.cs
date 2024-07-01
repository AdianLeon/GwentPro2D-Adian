using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de eliminar la carta de menor poder del rival
public class LessPowerEffect : MonoBehaviour, ICardEffect, IToJson
{
    public void TriggerEffect(){//Elimina la carta con menos poder del campo enemigo
        //Determinando el campo a afectar
        List <GameObject> targetField=null;//Una lista del campo enemigo
        if(this.GetComponent<Card>().WhichField==fields.P1){//Si esta carta es de P1
            targetField=TotalFieldForce.p2PlayedCards;//El campo P2 es el enemigo
        }else if(this.GetComponent<Card>().WhichField==fields.P2){//Si esta carta es de P2
            targetField=TotalFieldForce.p1PlayedCards;//El campo P1 es el enemigo
        }

        //Hallando la carta de menor poder y eliminandola
        if(targetField.Count!=0){//Si la lista del campo enemigo tiene elementos
            GameObject Card=targetField[targetField.Count-1];//Se empieza a comparar por la ultima carta
            //Si todas las cartas tienen el mismo poder la carta eliminada es la ultima jugada
            int minTotalPower=Card.GetComponent<CardWithPower>().power+Card.GetComponent<CardWithPower>().AddedPower;//Poder total minimo
            for(int i=0;i<targetField.Count-1;i++){//Se recorre la lista excepto el ultimo elemento pues ya lo consideramos
                if(targetField[i].GetComponent<CardWithPower>().power+targetField[i].GetComponent<CardWithPower>().AddedPower<minTotalPower){//Si el poder total de la carta es menor que el que tenemos guardado
                    Card=targetField[i];//Esta sera nuestra nueva carta de menor poder
                    minTotalPower=Card.GetComponent<CardWithPower>().power+Card.GetComponent<CardWithPower>().AddedPower;//Este sera nuestro nuevo menor poder
                }
            }
            Graveyard.ToGraveyard(Card);//Se envia al cementerio la carta resultante(la de menor poder)
            RoundPoints.URLongWrite("Se ha eliminado a "+Card.GetComponent<Card>().cardName);
            //En caso de que todas tengan el mismo poder se envia al cementerio la ultima jugada porque se empieza a comparar por la ultima carta
            TotalFieldForce.UpdateForce();//Se actualizan las fuerzas de los jugadores
        }else{
            RoundPoints.URLongWrite("No se pudo activar el efecto porque el enemigo no ha jugado cartas");
        }
    }
}
