using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para soltar las cartas y que se asignen a la dropzone donde corresponden
public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Obtiene la propiedad de Dragging de los rangos y los campos
    public Dragging.rank cardType;
    public Dragging.fields whichField;
    public static GameObject zoneEntered=null;
    public static bool pointerInZone;//Si el puntero esta sobre una zona o no
    public static float lastClickTime;//Necesario para que funcione correctamente el click sobre zonas

    void Start(){
        lastClickTime=0;
    }
    public void OnPointerEnter(PointerEventData eventData){//Cuando el mouse esta en la zona
        pointerInZone=true;
        zoneEntered=gameObject;
        if(!Dragging.onDrag)
            this.gameObject.GetComponent<Image>().color= new Color (1,1,1,0.1f);//La zona se ilumina
    }
    public void OnPointerExit(PointerEventData eventData){//Cuando el mouse sale de la zona
        zoneEntered=null;
        pointerInZone=false;//El mouse ya no esta en la zona
        if(!Dragging.onDrag)
            this.gameObject.GetComponent<Image>().color= new Color (1,1,1,0);//Se deja de iluminar
    }
    
    public void OnDrop(PointerEventData eventData){//Detecta cuando se suelta una carta en una zona valida
        //Cambia donde se queda la carta, en vez de quedarse en la mano ahora se queda en la zona soltada si es valida
        Dragging d=eventData.pointerDrag.GetComponent<Dragging>();
        if(d!=null){
            //Solo si coincide el tipo de carta con el tipo de dropzone y es en el campo correspondiente
            if(cardType==d.cardType && (whichField==d.whichField || whichField==Dragging.fields.None))
            {
                d.parentToReturnTo=this.transform;
            }
        }

    }
}
