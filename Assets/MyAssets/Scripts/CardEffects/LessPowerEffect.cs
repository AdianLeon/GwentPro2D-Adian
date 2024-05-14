using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessPowerEffect : CardEffect
{
    override public void TriggerEffect(){//Elimina la carta con menos poder del campo enemigo
        //Determinando el campo a afectar
        List <GameObject> targetField=new List <GameObject>();//Una lista del campo enemigo
        if(this.GetComponent<Card>().whichField==Card.fields.P1){//Si Vector es jugado por P1
            targetField=TotalFieldForce.P2PlayedCards;//El campo P2 es el enemigo
        }else if(this.GetComponent<Card>().whichField==Card.fields.P2){//Si Vector es jugado por P2
            targetField=TotalFieldForce.P1PlayedCards;//El campo P1 es el enemigo
        }

        //Hallando la carta de menor poder y eliminandola
        if(targetField.Count!=0){//Si la lista del campo enemigo tiene elementos
            GameObject Card=targetField[targetField.Count-1];//Se empieza a comparar por la ultima carta
            //Si todas las cartas tienen el mismo poder la carta eliminada es la ultima jugada
            int minTotalPower=Card.GetComponent<UnitCard>().power+Card.GetComponent<UnitCard>().addedPower;//Poder total minimo
            for(int i=0;i<targetField.Count-1;i++){//Se recorre la lista excepto el ultimo elemento pues ya lo consideramos
                if(targetField[i].GetComponent<UnitCard>().power+targetField[i].GetComponent<UnitCard>().addedPower<minTotalPower){//Si el poder total de la carta es menor que el que tenemos guardada
                    Card=targetField[i];//Esta sera nuestra nueva carta de menor poder
                    minTotalPower=Card.GetComponent<UnitCard>().power+Card.GetComponent<UnitCard>().addedPower;//Este sera nuestro nuevo menor poder
                }
            }
            Graveyard.ToGraveyard(Card);//Se envia al cementerio la carta resultante(la de menor poder)
            TotalFieldForce.UpdateForce();
        }
    }
}
