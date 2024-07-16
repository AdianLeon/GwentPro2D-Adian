using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//Script que contiene los comportamientos y propiedades en comun de todas las cartas
public abstract class Card : MonoBehaviour, IGlow, IShowZone, IPointerEnterHandler, IPointerExitHandler
{
    private static Card enteredCard;//En esta variable se guarda el objeto debajo del puntero el cual mostramos en CardView
    public static Card GetEnteredCard=>enteredCard;
    public string Faction;//Faccion de la carta
    public string CardName;//Nombre de la carta
    public string OnActivationName;//Nombre de OnActivation
    public Sprite Artwork;//Imagen para mostrar en el CardView
    public Player WhichPlayer;//Jugador dueno de la carta
    public abstract bool IsPlayable{get;}//Conjunto de condiciones para que la carta se pueda jugar, diferente para todas las cartas
    public virtual Color GetCardViewColor=>new Color(1,1,1);//Retorna el color de la carta en el CardView
    public virtual void LoadInfo(){//Esta funcion es especifica para cada tipo de carta, pero todas comparten lo siguiente
        GameObject.Find("Faction").GetComponent<TextMeshProUGUI>().text=Faction;
        GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text=CardName;
        GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
        //EffectDescription
        GameObject.Find("EffectDescription").GetComponent<TextMeshProUGUI>().text="Esta carta no tiene efecto";
        /*if(OnActivation.EffectDescription!=""){
        }else*/if(GetComponent<ICardEffect>()!=null){
            GameObject.Find("EffectDescription").GetComponent<TextMeshProUGUI>().text=GetComponent<ICardEffect>().GetEffectDescription;
        }else if(GetComponent<ISpecialCard>()!=null){
            GameObject.Find("EffectDescription").GetComponent<TextMeshProUGUI>().text=GetComponent<ISpecialCard>().GetEffectDescription;
        }
        //Quality e Image
        GameObject.Find("Quality").GetComponent<Image>().sprite=Resources.Load<Sprite>("BlankImage");
        GameObject.Find("CardPreview").GetComponent<Image>().sprite=Artwork;

        //Colores
        GameObject.Find("BackGroundCard").GetComponent<Image>().color=GetCardViewColor;
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=GetCardViewColor;
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color=GetCardViewColor;
    }
    public virtual void ShowZone(){
        DropZone[] unitZones=GameObject.FindObjectsOfType<DropZone>();//Se crea un array con todas las zonas de cartas de unidad
        foreach(DropZone unitZone in unitZones){
            if(unitZone.IsDropValid(this.gameObject)){
                //La zona se ilumina solo si coincide con la zona jugable y el campo de la carta
                unitZone.OnGlow();
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        enteredCard=this;
        OffGlow();//La carta se sombrea cuando pasamos por encima
        LoadInfo();//Se carga toda la informacion de esta carta en el CardView

        if(!Judge.CanPlay){return;}//Si no se pude jugar
        if(enteredCard.GetComponent<DraggableCard>()==null){return;}//Si no tiene componente DraggableCard
        if(DraggableCard.IsOnDrag){return;}//Si se esta arrastrando
        if(!enteredCard.GetComponent<DraggableCard>().IsOnHand){return;}//Si no esta en la mano

        //Si el jugador puede jugar, tiene componente DraggableCard, no se esta arrastrando ninguna carta y ademas esta en la mano
        ShowZone();//Se iluminan las zonas donde la carta se puede soltar
    }
    public void OnPointerExit(PointerEventData eventData){//Se llama cuando el mouse sale de la carta
        GFUtils.GlowOff();//Se dessombrean todas las cartas jugadas y se desactiva la iluminacion de todas las zonas
        OnGlow();//La carta se dessombrea
        enteredCard=null;//Ya no se esta encima de ninguna carta
    }
    public void OnGlow(){//Cuando una carta activa su glow se devuelve a su estado normal totalmente visible
        this.gameObject.GetComponent<Image>().color=new Color(1,1,1,1);
    }
    public void OffGlow(){//Cuando una carta desactiva su glow se oscurece
        this.gameObject.GetComponent<Image>().color=new Color(0.75f,0.75f,0.75f,1);
    }
}