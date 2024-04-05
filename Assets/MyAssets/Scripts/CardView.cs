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
    public Sprite qualitySprite;
    void Start(){
        card=this.gameObject;
        cardName="None";
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
        Card c=card.GetComponent<Card>();
        Dragging d=card.GetComponent<Dragging>();
        if(d!=null){
            if(d.cardType==Dragging.rank.Melee){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="M";
            }else if(d.cardType==Dragging.rank.Ranged){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="R";
            }else if(d.cardType==Dragging.rank.Siege){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="S";
            }else if(d.cardType==Dragging.rank.Aumento){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="A";
            }else if(d.cardType==Dragging.rank.Clima){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="C";
            }else if(d.cardType==Dragging.rank.Despeje){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="D";
            }else if(d.cardType==Dragging.rank.Senuelo){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="S";
            }else if(c.cardRealName=="Gru"){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="L";
            }else{
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="";
            }
        }
        if(c!=null){
            //Quality y Image
            GameObject.Find("Quality").GetComponent<Image>().sprite=qualitySprite;
            GameObject.Find("CardPreview").GetComponent<Image>().sprite=c.artwork;

            //Colors
            if(c.wQuality==Card.quality.Silver){
                GameObject.Find("BackGroundCard").GetComponent<Image>().color=new Color(0.8f,0.8f,0.8f,1);
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=new Color(0.8f,0.8f,0.8f,1);
                GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color=new Color(0.8f,0.8f,0.8f,1);
            }else if(c.wQuality==Card.quality.Gold){
                GameObject.Find("BackGroundCard").GetComponent<Image>().color=new Color(1,0.8f,0,1);
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=new Color(1,0.8f,0,1);
                GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color=new Color(1,0.8f,0,1);
            }else if(c.cardRealName=="Gru"){
                GameObject.Find("BackGroundCard").GetComponent<Image>().color=new Color(0.3f,0.2f,0.4f,1);
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=new Color(0.3f,0.2f,0.4f,1);
            }else if(d.cardType==Dragging.rank.Aumento){
                GameObject.Find("BackGroundCard").GetComponent<Image>().color=new Color(0.2f,0.8f,0.2f,1);
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=new Color(0.2f,0.8f,0.2f,1);
            }else if(d.cardType==Dragging.rank.Clima){
                GameObject.Find("BackGroundCard").GetComponent<Image>().color=new Color(0.8f,0.2f,0.2f,1);
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=new Color(0.8f,0.2f,0.2f,1);
            }else if(d.cardType==Dragging.rank.Senuelo){
                GameObject.Find("BackGroundCard").GetComponent<Image>().color=new Color(0.5f,0.5f,1,1);
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=new Color(0.5f,0.5f,1,1);
            }

            //Power
            if(c.power!=0){
                GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=c.power.ToString();
            }else{
                GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
            }

            //AddedPower
            if(c.addedPower>0){
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="+"+c.addedPower.ToString();
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().color=new Color(0,0,1,1);
            }else if(c.addedPower<0){
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text=c.addedPower.ToString();
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().color=new Color(1,0,0,1);
            }else{
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
            }

            //Name
            GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text=c.cardRealName;
            //Description
            GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>().text=c.description;
            //EffectDescription
            if(c.effectDescription.Length>0){
                RoundPoints.URWrite("Efecto: "+c.effectDescription);
            }else{
                //Cuando no tiene efecto en el URWrite se pone info sobre la ronda
                if(TurnManager.CardsPlayed==0 && TurnManager.lastTurn){
                    RoundPoints.URWrite("Turno de P"+TurnManager.PlayerTurn+", es el ultimo turno antes de que se acabe la ronda");
                    
                }else if(TurnManager.CardsPlayed==0){
                    RoundPoints.URWrite("Turno de P"+TurnManager.PlayerTurn);
                }else if(TurnManager.CardsPlayed!=0 && TurnManager.lastTurn){
                    RoundPoints.URWrite("Presiona espacio para acabar la ronda");
                }else{
                    RoundPoints.URWrite("Presiona espacio para pasar de turno");
                }
            }
        }
    }
}
