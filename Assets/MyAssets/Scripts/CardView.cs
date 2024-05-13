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
        cardName="None";//Inicializamos la referencia del nombre de la carta a nada
    }
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        LoadInfo();//Se carga toda la informacion de esta carta en el CardView
        if(this.gameObject.GetComponent<Dragging>()!=null){
            if(!Dragging.onDrag && this.gameObject.transform.parent==this.gameObject.GetComponent<Dragging>().hand.transform)//Si no se esta arrastrando ninguna carta y ademas esta en la mano
                VisualEffects.ZonesGlow(this.gameObject);//Se ilumina la zona donde se puede soltar
        }
        cardName=this.gameObject.name;//Obtenemos la referencia a esta carta para usarla luego
        this.gameObject.GetComponent<Image>().color=new Color (0.75f,0.75f,0.75f,1);//La carta se sombrea cuando pasamos por encima
    }

    public void OnPointerExit(PointerEventData eventData){//Se activa cuando el mouse sale de la carta
        if(!Dragging.onDrag && this.gameObject.GetComponent<Dragging>()!=null){//Si no se esta arrastrando ninguna carta y el objeto tiene dragging
            if(this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts==true)//Si el objecto bloquea los raycasts
                VisualEffects.OffZonesGlow();//Se desactivan la iluminacion de todas las zonas
        }
        cardName="None";//Se pierde el nombre
        this.gameObject.GetComponent<Image>().color=new Color (1,1,1,1);//La carta se dessombrea
        if(TurnManager.CardsPlayed!=0 && !TurnManager.lastTurn){//Si se han jugado cartas y no es el ultimo turno
            RoundPoints.URWrite("Presiona espacio para pasar de turno");
        }
    }
    public void LoadInfo(){
        //Poniendo informacion de la carta en el objeto gigante de la izquierda de la pantalla
        Card c=this.gameObject.GetComponent<Card>();//Componente Card de la carta
        Dragging d=this.gameObject.GetComponent<Dragging>();//Componente Dragging de la carta
        if(d!=null){//Si no esta en el cementerio
            GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
            GameObject.Find("Death").GetComponent<Image>().color=new Color(1,1,1,0);
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
        }else if(c.cardRealName=="Gru"){//Si es la carta lider
                GameObject.Find("Death").GetComponent<Image>().color=new Color(1,1,1,0);
                GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
                GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[L]";
        }else if(c.id==-2){//Si es alguno de los Guardias
            GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="";
            GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);
            GameObject.Find("Death").GetComponent<Image>().color=new Color(1,1,1,0);
        }else{//Si esta en el cementerio
            GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="";
            GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);
            GameObject.Find("Death").GetComponent<Image>().color=new Color(1,1,1,0.8f);
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

            //AddedPower
            if(c.addedPower>0){
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="+"+c.addedPower.ToString();
                GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().color=new Color(0,1,0,1);
            }else if(c.addedPower<0){
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text=c.addedPower.ToString();
                GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().color=new Color(1,0,0,1);
            }else{
                GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
                GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
            }

            //Name
            GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text=c.cardRealName;
            //Description
            GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>().text=c.description;
            //EffectDescription
            if(c.effectDescription.Length>0){//Si hay descripcion de efecto
                RoundPoints.URWrite("Efecto: "+c.effectDescription);
            }else{
                //Cuando no hay descripcion de efecto en el URWrite se pone info sobre la ronda
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
