using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas senuelo
public class BaitCard : CardWithPower, ISpecialCard, IAffectable, IShowZone
{
    private List<string> affectedByWeathers=new List<string>();
    public List<string> AffectedByWeathers{get=>affectedByWeathers; set=>affectedByWeathers=value;}
    public override Color GetCardViewColor(){return new Color(0.8f,0.5f,0.7f);}
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[S]";
    }
    private bool SwapConditions(GameObject cardToSwap, bool showMessage=false){//Si no esta en la mano ni en el cementerio, es afectable, coincide con el campo del senuelo y no es otro senuelo
        if(cardToSwap.GetComponent<Dragging>().IsOnHand){
            if(showMessage){GFUtils.UserRead.LongWrite("No es valido usar un senuelo sobre la mano");}
            return false;
        }
        if(cardToSwap.transform.parent.gameObject==GameObject.Find("GraveyardP1") || cardToSwap.transform.parent.gameObject==GameObject.Find("GraveyardP2")){
            if(showMessage){GFUtils.UserRead.LongWrite("No es valido usar un senuelo en un cementerio");}
            return false;
        }
        if(cardToSwap.GetComponent<Card>().WhichPlayer!=WhichPlayer){
            if(showMessage){GFUtils.UserRead.LongWrite("Esa carta no esta en tu campo");}
            return false;
        }
        if(cardToSwap.GetComponent<BaitCard>()!=null){
            if(showMessage){GFUtils.UserRead.LongWrite("No es valido usar un senuelo sobre otro senuelo");}
            return false;
        }
        if(cardToSwap.GetComponent<IAffectable>()==null){
            if(showMessage){GFUtils.UserRead.LongWrite("Esa carta no es afectable por el senuelo");}
            return false;
        }
        return true;
    }
    public void ShowZone(){
        //Activa el glow (oscurece) para las cartas con las que el senuelo no se puede intercambiar
        foreach(GameObject cardPlayed in Field.AllPlayedCards){
            if(!SwapConditions(cardPlayed)){
                cardPlayed.GetComponent<Card>().OffGlow();//Se le activa el glow (oscurece la carta)
            }
        }
    }

    public override bool IsPlayable{get{//Conjunto de condiciones para que el senuelo se juegue
        if(!this.gameObject.GetComponent<Dragging>().IsOnHand){Debug.Log("El senuelo se intenta jugar pero no esta en la mano!!");return false;}
        //Si estamos encima de una carta
        if(CardView.GetSelectedCard==null){return false;}
        //Chequeamos las condiciones para intercambiar con esa carta
        return SwapConditions(CardView.GetSelectedCard,true);
    }}
    public void TriggerSpecialEffect(){
        GameObject placehold=new GameObject();//Creamos un objeto auxiliar para saber donde esta el senuelo
        placehold.transform.SetParent(this.transform.parent);
        LayoutElement le=placehold.AddComponent<LayoutElement>();
        placehold.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        this.transform.SetParent(CardView.GetSelectedCard.transform.parent);//El senuelo se pone donde esta la carta seleccionada
        this.transform.SetSiblingIndex(CardView.GetSelectedCard.transform.GetSiblingIndex());

        CardView.GetSelectedCard.transform.SetParent(placehold.transform.parent);//GetSelectedCard se pone donde esta el objeto auxiliar
        CardView.GetSelectedCard.transform.SetSiblingIndex(placehold.transform.GetSiblingIndex());

        placehold.transform.SetParent(GameObject.Find("Trash").transform);//Se destruye el objeto auxiliar
        Destroy(placehold);
        
        //Deshace el efecto de clima cuando la carta vuelve a la mano, el senuelo recibira el clima como consecuencia de la llamada de UpdateClima
        ClearWeatherCard.ClearCardOfWeathers(CardView.GetSelectedCard);
    }
}
