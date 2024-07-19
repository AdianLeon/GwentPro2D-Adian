using UnityEngine;
//Script para el efecto del promedio
public class PromEffect : MonoBehaviour, ICardEffect
{
     public string GetEffectDescription => "Cuando esta carta es jugada su poder se convierte en el promedio del poder total de todas las cartas del campo";
     public void TriggerEffect()
     {//Iguala el poder de la carta jugada al promedio del poder total de todas las cartas con poder del campo
          int total = 0;//Total de poder de todas las cartas del campo
          total += Field.P1ForceValue;//Se anade el poder del P1
          total += Field.P2ForceValue;//Se anade el poder del P2
          total -= gameObject.GetComponent<PowerCard>().TotalPower;//Quitamos del poder total el de esta carta
          int divisor = Field.PlayedCardsWithoutWeathers.Count - 1;//El divisor es el total de cartas en el campo quitando esta carta
          if (divisor > 0)
          {//Si hay cartas en el campo
               gameObject.GetComponent<PowerCard>().Power = total / divisor;//El poder de la carta jugada es el promedio del total de poder de todas las cartas en el campo
               UserRead.Write("El total de poder en el campo es " + total + " y hay " + divisor + " cartas. El poder promedio es de: " + total / divisor);
          }
          else
          {
               UserRead.Write("No se pudo activar el efecto porque no se han jugado cartas");
          }
     }
}
