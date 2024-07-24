using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Script para el efecto del promedio
public class PromEffect : MonoBehaviour, ICardEffect
{
     public string GetEffectDescription => "Cuando esta carta es jugada su poder se convierte en el promedio del poder total de todas las cartas del campo";
     public void TriggerEffect()
     {//Iguala el poder de la carta jugada al promedio del poder total de todas las cartas con poder del campo
          if (Field.PlayedFieldCards.Count() == 1) { UserRead.Write("No se pudo activar el efecto porque no se han jugado cartas"); return; }
          //Promedio del poder total de todas las cartas del campo
          IEnumerable<PowerCard> field = Field.PlayedFieldCards.Where(card => card != gameObject.GetComponent<PowerCard>());
          int average = (int)field.Select(card => card.TotalPower).Average();
          gameObject.GetComponent<PowerCard>().Power = average;//El poder de la carta jugada es el promedio del total de poder de todas las cartas en el campo
          UserRead.Write("El total de poder en el campo es " + field.Sum(card => card.TotalPower) + " y hay " + field.Count() + " cartas. El poder promedio es de: " + average);
     }
}
