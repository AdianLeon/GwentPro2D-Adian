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
    void Start(){
    }
    //Funciones necesarias para controlar si el puntero esta encima de la carta
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Poniendo el sprite de la carta en el objeto gigante de la izquierda de la pantalla
        GameObject.Find("CardPreview").GetComponent<Image>().sprite=card.GetComponent<Image>().sprite;
    }
    //Esta funcion es necesaria, no hace nada pero es necesaria
    public void OnPointerExit(PointerEventData eventData)
    {}
}
