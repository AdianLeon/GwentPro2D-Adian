using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para soltar las cartas y que se asignen a la dropzone donde corresponden si es la correcta
public abstract class DropZone : MonoBehaviour, IDropHandler, IGlow
{
    public abstract void OnDrop(PointerEventData eventData);//Detecta cuando se suelta una carta en una zona valida
    //Esta funcion varia en dependencia de la DropZone, por ello se desarrolla en los scripts que heredan de este
    public virtual void OnGlow(){//Hace que la zona se ilumine, pero hay ciertas zonas que pueden iluminarse de otra forma, por eso esta funcion es virtual
        this.gameObject.GetComponent<Image>().color=new Color(1,1,1,0.1f);
    }
    public void OffGlow(){//Todas las zonas cuando se llame este metodo se haran invisibles de nuevo
        this.gameObject.GetComponent<Image>().color=new Color(1,1,1,0);
    }
}