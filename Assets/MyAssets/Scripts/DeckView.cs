using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//Script copia de CardView pero para mostrar las cartas en grande en el menu Deck
public class DeckView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject card;   
    void Start(){
        card=this.gameObject;
    }
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        Card c=card.GetComponent<Card>();
        Dragging d=card.GetComponent<Dragging>();
        
        if(d!=null){//Si posee componente dragging (no es el lider)
            d.isDraggable=false;//Desactivamos que se pueda arrastrar
            GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
            if(d.cardType==Dragging.rank.Melee){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[M]";
            }else if(d.cardType==Dragging.rank.Ranged){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[R]";
            }else if(d.cardType==Dragging.rank.Siege){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[S]";
            }else if(d.cardType==Dragging.rank.Aumento){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[A]";
            }else if(d.cardType==Dragging.rank.Clima){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[C]";
                if(c.id==4 || c.id==5){//Si es despeje
                    GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[D]";
                }
            }else if(d.cardType==Dragging.rank.Bait){
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[S]";
            }
        }else if(c.cardRealName=="Gru"){//Si la carta es el lider
                GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[L]";
        }
        if(c!=null){
            //Quality y Image
            GameObject.Find("Quality").GetComponent<Image>().sprite=c.qualitySprite;
            GameObject.Find("CardPreview").GetComponent<Image>().sprite=c.artwork;

            //Colors
            GameObject.Find("BackGroundCard").GetComponent<Image>().color=c.cardColor;
            GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=c.cardColor;
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color=c.cardColor;
    
            //Power
            if(c.power!=0){
                GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=c.power.ToString();
                GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
            }else{
                GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
                GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);
            }
            if(c.id==9 || c.id==10){//Senuelo o Minion
                GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=c.power.ToString();
                GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
            }
            //Name
            GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text=c.cardRealName;
            //Description
            GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>().text=c.description;
            //EffectDescription
            if(c.effectDescription.Length>0){//Si tiene descripcion de efecto
                GameObject.Find("Effect Description").GetComponent<TextMeshProUGUI>().text=c.effectDescription;
            }else{//Caso contrario para evitar dejar el efecto escrito en el objeto; se escribe que no tiene efecto
                GameObject.Find("Effect Description").GetComponent<TextMeshProUGUI>().text="Esta carta no tiene efecto";
            }
        }
        card.GetComponent<Image>().color=new Color (0.75f,0.75f,0.75f,1);//La carta se sombrea cuando pasamos por encima
    }

    public void OnPointerExit(PointerEventData eventData){
        card.GetComponent<Image>().color=new Color (1,1,1,1);//La carta se dessombrea cuando salimos de encima de ella
    }
}
