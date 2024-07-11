using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
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
    public void ShowZone(){
        //Activa el glow (oscurece) para las cartas con las que el senuelo no se puede intercambiar
        foreach(GameObject cardPlayed in Judge.PlayedCards){
            if(cardPlayed.GetComponent<IAffectable>()==null || cardPlayed.GetComponent<Card>().WhichField!=WhichField || cardPlayed.GetComponent<BaitCard>()!=null){
                //Si no es afectable, no coincide con el campo del senuelo o es otro senuelo
                cardPlayed.GetComponent<Card>().OffGlow();//Se le activa el glow (oscurece la carta)
            }
        }
    }

    public override bool IsPlayable{get{//Conjunto de condiciones para que el senuelo se juegue
        //Si no es afectable
        if(CardView.GetSelectedCard.GetComponent<IAffectable>()==null){return false;}
        //Si sus campos no coinciden
        if(CardView.GetSelectedCard.GetComponent<Card>().WhichField!=this.GetComponent<Card>().WhichField){return false;}
        //Si la carta sobre la que estamos esta en el cementerio
        if(CardView.GetSelectedCard.transform.parent.gameObject==GameObject.Find("GraveyardP1") || CardView.GetSelectedCard.transform.parent.gameObject==GameObject.Find("GraveyardP2")){return false;}
        //Si la carta con la que intercambiaremos el senuelo es otro senuelo
        if(CardView.GetSelectedCard.GetComponent<BaitCard>()!=null){return false;}
        //Si el senuelo no esta en la mano
        if(!this.GetComponent<Dragging>().IsOnHand){return false;}

        return true;
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
