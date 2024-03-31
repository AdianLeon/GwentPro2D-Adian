using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//Script para algunos efectos cuando se pase el mouse por encima de la carta
public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Objetos a utilizar
    public GameObject card;
    public static string cardName;
    void Start(){
        card=this.gameObject;
    }
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        LoadInfo();
        if(card.GetComponent<Dragging>()!=null){
            if(!Dragging.onDrag && card.transform.parent==card.GetComponent<Dragging>().hand.transform)
                Effects.ZonesGlow(card);
        }
        cardName=card.name;
        card.GetComponent<Image>().color=new Color (0.75f,0.75f,0.75f,1);
    }

    public void OnPointerExit(PointerEventData eventData){//Se activa cuando el mouse sale de la carta
        if(!Dragging.onDrag && card.GetComponent<Dragging>()!=null){
            if(card.GetComponent<CanvasGroup>().blocksRaycasts==true)
                Effects.OffZonesGlow();
        }
        cardName="None";
        card.GetComponent<Image>().color=new Color (1,1,1,1);
    }
    public void LoadInfo(){
        //Poniendo el sprite de la carta en el objeto gigante de la izquierda de la pantalla
        GameObject.Find("CardPreview").GetComponent<Image>().sprite=card.GetComponent<Image>().sprite;
        Card c=card.GetComponent<Card>();
        if(c!=null){
            if(c.power!=0){
                GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=c.power.ToString();
            }else{
                GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
            }
            if(c.addedPower>0){
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="+"+c.addedPower.ToString();
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().color=new Color(0,0,1,1);
            }else if(c.addedPower<0){
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text=c.addedPower.ToString();
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().color=new Color(1,0,0,1);
            }else{
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
            }
            GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text=c.cardRealName;
            GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>().text=c.description;
            if(c.effectDescription.Length>0){
                RoundPoints.URWrite(c.effectDescription);
            }else{
                if(TurnManager.CardsPlayed==0){
                    RoundPoints.URWrite("Turno de P"+TurnManager.PlayerTurn.ToString());
                }else{
                    if(!TurnManager.lastTurn)
                        RoundPoints.URWrite("Presiona espacio para pasar el turno");
                }
            }
        }else{
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
            GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>().text="";
        }
    }
}
