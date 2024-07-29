using System.Linq;
using UnityEngine;
//Script para el efecto de eliminar la carta de mayor poder del campo
public class MostPowerEffect : MonoBehaviour, ICardEffect
{
     public string GetEffectDescription => "Cuando esta carta es jugada manda al cementerio a la carta de mayor poder (no se incluye a si misma)";
     public void TriggerEffect()
     {//Si hay cartas jugadas en el campo elimina la carta con mas poder (sin incluir la que activa el efecto)
          if (Field.PlayedFieldCards.Count() == 1) { UserRead.Write("No se pudo activar el efecto porque no se han jugado cartas"); return; }
          //Escogemos la carta de mayor poder exluyendo la que esta activando el efecto
          PowerCard maxPowerCard = Field.PlayedFieldCards.Where(card => card != gameObject.GetComponent<PowerCard>()).MaxBy(card => card.TotalPower);
          Graveyard.SendToGraveyard(maxPowerCard);//Se envia al cementerio
          UserRead.Write("Se ha eliminado a " + maxPowerCard.CardName);
     }
}
