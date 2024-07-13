using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Script para las DropZones de cartas de aumento
public class DZBoost : DropZone
{
    public GameObject Target;//Objetivo del efecto aumento
    public Player ValidPlayer{get=>GFUtils.GetField(this.name);}//Jugador valido
    public override void OnDrop(PointerEventData eventData){//Detecta cuando se suelta una carta en una zona valida
        if(!Dragging.IsOnDrag){return;}
        //Cambia donde se queda la carta, en vez de quedarse en la mano ahora se queda en la zona soltada si es valida
        BoostCard c=eventData.pointerDrag.GetComponent<BoostCard>();
        if(c!=null){
            //Solo si son cartas de aumento del jugador correspondiente
            if(ValidPlayer==c.WhichPlayer){
                c.gameObject.GetComponent<Dragging>().DropCardOnZone(this.gameObject);
            }
        }
    }
}
