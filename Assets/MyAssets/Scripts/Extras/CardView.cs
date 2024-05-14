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
    public GameObject card;
    public static string cardName;
    
    void Start(){
        cardName="None";//Inicializamos la referencia del nombre de la carta a nada
    }
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        this.gameObject.GetComponent<Image>().color=new Color (0.75f,0.75f,0.75f,1);//La carta se sombrea cuando pasamos por encima
        this.gameObject.GetComponent<Card>().LoadInfo();//Se carga toda la informacion de esta carta en el CardView
        if(this.gameObject.GetComponent<Dragging>()!=null){
            if(!Dragging.onDrag && this.gameObject.transform.parent==this.gameObject.GetComponent<Dragging>().hand.transform)//Si no se esta arrastrando ninguna carta y ademas esta en la mano
                VisualEffects.ZonesGlow(this.gameObject);//Se ilumina la zona donde se puede soltar
        }
        cardName=this.gameObject.name;//Obtenemos la referencia a esta carta para usarla luego
    }

    public void OnPointerExit(PointerEventData eventData){//Se activa cuando el mouse sale de la carta
        if(!Dragging.onDrag && this.gameObject.GetComponent<Dragging>()!=null){//Si no se esta arrastrando ninguna carta y el objeto tiene dragging
            if(this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts==true)//Si el objecto bloquea los raycasts
                VisualEffects.OffZonesGlow();//Se desactivan la iluminacion de todas las zonas
        }
        cardName="None";//Se pierde el nombre
        this.gameObject.GetComponent<Image>().color=new Color (1,1,1,1);//La carta se dessombrea
        if(TurnManager.CardsPlayed!=0 && !TurnManager.lastTurn){//Si se han jugado cartas y no es el ultimo turno
            RoundPoints.URWrite("Presiona espacio para pasar de turno");
        }
    }

}
