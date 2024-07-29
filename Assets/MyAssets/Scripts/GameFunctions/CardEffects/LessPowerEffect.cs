using System.Linq;
using UnityEngine;
//Script para el efecto de eliminar la carta de menor poder del rival
public class LessPowerEffect : MonoBehaviour, ICardEffect
{
     public string GetEffectDescription => "Cuando esta carta es jugada manda al cementerio a la carta de menor poder del rival";
     public void TriggerEffect()
     {//Si hay cartas jugadas en el campo enemigo, selecciona la de menor poder y la envia al cementerio
          if (Field.EnemyCards.Count() == 0) { UserRead.Write("No se pudo activar el efecto porque el enemigo no ha jugado cartas"); return; }
          //Buscamos entre las cartas la de menor poder
          PowerCard minPowerCard = Field.EnemyCards.MinBy(card => card.TotalPower);
          Graveyard.SendToGraveyard(minPowerCard);//Se envia al cementerio la carta resultante(la de menor poder)
          UserRead.Write("Se ha eliminado a " + minPowerCard.CardName);
     }
}
