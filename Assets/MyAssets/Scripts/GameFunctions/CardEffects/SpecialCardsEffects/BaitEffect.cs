using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para las cartas senuelo
public class BaitEffect : MonoBehaviour, ICardEffect
{
    //Este efecto funciona diferente al resto, en vez de llamar a TriggerEffect
    //es mucho mas conveniente que desde que se coloca la carta en el campo y vuelva a su lugar correspondiente
    //en la mano se llame a SwapEffect con la carta que esta debajo del puntero (CardView.selectedCard)
    public void TriggerEffect(){}
    public void SwapConditions(){
        //Si no es afectable
        if(CardView.selectedCard.GetComponent<IAffectable>()==null){return;}
        //Si sus campos no coinciden
        if(CardView.selectedCard.GetComponent<Card>().WhichField!=this.GetComponent<Card>().WhichField){return;}
        //Si la carta sobre la que estamos esta en el cementerio
        if(CardView.selectedCard.transform.parent.gameObject==GameObject.Find("GraveyardP1") || CardView.selectedCard.transform.parent.gameObject==GameObject.Find("GraveyardP2")){return;}
        //Si la carta con la que intercambiaremos el senuelo es otro senuelo
        if(CardView.selectedCard.GetComponent<BaitCard>()!=null){return;}

        Debug.Log("Se activa el efecto senuelo: "+this.GetComponent<BaitEffect>());
        this.GetComponent<BaitEffect>().SwapEffect();//Usa el efecto del senuelo
    }
    public void SwapEffect(){
        GameObject placehold=new GameObject();//Creamos un objeto auxiliar para saber donde esta el senuelo
        placehold.transform.SetParent(this.transform.parent);
        LayoutElement le=placehold.AddComponent<LayoutElement>();
        placehold.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        this.transform.SetParent(CardView.selectedCard.transform.parent);//El senuelo se pone donde esta la carta seleccionada
        this.transform.SetSiblingIndex(CardView.selectedCard.transform.GetSiblingIndex());

        CardView.selectedCard.transform.SetParent(placehold.transform.parent);//selectedCard se pone donde esta el objeto auxiliar
        CardView.selectedCard.transform.SetSiblingIndex(placehold.transform.GetSiblingIndex());

        placehold.transform.SetParent(GameObject.Find("Trash").transform);//Se destruye el objeto auxiliar
        Destroy(placehold);

        CardView.selectedCard.GetComponent<Dragging>().IsDraggable=true;//selectedCard ahora es arrastrable como cualquier otra de la mano

        TotalFieldForce.RemoveCard(CardView.selectedCard);//Se quita selectedCard de las cartas jugadas
        TurnManager.playedCards.Remove(CardView.selectedCard);

        if(CardView.selectedCard.GetComponent<MultiplyEffect>()!=null){//Si selectedCard tiene efecto de multiplicar
            CardView.selectedCard.GetComponent<UnitCard>().power=CardView.selectedCard.GetComponent<MultiplyEffect>().originalPower;
        }
        if(CardView.selectedCard.GetComponent<PromEffect>()!=null){//Si selectedCard tiene efecto de promedio
            CardView.selectedCard.GetComponent<UnitCard>().power=0;
        }
        //Deshace el efecto de clima cuando la carta vuelve a la mano, el senuelo recibira el clima como consecuencia de la llamada de UpdateClima
        ClearWeatherEffect.ClearCardOfWeathers(CardView.selectedCard);
    }
}