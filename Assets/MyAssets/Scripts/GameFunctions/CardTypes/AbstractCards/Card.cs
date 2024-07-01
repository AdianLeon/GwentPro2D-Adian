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
    public string OnActivationCode{get => onActivationCode; set => onActivationCode=value;}
    public Sprite artwork;//Imagen para mostrar en el CardView
    private Color cardViewColor;//Color de la carta en el CardView
    public fields whichField;
    public fields WhichField{get=>whichField;set{Debug.Log(this.gameObject.name+" whichField set to "+value); whichField=value;}}

    private void Start(){
        if(GetComponent<WeatherCard>()!=null){
            cardViewColor=new Color(0.7f,0.2f,0.2f);
        }else if(GetComponent<ClearWeatherCard>()!=null){
            cardViewColor=new Color(0.5f,1,1);
        }else if(GetComponent<BoostCard>()!=null){
            cardViewColor=new Color(0.4f,1,0.3f);
        }else if(GetComponent<BaitCard>()!=null){
            cardViewColor=new Color(0.8f,0.5f,0.7f);
        }else if(GetComponent<SilverCard>()!=null){
            cardViewColor=new Color(0.8f,0.8f,0.8f);
        }else if(GetComponent<GoldCard>()!=null){
            cardViewColor=new Color(0.8f,0.7f,0.2f);
        }else if(GetComponent<LeaderCard>()!=null){
            cardViewColor=new Color(0.7f,0.1f,0.5f);
        }else{
            cardViewColor=new Color(1,1,1);
        }
    }
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
        GameObject.Find("BackGroundCard").GetComponent<Image>().color=cardViewColor;
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=cardViewColor;
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color=cardViewColor;
    }
}
