using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script que contiene las propiedades de todas las cartas
public class Card : MonoBehaviour
{
    public string cardRealName;//Nombre a mostrar en el objeto gigante a la izquierda del campo
    public string description;//Descripcion de la carta a mostrar en el objeto gigante a la izquierda del campo
    public string effectDescription;//Descripcion del efecto
    public Sprite artwork;//Imagen relacionada con la carta para mostrar en grande en el objeto gigante a la izquierda del campo
    public Sprite qualitySprite;//Otra imagen que representa al enum quality
    public Color cardColor;//Color determinado de la carta
    public enum fields{P1,P2};
    public fields whichField;
    public virtual void LoadInfo(){//Esta funcion es especifica para cada tipo de carta, pero todas comparten lo siguiente
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
        GameObject.Find("Quality").GetComponent<Image>().sprite=qualitySprite;
        GameObject.Find("CardPreview").GetComponent<Image>().sprite=artwork;

        //Colores
        GameObject.Find("BackGroundCard").GetComponent<Image>().color=cardColor;
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color=cardColor;
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color=cardColor;

        //Special
        if(cardRealName=="Guardia"){//Si es alguno de los Guardias
            GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="";
            GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);
        }
    }
}
