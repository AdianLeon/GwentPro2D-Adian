using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de eliminar la carta de mayor poder del campo
public class MostPowerEffect : MonoBehaviour, ICardEffect
{
     public void TriggerEffect(){//Elimina la carta con mas poder del campo (sin incluir la carta que activa el efecto)
          Transform location=this.transform.parent;//Guardamos donde esta la carta
          this.transform.SetParent(GameObject.Find("Hand"+this.GetComponent<Card>().WhichPlayer).transform);//Quitamos la carta del campo

          List<GameObject> field=Field.PlayedCardsWithoutWeathers;

          GameObject chosenCard=null;//Carta escogida
          int chosenCardPower=int.MinValue;//Valor de su poder

          foreach(GameObject card in field){
               if(card.GetComponent<CardWithPower>().TotalPower>chosenCardPower){//Si alguna de las cartas de ambos campos es mayor en poder total que el guardado
                    chosenCard=card;//Esa es la nueva elegida
                    chosenCardPower=card.GetComponent<CardWithPower>().TotalPower;//Recordamos su poder
               }
          }
          if(chosenCard!=null){//Si elegimos una carta
               Graveyard.SendToGraveyard(chosenCard);//Se envia al cementerio
               GFUtils.UserRead.LongWrite("Se ha eliminado a "+chosenCard.GetComponent<Card>().CardName);
          }else{
               GFUtils.UserRead.LongWrite("No se pudo activar el efecto porque no se han jugado cartas");
          }

          this.transform.SetParent(location);//Devolvemos la carta a su posicion original
     }     
}
