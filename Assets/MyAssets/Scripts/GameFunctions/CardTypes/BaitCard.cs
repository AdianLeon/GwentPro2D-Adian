using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para las cartas senuelo
public class BaitCard : PowerCard, ISpecialCard, IAffectable
{
    public string GetEffectDescription=>"Esta carta se puede intercambiar con otra en el campo propio (solo con unidades de plata)";
    private List<WeatherCard> affectedByWeathers=new List<WeatherCard>();//Lista de cartas clima que afectan el senuelo
    public List<WeatherCard> AffectedByWeathers{get=>affectedByWeathers; set=>affectedByWeathers=value;}
    public override Color GetCardViewColor=>new Color(0.8f,0.5f,0.7f);
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[S]";
    }
    private bool SwapConditions(DraggableCard cardToSwap, bool showMessage=false){//Si no es una carta lider, no esta en la mano ni en el cementerio, coincide con el campo del senuelo, no es otro senuelo y es afectable
        if(cardToSwap.GetComponent<LeaderCard>()!=null){ if(showMessage){UserRead.Write("No es valido usar un senuelo con una carta lider");}               return false;}
        if(cardToSwap.IsOnHand){ if(showMessage){UserRead.Write("No es valido usar un senuelo sobre la mano");}                                             return false;}
        if(cardToSwap.transform.parent.GetComponent<Graveyard>()!=null){ if(showMessage){UserRead.Write("No es valido usar un senuelo en un cementerio");}  return false;}
        if(cardToSwap.WhichPlayer!=WhichPlayer){ if(showMessage){UserRead.Write("Esa carta no esta en tu campo");}                                          return false;}
        if(cardToSwap.GetComponent<BaitCard>()!=null){ if(showMessage){UserRead.Write("No es valido usar un senuelo sobre otro senuelo");}                  return false;}
        if(cardToSwap.GetComponent<IAffectable>()==null){ if(showMessage){UserRead.Write("Esa carta no es afectable por el senuelo");}                      return false;}
        return true;
    }
    public override void ShowZone(){//Activa el glow (oscurece) para las cartas con las que el senuelo no se puede intercambiar
        foreach(DraggableCard cardPlayed in Field.AllPlayedCards){
            if(!SwapConditions(cardPlayed)){
                cardPlayed.GetComponent<Card>().OffGlow();//Se le activa el glow (oscurece la carta)
            }
        }
    }
    public override bool IsPlayable{get{//Conjunto de condiciones para que el senuelo se juegue
        if(!gameObject.GetComponent<DraggableCard>().IsOnHand){
            Debug.Log("El senuelo se intenta jugar pero no esta en la mano!!");
            return false;
        }
        //Si estamos encima de una carta
        if(GetEnteredCard==null){return false;}
        //Chequeamos las condiciones para intercambiar con esa carta
        return SwapConditions(GetEnteredCard.GetComponent<DraggableCard>(),true);
    }}
    public void TriggerSpecialEffect(){//Efecto de intercambio del senuelo
        GameObject placehold=new GameObject();//Creamos un objeto auxiliar para saber donde esta el senuelo
        placehold.transform.SetParent(this.transform.parent);
        placehold.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        this.transform.SetParent(GetEnteredCard.transform.parent);//El senuelo se pone donde esta la carta seleccionada
        this.transform.SetSiblingIndex(GetEnteredCard.transform.GetSiblingIndex());

        GetEnteredCard.transform.SetParent(placehold.transform.parent);//GetEnteredCard se pone donde esta el objeto auxiliar
        GetEnteredCard.transform.SetSiblingIndex(placehold.transform.GetSiblingIndex());

        placehold.transform.SetParent(GameObject.Find("Trash").transform);//Se destruye el objeto auxiliar
        Destroy(placehold);
    }
}
