using System.Linq;
using UnityEngine;
//Script para el efecto del promedio
public class PromEffect : MonoBehaviour, ICardEffect
{
     public string GetEffectDescription => "Cuando esta carta es jugada su poder se convierte en el promedio del poder total de todas las cartas del campo";
     public void TriggerEffect()
     {//Iguala el poder de la carta jugada al promedio del poder total de todas las cartas con poder del campo

          if (Field.PlayedCardsWithoutWeathers.Count == 1)
          {
               UserRead.Write("No se pudo activar el efecto porque no se han jugado cartas");
               return;
          }
          Transform location = transform.parent;//Guardamos donde esta la carta efecto
          transform.SetParent(GameObject.Find("Trash").transform);//Quitamos la carta efecto del campo

          //Promedio del poder total de todas las cartas del campo
          int average = (int)Field.PlayedCardsWithoutWeathers.Select(card => card.TotalPower).Average();

          gameObject.GetComponent<PowerCard>().Power = average;//El poder de la carta jugada es el promedio del total de poder de todas las cartas en el campo
          UserRead.Write("El total de poder en el campo es " + Field.PlayedCardsWithoutWeathers.Select(card => card.TotalPower).Sum() + " y hay " + Field.PlayedCardsWithoutWeathers.Count + " cartas. El poder promedio es de: " + average);

          transform.SetParent(location);//Devolvemos la carta efecto a su posicion original
     }
}
