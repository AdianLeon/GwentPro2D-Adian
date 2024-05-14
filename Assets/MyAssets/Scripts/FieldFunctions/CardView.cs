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
    public static GameObject selectedCard;
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        selectedCard=this.gameObject;
        selectedCard.GetComponent<Image>().color=new Color (0.75f,0.75f,0.75f,1);//La carta se sombrea cuando pasamos por encima
        selectedCard.GetComponent<Card>().LoadInfo();//Se carga toda la informacion de esta carta en el CardView
        if(selectedCard.GetComponent<Dragging>()!=null){
            if(!Dragging.onDrag && selectedCard.transform.parent==selectedCard.GetComponent<Dragging>().hand.transform)//Si no se esta arrastrando ninguna carta y ademas esta en la mano
                VisualEffects.ZonesGlow(selectedCard);//Se ilumina la zona donde se puede soltar
        }
    }

    public void OnPointerExit(PointerEventData eventData){//Se activa cuando el mouse sale de la carta
        if(!Dragging.onDrag && selectedCard.GetComponent<Dragging>()!=null){//Si no se esta arrastrando ninguna carta y el objeto tiene dragging
            if(selectedCard.GetComponent<CanvasGroup>().blocksRaycasts==true)//Si el objecto bloquea los raycasts
                VisualEffects.OffZonesGlow();//Se desactivan la iluminacion de todas las zonas
        }
        selectedCard.GetComponent<Image>().color=new Color (1,1,1,1);//La carta se dessombrea
        if(TurnManager.CardsPlayed!=0 && !TurnManager.lastTurn){//Si se han jugado cartas y no es el ultimo turno
            RoundPoints.URWrite("Presiona espacio para pasar de turno");
        }
    }

}
