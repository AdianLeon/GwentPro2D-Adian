using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para las cartas senuelo
public class BaitCard : CardWithPower, ISpecialCard, IAffectable, IShowZone
{
    private List<WeatherCard> affectedByWeathers=new List<WeatherCard>();//Lista de cartas clima que afectan el senuelo
    public List<WeatherCard> AffectedByWeathers{get=>affectedByWeathers; set=>affectedByWeathers=value;}
    public override Color GetCardViewColor(){return new Color(0.8f,0.5f,0.7f);}
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[S]";
    }
    private bool SwapConditions(GameObject cardToSwap, bool showMessage=false){//Si no es una carta lider, no esta en la mano ni en el cementerio, coincide con el campo del senuelo, no es otro senuelo y es afectable
        if(cardToSwap.GetComponent<LeaderCard>()!=null){ if(showMessage){GFUtils.UserRead.Write("No es valido usar un senuelo con una carta lider");}               return false;}
        if(cardToSwap.GetComponent<Dragging>().IsOnHand){ if(showMessage){GFUtils.UserRead.Write("No es valido usar un senuelo sobre la mano");}                    return false;}
        if(cardToSwap.transform.parent.GetComponent<Graveyard>()!=null){ if(showMessage){GFUtils.UserRead.Write("No es valido usar un senuelo en un cementerio");}  return false;}
        if(cardToSwap.GetComponent<Card>().WhichPlayer!=WhichPlayer){ if(showMessage){GFUtils.UserRead.Write("Esa carta no esta en tu campo");}                     return false;}
        if(cardToSwap.GetComponent<BaitCard>()!=null){ if(showMessage){GFUtils.UserRead.Write("No es valido usar un senuelo sobre otro senuelo");}                  return false;}
        if(cardToSwap.GetComponent<IAffectable>()==null){ if(showMessage){GFUtils.UserRead.Write("Esa carta no es afectable por el senuelo");}                      return false;}
        return true;
    }
    public override void ShowZone(){//Activa el glow (oscurece) para las cartas con las que el senuelo no se puede intercambiar
        foreach(GameObject cardPlayed in Field.AllPlayedCards){
            if(!SwapConditions(cardPlayed)){
                cardPlayed.GetComponent<Card>().OffGlow();//Se le activa el glow (oscurece la carta)
            }
        }
    }
    public override bool IsPlayable{get{//Conjunto de condiciones para que el senuelo se juegue
        if(!this.gameObject.GetComponent<Dragging>().IsOnHand){
            Debug.Log("El senuelo se intenta jugar pero no esta en la mano!!");
            return false;
        }
        //Si estamos encima de una carta
        if(CardView.GetSelectedCard==null){return false;}
        //Chequeamos las condiciones para intercambiar con esa carta
        return SwapConditions(CardView.GetSelectedCard,true);
    }}
    public void TriggerSpecialEffect(){//Efecto de intercambio del senuelo
        GameObject placehold=new GameObject();//Creamos un objeto auxiliar para saber donde esta el senuelo
        placehold.transform.SetParent(this.transform.parent);
        placehold.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        this.transform.SetParent(CardView.GetSelectedCard.transform.parent);//El senuelo se pone donde esta la carta seleccionada
        this.transform.SetSiblingIndex(CardView.GetSelectedCard.transform.GetSiblingIndex());

        CardView.GetSelectedCard.transform.SetParent(placehold.transform.parent);//GetSelectedCard se pone donde esta el objeto auxiliar
        CardView.GetSelectedCard.transform.SetSiblingIndex(placehold.transform.GetSiblingIndex());

        placehold.transform.SetParent(GameObject.Find("Trash").transform);//Se destruye el objeto auxiliar
        Destroy(placehold);
    }
}
