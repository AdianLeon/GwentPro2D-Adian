using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para que se vea la carta en grande en al izquierda de la pantalla
public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Objetos a utilizar
    public GameObject card;
    public static string cardName;
    void Start(){
        card=this.gameObject;
    }
    //Funciones necesarias para controlar si el puntero esta encima de la carta
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Poniendo el sprite de la carta en el objeto gigante de la izquierda de la pantalla
        GameObject.Find("CardPreview").GetComponent<Image>().sprite=card.GetComponent<Image>().sprite;
        cardName=card.name;
        //GameObject.Find("PowerCard").GetComponent<Tmpro>().text=card.GetComponent<Card>().power.ToString();
        //if(addedPower>=0){GameObject.Find("AddedPower").GetComponent<Testrapro>().text="+"+card.GetComponent<Card>().addedPower.ToString();}
        //else{GameObject.Find("AddedPower").GetComponent<Testrapro>().text=card.GetComponent<Card>().addedPower.ToString();}
    }
    //Esta funcion es necesaria, no hace nada pero es necesaria
    public void OnPointerExit(PointerEventData eventData)
    {}
}
