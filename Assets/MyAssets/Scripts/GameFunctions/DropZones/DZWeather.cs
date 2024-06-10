using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Script para las DropZones de cartas de clima
public class DZWeather : DropZone
{
    public GameObject target1;//Primer objetivo que el clima afecta (P1)
    public GameObject target2;//Segundo objetivo que el clima afecta (P2)
    public override void OnDrop(PointerEventData eventData){//Detecta cuando se suelta una carta en una zona valida
        //Cambia donde se queda la carta, en vez de quedarse en la mano ahora se queda en la zona soltada si es valida
        WeatherCard w=eventData.pointerDrag.GetComponent<WeatherCard>();
        ClearWeatherCard c=eventData.pointerDrag.GetComponent<ClearWeatherCard>();
        if(c!=null || w!=null){
            //Solo si son cartas de clima o despeje
            eventData.pointerDrag.GetComponent<Dragging>().parentToReturnTo=this.transform;
        }
    }
}
