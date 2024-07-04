using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
//Script para las cartas senuelo
public class BaitCard : CardWithPower, IAffectable, IShowZone
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
        foreach(GameObject cardPlayed in Board.PlayedCards){
            if(cardPlayed.GetComponent<IAffectable>()==null || cardPlayed.GetComponent<Card>().WhichField!=WhichField || cardPlayed.GetComponent<BaitCard>()!=null){
                //Si no es afectable, no coincide con el campo del senuelo o es otro senuelo
                cardPlayed.GetComponent<Card>().OnGlow();//Se le activa el glow (oscurece la carta)
            }
        }
    }

    //Este efecto funciona diferente al resto, en vez de llamar a TriggerEffect
    //es mucho mas conveniente que desde que se coloca la carta en el campo y vuelva a su lugar correspondiente
    //en la mano se llame a SwapEffect con la carta que esta debajo del puntero (CardView.GetSelectedCard)
    public void SwapConditions(){
        //Si no es afectable
        if(CardView.GetSelectedCard.GetComponent<IAffectable>()==null){return;}
        //Si sus campos no coinciden
        if(CardView.GetSelectedCard.GetComponent<Card>().WhichField!=this.GetComponent<Card>().WhichField){return;}
        //Si la carta sobre la que estamos esta en el cementerio
        if(CardView.GetSelectedCard.transform.parent.gameObject==GameObject.Find("GraveyardP1") || CardView.GetSelectedCard.transform.parent.gameObject==GameObject.Find("GraveyardP2")){return;}
        //Si la carta con la que intercambiaremos el senuelo es otro senuelo
        if(CardView.GetSelectedCard.GetComponent<BaitCard>()!=null){return;}

        Debug.Log("Se activa el efecto senuelo: "+this.GetComponent<BaitCard>());
        SwapEffect();//Usa el efecto del senuelo
    }
    public void SwapEffect(){
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

        if(CardView.GetSelectedCard.GetComponent<MultiplyEffect>()!=null){//Si GetSelectedCard tiene efecto de multiplicar
            CardView.GetSelectedCard.GetComponent<UnitCard>().power=CardView.GetSelectedCard.GetComponent<MultiplyEffect>().OriginalPower;
        }
        if(CardView.GetSelectedCard.GetComponent<PromEffect>()!=null){//Si GetSelectedCard tiene efecto de promedio
            CardView.GetSelectedCard.GetComponent<UnitCard>().power=0;
        }
        //Deshace el efecto de clima cuando la carta vuelve a la mano, el senuelo recibira el clima como consecuencia de la llamada de UpdateClima
        ClearWeatherCard.ClearCardOfWeathers(CardView.GetSelectedCard);
    }
}
