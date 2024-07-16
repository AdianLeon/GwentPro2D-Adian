using UnityEngine;
//Script para el efecto de eliminar la carta de mayor poder del campo
public class MostPowerEffect : MonoBehaviour, ICardEffect
{
     public string GetEffectDescription=>"Cuando esta carta es jugada manda al cementerio a la carta de mayor poder (no se incluye a si misma)";
     public void TriggerEffect(){//Elimina la carta con mas poder del campo (sin incluir la carta que activa el efecto)
          Transform location=transform.parent;//Guardamos donde esta la carta efecto
          transform.SetParent(GameObject.Find("Trash").transform);//Quitamos la carta efecto del campo
          GameObject chosenCard=null;//Carta escogida
          int chosenCardPower=int.MinValue;//Valor de su poder

          foreach(GameObject card in Field.PlayedCardsWithoutWeathers){
               if(card.GetComponent<PowerCard>().TotalPower>=chosenCardPower){//Si alguna de las cartas de ambos campos es mayor o igual en poder total que el guardado
                    chosenCard=card;//Esa es la nueva elegida
                    chosenCardPower=card.GetComponent<PowerCard>().TotalPower;//Recordamos su poder
               }
          }
          if(chosenCard!=null){//Si elegimos una carta
               Graveyard.SendToGraveyard(chosenCard);//Se envia al cementerio
               UserRead.Write("Se ha eliminado a "+chosenCard.GetComponent<Card>().CardName);
          }else{
               UserRead.Write("No se pudo activar el efecto porque no se han jugado cartas");
          }

          transform.SetParent(location);//Devolvemos la carta efecto a su posicion original
     }     
}
