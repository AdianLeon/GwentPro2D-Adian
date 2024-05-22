using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para las cartas senuelo
public class BaitEffect : CardEffect
{
    //Este efecto funciona un poco diferente al resto de efectos, en vez de llamar a TriggerEffect
    //es mucho mas conveniente que desde que se coloca la carta en el campo y vuelva a su lugar correspondiente
    //en la mano y desde ahi que se llame a SwapEffect con la carta que esta debajo del puntero (CardView.selectedCard)
    public void SwapEffect(){
        //La carta con la que debemos intercambiar el senuelo esta guardada en CardView.selectedCard
        //Si esta carta no es de clima ni de oro (u otro senuelo)
        if(CardView.selectedCard.GetComponent<UnitCard>()!=null && CardView.selectedCard.GetComponent<UnitCard>().wichQuality!=UnitCard.quality.Gold){
            GameObject placehold=new GameObject();//Creamos un objeto auxiliar para saber donde esta el senuelo
            placehold.transform.SetParent(this.transform.parent);
            LayoutElement le=placehold.AddComponent<LayoutElement>();
            placehold.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            this.transform.SetParent(CardView.selectedCard.transform.parent);//El senuelo se pone donde esta la carta seleccionada
            this.transform.SetSiblingIndex(CardView.selectedCard.transform.GetSiblingIndex());

            CardView.selectedCard.transform.SetParent(placehold.transform.parent);//selectedCard se pone donde esta el objeto auxiliar
            CardView.selectedCard.transform.SetSiblingIndex(placehold.transform.GetSiblingIndex());
            Destroy(placehold);//Se destruye

            CardView.selectedCard.GetComponent<Dragging>().isDraggable=true;//selectedCard ahora es arrastrable como cualquier otra de la mano
            TotalFieldForce.RemoveCard(CardView.selectedCard);//Se quita selectedCard de las cartas jugadas
            TurnManager.playedCards.Remove(CardView.selectedCard);
            if(CardView.selectedCard.GetComponent<MultiplyEffect>()!=null){//Si selectedCard tiene efecto de multiplicar
                CardView.selectedCard.GetComponent<UnitCard>().power=CardView.selectedCard.GetComponent<MultiplyEffect>().originalPower;
            }
            //Deshace el efecto de clima cuando la carta vuelve a la mano, el senuelo recibira el clima como consecuencia de la llamada de UpdateClima
            ClearWeatherEffect.ClearCardOfWeathers(CardView.selectedCard);
        }
    }
}