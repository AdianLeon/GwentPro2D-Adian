using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Script para el efecto de multiplicar por n el poder
public class MultiplyEffect : MonoBehaviour, ICardEffect
{
    public string GetEffectDescription => "Cuando esta carta es jugada su poder es multiplicado por la cantidad de cartas iguales a ella jugadas en el campo (contandose a si misma)";
    private int originalPower;//Se guarda el poder original de la carta
    //Esto es para evitar que si se activara el efecto dos o mas veces por uso del efecto del senuelo el poder de esta carta sea demasiado alto
    public int OriginalPower => originalPower;
    void Awake()
    {
        originalPower = gameObject.GetComponent<PowerCard>().Power;
    }
    public void TriggerEffect()
    {//Multiplica por n su ataque, siendo n la cantidad de cartas iguales a ella en el campo.
        //Se cuenta cuantas cartas con el mismo nombre hay (n al menos sera 1 despues del conteo ya que la carta que llama a este efecto siempre se contara a si misma)
        int n = Field.PlayedCardsWithoutWeathers.Count(card => card.CardName == gameObject.GetComponent<Card>().CardName);

        gameObject.GetComponent<PowerCard>().Power = originalPower * n;//Se iguala el poder de la carta jugada a n veces su propio poder
        UserRead.Write("Hay " + n + " cartas iguales a " + GetComponent<Card>().CardName + " y tiene " + originalPower + " de poder. Luego del efecto posee " + GetComponent<PowerCard>().Power);
    }
}
