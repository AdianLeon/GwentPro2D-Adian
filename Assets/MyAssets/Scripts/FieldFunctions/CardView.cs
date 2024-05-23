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
    public static GameObject selectedCard;//En esta variable se guarda el objeto debajo del puntero el cual mostramos en CardView
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        selectedCard=this.gameObject;
        selectedCard.GetComponent<Image>().color=new Color (0.75f,0.75f,0.75f,1);//La carta se sombrea cuando pasamos por encima
        selectedCard.GetComponent<Card>().LoadInfo();//Se carga toda la informacion de esta carta en el CardView
        if((TurnManager.cardsPlayed==0 || TurnManager.lastTurn) && selectedCard.GetComponent<Dragging>()!=null){//Si el jugador puede jugar
            if(!Dragging.onDrag && selectedCard.transform.parent==selectedCard.GetComponent<Dragging>().hand.transform)//Si no se esta arrastrando ninguna carta y ademas esta en la mano
                if(selectedCard.GetComponent<BaitCard>()!=null){//Si la carta es senuelo
                    VisualEffects.ValidSwapsGlow(selectedCard);//Se iluminan las cartas con las que el senuelo se puede intercambiar
                }else{
                    VisualEffects.ZonesGlow(selectedCard);//Se ilumina la zona donde se puede soltar
                }
        }
    }

    public void OnPointerExit(PointerEventData eventData){//Se activa cuando el mouse sale de la carta
        if(!Dragging.onDrag && selectedCard.GetComponent<Dragging>()!=null){//Si no se esta arrastrando ninguna carta y el objeto tiene dragging
            if(selectedCard.GetComponent<CanvasGroup>().blocksRaycasts==true){//Si el objecto bloquea los raycasts
                VisualEffects.OffCardsGlow();//Se dessombrean todas las cartas
                VisualEffects.OffZonesGlow();//Se desactiva la iluminacion de todas las zonas
            }
        }
        selectedCard.GetComponent<Image>().color=new Color (1,1,1,1);//La carta se dessombrea
        if(Dragging.onDrag && Dragging.cardBeingDragged.GetComponent<BaitCard>()!=null){//Si estamos arrastrando un senuelo
            VisualEffects.ValidSwapsGlow(Dragging.cardBeingDragged);//Se iluminan las cartas con las que el senuelo se puede intercambiar
        }
        selectedCard=null;
        if(Dragging.onDrag){
            RoundPoints.URWrite(". . .");
        }else{
            RoundPoints.URWriteRoundInfo();
        }
    }

}
