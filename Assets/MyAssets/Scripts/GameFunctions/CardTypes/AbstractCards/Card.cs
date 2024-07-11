using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
//Script que contiene las propiedades de todas las cartas
abstract public class Card : MonoBehaviour, IGlow
{
    public string Faction;//Faccion de la carta
    public string CardName;//Nombre a mostrar en el objeto gigante a la izquierda del campo
    public string Description;//Descripcion de la carta a mostrar en el objeto gigante a la izquierda del campo
    public string EffectDescription;//Descripcion del efecto
    public string OnActivationName;//Nombre de OnActivation
    public Sprite Artwork;//Imagen para mostrar en el CardView
    public Player WhichField;//Campo de la carta
    public virtual Color GetCardViewColor(){return new Color(1,1,1);}//Retorna el color de la carta en el CardView
    public virtual void LoadInfo(){//Esta funcion es especifica para cada tipo de carta, pero todas comparten lo siguiente
        //Faction
        GameObject.Find("Faction").GetComponent<TextMeshProUGUI>().text=Faction;
        //Name
        GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text=CardName;
        //Description
        GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>().text=Description;
        //EffectDescription
        if(EffectDescription.Length>0){//Si hay descripcion de efecto
            RoundPoints.WriteUserRead("Efecto: "+EffectDescription);
        }else{
            RoundPoints.WriteUserRead("Esta carta no tiene efecto");
        }
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
    public void OnGlow(){//Cuando una carta desactiva su glow se devuelve a su estado normal totalmente visible
        this.gameObject.GetComponent<Image>().color=new Color(1,1,1,1);
    }
    public void OffGlow(){//Cuando una carta activa su glow se oscurece
        this.gameObject.GetComponent<Image>().color=new Color(0.75f,0.75f,0.75f,1);
    }
}