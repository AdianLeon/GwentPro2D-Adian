using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de eliminar la carta de menor poder del rival
public class LessPowerEffect : MonoBehaviour, ICardEffect
{
     public string GetEffectDescription => "Cuando esta carta es jugada manda al cementerio a la carta de menor poder del rival";
     public void TriggerEffect()
     {//Elimina la carta con menos poder del campo enemigo
          PowerCard minPowerCard = null;//Carta de menor poder
          int minPower = int.MaxValue;//Poder total minimo
          foreach (PowerCard card in Field.EnemyPlayedCards)
          {//Se recorre la lista excepto el ultimo elemento pues ya lo consideramos
               if (card.GetComponent<PowerCard>().TotalPower < minPower)
               {//Si el poder total de la carta es menor que el que tenemos guardado
                    minPowerCard = card;//Esta sera nuestra nueva carta de menor poder
                    minPower = card.GetComponent<PowerCard>().TotalPower;//Este sera nuestro nuevo menor poder
               }
          }
          if (minPowerCard != null)
          {
               Graveyard.SendToGraveyard(minPowerCard);//Se envia al cementerio la carta resultante(la de menor poder)
               UserRead.Write("Se ha eliminado a " + minPowerCard.CardName);
          }
          else
          {
               UserRead.Write("No se pudo activar el efecto porque el enemigo no ha jugado cartas");
          }
          //En caso de que todas tengan el mismo poder se envia al cementerio la ultima jugada porque se empieza a comparar por la ultima carta
     }
}
