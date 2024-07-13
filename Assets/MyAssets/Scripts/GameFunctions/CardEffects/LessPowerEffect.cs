using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de eliminar la carta de menor poder del rival
public class LessPowerEffect : MonoBehaviour, ICardEffect
{
     public void TriggerEffect(){//Elimina la carta con menos poder del campo enemigo
          //Determinando el campo a afectar
          List <GameObject> targetField=GameObject.Find("Field"+Judge.GetEnemy).GetComponent<Field>().GetCards;//Una lista de las cartas del campo enemigo
          
          if(targetField.Count>0){//Si la lista del campo enemigo tiene elementos
               GameObject Card=targetField[targetField.Count-1];//Se empieza a comparar por la ultima carta
               //Si todas las cartas tienen el mismo poder la carta eliminada es la ultima jugada
               int minTotalPower=Card.GetComponent<CardWithPower>().TotalPower;//Poder total minimo
               for(int i=0;i<targetField.Count-1;i++){//Se recorre la lista excepto el ultimo elemento pues ya lo consideramos
                    if(targetField[i].GetComponent<CardWithPower>().TotalPower<minTotalPower){//Si el poder total de la carta es menor que el que tenemos guardado
                         Card=targetField[i];//Esta sera nuestra nueva carta de menor poder
                         minTotalPower=Card.GetComponent<CardWithPower>().TotalPower;//Este sera nuestro nuevo menor poder
                    }
               }
               Graveyard.SendToGraveyard(Card);//Se envia al cementerio la carta resultante(la de menor poder)
               GFUtils.UserRead.LongWrite("Se ha eliminado a "+Card.GetComponent<Card>().CardName);
               //En caso de que todas tengan el mismo poder se envia al cementerio la ultima jugada porque se empieza a comparar por la ultima carta
          }else{
               GFUtils.UserRead.LongWrite("No se pudo activar el efecto porque el enemigo no ha jugado cartas");
          }
     }
}
