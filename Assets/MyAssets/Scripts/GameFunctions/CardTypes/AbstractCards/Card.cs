using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script que contiene las propiedades de todas las cartas
abstract public class Card : MonoBehaviour
{
    public string faction;//Faccion de la carta
    public string cardRealName;//Nombre a mostrar en el objeto gigante a la izquierda del campo
    public string description;//Descripcion de la carta a mostrar en el objeto gigante a la izquierda del campo
    public string effectDescription;//Descripcion del efecto
    private string onActivationCode;
    public string OnActivationCode{get=>onActivationCode; set=>onActivationCode=value;}
    public Sprite artwork;//Imagen para mostrar en el CardView
    public fields whichField;//Campo de la carta
    public fields WhichField{get=>whichField; set=>whichField=value;}

    public virtual Color GetCardViewColor(){return new Color(1,1,1);}//Retorna el color de la carta en el CardView
    public virtual void LoadInfo(){//Esta funcion es especifica para cada tipo de carta, pero todas comparten lo siguiente
        //Faction
        GameObject.Find("Faction").GetComponent<TextMeshProUGUI>().text=faction;
        //Name
        GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text=cardRealName;
        //Description
        GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>().text=description;
        //EffectDescription
        if(effectDescription.Length>0){//Si hay descripcion de efecto
            RoundPoints.URWrite("Efecto: "+effectDescription);
        }else{
            RoundPoints.URWrite("Esta carta no tiene efecto");
        }
        GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);

        //Quality e Image
        GameObject.Find("Quality").GetComponent<Image>().sprite=Resources.Load<Sprite>("BlankImage");
        GameObject.Find("CardPreview").GetComponent<Image>().sprite=artwork;

        //Colores
        GameObject.Find("BackGroundCard").GetComponent<Image>().color=GetCardViewColor();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=GetCardViewColor();
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color=GetCardViewColor();
    }
}
