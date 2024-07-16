using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de eliminar la carta de menor poder del rival
public class LessPowerEffect : MonoBehaviour, ICardEffect
{
     public string GetEffectDescription=>"Cuando esta carta es jugada manda al cementerio a la carta de menor poder del rival";
     public void TriggerEffect(){//Elimina la carta con menos poder del campo enemigo
          //Determinando el campo a afectar
          List <GameObject> targetField=GameObject.Find("Field"+Judge.GetEnemy).GetComponent<Field>().GetCards;//Una lista de las cartas del campo enemigo
          if(targetField.Count==0){//Si la lista del campo enemigo no tiene elementos
               UserRead.Write("No se pudo activar el efecto porque el enemigo no ha jugado cartas");
               return;
          }
          GameObject minPowerCard=null;//Carta de menor poder
          int minPower=int.MaxValue;//Poder total minimo
          foreach(GameObject card in targetField){//Se recorre la lista excepto el ultimo elemento pues ya lo consideramos
               if(card.GetComponent<PowerCard>().TotalPower<minPower){//Si el poder total de la carta es menor que el que tenemos guardado
                    minPowerCard=card;//Esta sera nuestra nueva carta de menor poder
                    minPower=card.GetComponent<PowerCard>().TotalPower;//Este sera nuestro nuevo menor poder
               }
          }
          Graveyard.SendToGraveyard(minPowerCard);//Se envia al cementerio la carta resultante(la de menor poder)
          UserRead.Write("Se ha eliminado a "+minPowerCard.GetComponent<Card>().CardName);
          //En caso de que todas tengan el mismo poder se envia al cementerio la ultima jugada porque se empieza a comparar por la ultima carta
     }
}
