using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script que contiene los comportamientos y propiedades en comun de todas las cartas
public abstract class Card : MonoBehaviour, IGlow, IShowZone
{
    public string Faction;//Faccion de la carta
    public string CardName;//Nombre de la carta
    public string EffectDescription;//Descripcion del efecto de la carta
    public string OnActivationName;//Nombre de OnActivation
    public Sprite Artwork;//Imagen para mostrar en el CardView
    public Player WhichPlayer;//Jugador dueno de la carta
    public virtual Color GetCardViewColor(){return new Color(1,1,1);}//Retorna el color de la carta en el CardView
    public virtual void LoadInfo(){//Esta funcion es especifica para cada tipo de carta, pero todas comparten lo siguiente
        GameObject.Find("Faction").GetComponent<TextMeshProUGUI>().text=Faction;
        GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text=CardName;
        GameObject.Find("EffectDescription").GetComponent<TextMeshProUGUI>().text=EffectDescription;
        GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);

        //Quality e Image
        GameObject.Find("Quality").GetComponent<Image>().sprite=Resources.Load<Sprite>("BlankImage");
        GameObject.Find("CardPreview").GetComponent<Image>().sprite=Artwork;

        //Colores
        GameObject.Find("BackGroundCard").GetComponent<Image>().color=GetCardViewColor();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=GetCardViewColor();
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color=GetCardViewColor();
    }
    public abstract bool IsPlayable{get;}//Conjunto de condiciones para que la carta se pueda jugar, diferente para todas las cartas
    public virtual void ShowZone(){
        DropZone[] unitZones=GameObject.FindObjectsOfType<DropZone>();//Se crea un array con todas las zonas de cartas de unidad
        foreach(DropZone unitZone in unitZones){
            if(unitZone.IsDropValid(this.gameObject)){
                //La zona se ilumina solo si coincide con la zona jugable y el campo de la carta
                unitZone.OnGlow();
            }
        }
    }
    public void OnGlow(){//Cuando una carta activa su glow se devuelve a su estado normal totalmente visible
        this.gameObject.GetComponent<Image>().color=new Color(1,1,1,1);
    }
    public void OffGlow(){//Cuando una carta desactiva su glow se oscurece
        this.gameObject.GetComponent<Image>().color=new Color(0.75f,0.75f,0.75f,1);
    }
}