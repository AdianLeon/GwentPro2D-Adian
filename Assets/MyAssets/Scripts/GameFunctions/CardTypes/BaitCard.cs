using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas senuelo
public class BaitCard : CardWithPower, IAffectable, IShowZone
{
    List<string> affectedByWeathers=new List<string>();
    public List<string> AffectedByWeathers{get=>affectedByWeathers; set=>affectedByWeathers=value;}
    public override Color GetCardViewColor(){return new Color(0.8f,0.5f,0.7f);}
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[S]";
    }
    public void ShowZone(){
        //Oscurece las cartas con las que el senuelo no se puede intercambiar
        foreach(GameObject cardPlayed in TurnManager.playedCards){
            if(cardPlayed.GetComponent<IAffectable>()==null || cardPlayed.GetComponent<Card>().WhichField!=this.GetComponent<Card>().WhichField){
                //Si no es afectable o si no coincide con el campo del senuelo
                cardPlayed.GetComponent<Image>().color=new Color (0.5f,0.5f,0.5f,1);
            }
        }
    }
}
