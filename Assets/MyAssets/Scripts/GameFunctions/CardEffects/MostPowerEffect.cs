using System.Linq;
using UnityEngine;
//Script para el efecto de eliminar la carta de mayor poder del campo
public class MostPowerEffect : MonoBehaviour, ICardEffect
{
     public string GetEffectDescription => "Cuando esta carta es jugada manda al cementerio a la carta de mayor poder (no se incluye a si misma)";
     public void TriggerEffect()
     {//Elimina la carta con mas poder del campo (sin incluir la carta que activa el efecto)
          if (Field.PlayedCardsWithoutWeathers.Count == 1)
          {
               UserRead.Write("No se pudo activar el efecto porque no se han jugado cartas");
               return;
          }
          Transform location = transform.parent;//Guardamos donde esta la carta efecto
          transform.SetParent(GameObject.Find("Trash").transform);//Quitamos la carta efecto del campo

          //Calculamos el poder maximo entre todas las cartas
          int maxPower = Field.PlayedCardsWithoutWeathers.Select(card => card.TotalPower).Max();
          //Escogemos la primera carta que cumpla ese poder maximo entre las cartas del rival
          PowerCard maxPowerCard = Field.EnemyPlayedCards.FirstOrDefault(card => card.TotalPower == maxPower);
          if (maxPowerCard == null)
          {//Si no encontramos la carta de poder maximo entre las jugadas por el rival la buscamos en nuestro campo donde si o si tiene que estar
               maxPowerCard = Field.PlayerPlayedCards.First(card => card.TotalPower == maxPower);
          }

          Graveyard.SendToGraveyard(maxPowerCard);//Se envia al cementerio
          UserRead.Write("Se ha eliminado a " + maxPowerCard.CardName);

          transform.SetParent(location);//Devolvemos la carta efecto a su posicion original
     }
}
