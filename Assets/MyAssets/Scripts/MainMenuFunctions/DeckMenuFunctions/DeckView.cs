using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//Script clon de CardView pero para mostrar las cartas en el menu Deck
public class DeckView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string faction;//Faccion (Nombre del deck)
    public string cardName;//Nombre a mostrar en el objeto gigante a la izquierda del campo
    public string description;//Descripcion de la carta a mostrar en el objeto gigante a la izquierda del campo
    public string effectDescription;//Descripcion del efecto
    public Sprite artwork;//Imagen relacionada con la carta para mostrar en grande en el objeto gigante a la izquierda del campo
    private Color cardColor;
    public string typeComponent;
    public int power;//Poder de la carta
    public string playableZone;//Zona jugable de la carta
    private void Start(){
        if(typeComponent=="WeatherCard"){
            cardColor=new Color(0.7f,0.2f,0.2f);
        }else if(typeComponent=="ClearWeatherCard"){
            cardColor=new Color(0.5f,1,1);
        }else if(typeComponent=="BoostCard"){
            cardColor=new Color(0.4f,1,0.3f);
        }else if(typeComponent=="BaitCard"){
            cardColor=new Color(0.8f,0.5f,0.7f);
        }else if(typeComponent=="SilverCard"){
            cardColor=new Color(0.8f,0.8f,0.8f);
        }else if(typeComponent=="GoldCard"){
            cardColor=new Color(0.8f,0.7f,0.2f);
        }else if(typeComponent=="LeaderCard"){
            cardColor=new Color(0.7f,0.1f,0.5f);
        }else{
            cardColor=new Color(1,1,1);
        }
    }
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        //Playable zone
        GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="["+this.playableZone+"]";

        //Quality
        if(typeComponent=="SilverCard"){
            GameObject.Find("Quality").GetComponent<Image>().sprite=Resources.Load<Sprite>("SilverQuality");
        }else if(typeComponent=="GoldCard"){
            GameObject.Find("Quality").GetComponent<Image>().sprite=Resources.Load<Sprite>("GoldQuality");
        }else if(typeComponent=="LeaderCard"){
            GameObject.Find("Quality").GetComponent<Image>().sprite=Resources.Load<Sprite>("LeaderQuality");
        }else{
        GameObject.Find("Quality").GetComponent<Image>().sprite=Resources.Load<Sprite>("BlankImage");
        }

        //Image
        GameObject.Find("CardPreview").GetComponent<Image>().sprite=this.artwork;

        //Colors
        GameObject.Find("BackGroundCard").GetComponent<Image>().color=this.cardColor;
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=this.cardColor;
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color=this.cardColor;

        //Power
        if(playableZone=="C"){
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="-"+this.power.ToString();
        }else if(playableZone=="A"){
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="+"+this.power.ToString();
        }else{
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=this.power.ToString();
        }
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
        if(playableZone=="D" || playableZone=="L"){
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
            GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);
        }
        //Name
        GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text=this.cardName;
        //Description
        GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>().text=this.description;
        //EffectDescription
        if(this.effectDescription.Length>0){//Si tiene descripcion de efecto
            GameObject.Find("Effect Description").GetComponent<TextMeshProUGUI>().text=this.effectDescription;
        }else{//Caso contrario para evitar dejar el efecto escrito en el objeto; se escribe que no tiene efecto
            GameObject.Find("Effect Description").GetComponent<TextMeshProUGUI>().text="Esta carta no tiene efecto";
        }
        this.GetComponent<Image>().color=new Color (0.75f,0.75f,0.75f,1);//La carta se sombrea cuando se pasa por encima
    }
    public void OnPointerExit(PointerEventData eventData){
        this.GetComponent<Image>().color=new Color (1,1,1,1);//La carta se dessombrea cuando se sale de encima de ella
    }
}
