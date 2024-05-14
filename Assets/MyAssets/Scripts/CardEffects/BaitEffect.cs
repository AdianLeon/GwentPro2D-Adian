using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitEffect : CardEffect
{
    public void SwapEffect(){
        //La carta con la que debemos intercambiar el senuelo esta guardada en CardView.selectedCard
        //Si esta carta no es de clima ni de oro
        if(CardView.selectedCard.GetComponent<Card>().whichZone!=Card.zones.Weather && CardView.selectedCard.GetComponent<UnitCard>().wichQuality!=UnitCard.quality.Gold){
            GameObject placehold=new GameObject();//Creamos un objeto auxiliar para saber donde esta el senuelo
            placehold.transform.SetParent(this.transform.parent);
            LayoutElement le=placehold.AddComponent<LayoutElement>();
            placehold.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            this.transform.SetParent(CardView.selectedCard.transform.parent);//El senuelo se pone donde esta la carta seleccionada
            this.transform.SetSiblingIndex(CardView.selectedCard.transform.GetSiblingIndex());

            CardView.selectedCard.transform.SetParent(placehold.transform.parent);//La carta seleccionada se pone donde esta el objeto auxiliar
            CardView.selectedCard.transform.SetSiblingIndex(placehold.transform.GetSiblingIndex());
            Destroy(placehold);//Se destruye

            CardView.selectedCard.GetComponent<Dragging>().isDraggable=true;//La escogida ahora es arrastrable como cualquier otra de la mano
            TotalFieldForce.RemoveCard(CardView.selectedCard);//Se quita selectedCard de las cartas jugadas
            TurnManager.PlayedCards.Remove(CardView.selectedCard);
            for(int j=0;j<CardView.selectedCard.GetComponent<UnitCard>().affected.Length;j++){
            //Deshace el efecto de clima cuando la carta vuelve a la mano, el senuelo recibira el clima como consecuencia de la llamada de UpdateClima
                if(CardView.selectedCard.GetComponent<UnitCard>().affected[j]){//Si esta afectado, se deshace
                    CardView.selectedCard.GetComponent<UnitCard>().affected[j]=false;
                    CardView.selectedCard.GetComponent<UnitCard>().addedPower++;
                }
            }
        }
    }
}