using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Script para el efecto de eliminar la carta de menor poder del rival
public class LessPowerEffect : MonoBehaviour, ICardEffect
{
     public string GetEffectDescription => "Cuando esta carta es jugada manda al cementerio a la carta de menor poder del rival";
     public void TriggerEffect()
     {
          if (Field.EnemyPlayedCards.Count == 0)
          {
               UserRead.Write("No se pudo activar el efecto porque el enemigo no ha jugado cartas");
               return;
          }
          //Encontramos el menor poder
          int minPower = Field.EnemyPlayedCards.Select(card => card.TotalPower).Min();
          //Buscamos entre las cartas una que tenga ese poder
          PowerCard minPowerCard = Field.EnemyPlayedCards.First(card => card.TotalPower == minPower);

          Graveyard.SendToGraveyard(minPowerCard);//Se envia al cementerio la carta resultante(la de menor poder)
          UserRead.Write("Se ha eliminado a " + minPowerCard.CardName);
     }
}
