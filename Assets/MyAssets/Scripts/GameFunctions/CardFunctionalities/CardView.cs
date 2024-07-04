using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//Script para algunos efectos cuando se pase el mouse por encima de la carta
public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Objetos a utilizar
    private static GameObject selectedCard;//En esta variable se guarda el objeto debajo del puntero el cual mostramos en CardView
    public static GameObject GetSelectedCard{get=>selectedCard;}
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        selectedCard=this.gameObject;
        selectedCard.GetComponent<Card>().OnGlow();//La carta se sombrea cuando pasamos por encima
        selectedCard.GetComponent<Card>().LoadInfo();//Se carga toda la informacion de esta carta en el CardView
        if(Board.CanPlay && selectedCard.GetComponent<Dragging>()!=null){//Si el jugador puede jugar
            if(!Dragging.IsOnDrag && selectedCard.GetComponent<Dragging>().IsOnHand){//Si no se esta arrastrando ninguna carta y ademas esta en la mano
                VisualEffects.ZonesGlow(selectedCard);//Se ilumina la zona donde se puede soltar
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData){//Se llama cuando el mouse sale de la carta
        VisualEffects.OffCardsGlow();//Se dessombrean todas las cartas jugadas
        VisualEffects.OffZonesGlow();//Se desactiva la iluminacion de todas las zonas

        selectedCard.GetComponent<Card>().OffGlow();//La carta se dessombrea
        selectedCard=null;//Ya no se esta encima de ninguna carta

        if(Dragging.IsOnDrag){//Se actualiza el mensaje en pantalla
            RoundPoints.WriteUserRead(". . .");
        }else{
            RoundPoints.WriteRoundInfoUserRead();
        }
    }

}