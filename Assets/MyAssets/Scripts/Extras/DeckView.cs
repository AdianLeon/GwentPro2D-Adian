using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//Script clon de CardView pero para mostrar las cartas en el menu Deck
public class DeckView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string cardRealName;//Nombre a mostrar en el objeto gigante a la izquierda del campo
    public string description;//Descripcion de la carta a mostrar en el objeto gigante a la izquierda del campo
    public string effectDescription;//Descripcion del efecto
    public Sprite artwork;//Imagen relacionada con la carta para mostrar en grande en el objeto gigante a la izquierda del campo
    public Sprite qualitySprite;//Otra imagen que representa al enum quality
    public Color cardColor;//Color determinado de la carta
    public int power;//Poder de la carta
    public string playableZone;//Zona jugable de la carta
    public bool showPower;//Si se muestra el poder de la carta o no
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        //Playable zone
        GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="["+this.playableZone+"]";

        //Quality y Image
        GameObject.Find("Quality").GetComponent<Image>().sprite=this.qualitySprite;
        GameObject.Find("CardPreview").GetComponent<Image>().sprite=this.artwork;

        //Colors
        GameObject.Find("BackGroundCard").GetComponent<Image>().color=this.cardColor;
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=this.cardColor;
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color=this.cardColor;

        //Power
        if(this.showPower){
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=this.power.ToString();
            GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
        }else{
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
            GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);
        }
        //Name
        GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text=this.cardRealName;
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
