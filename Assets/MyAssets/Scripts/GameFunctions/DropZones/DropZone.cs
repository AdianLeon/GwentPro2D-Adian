using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para soltar las cartas y que se asignen a la dropzone donde corresponden si es la correcta
public abstract class DropZone : MonoBehaviour, IDropHandler
{
    public virtual void OnDrop(PointerEventData eventData){}//Detecta cuando se suelta una carta en una zona valida
    //Esta funcion varia en dependencia de la DropZone, por ello se desarrolla en los scripts que heredan de este
}