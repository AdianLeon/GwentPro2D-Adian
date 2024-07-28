using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
//Script para soltar las cartas y que se asignen a la dropzone donde corresponden si es la correcta
public abstract class DropZone : MonoBehaviour, IDropHandler, IGlow
{
    public abstract bool IsDropValid(DraggableCard card);//Condiciones para que la zona admita la carta soltada
    public virtual void OnDropAction(DraggableCard card) => card.PlayCardIn(this);
    public void OnDrop(PointerEventData eventData)
    {//Detecta cuando se suelta una carta en una zona valida
        if (!DraggableCard.IsOnDrag) { return; }//Si no se esta arrastrando
        //Solo si se esta arrastrando y la carta es aceptada por el IsDropValid de cada DropZone se realiza la OnDropAction
        if (IsDropValid(eventData.pointerDrag.GetComponent<DraggableCard>())) { OnDropAction(eventData.pointerDrag.GetComponent<DraggableCard>()); }
    }
    public virtual void TriggerGlow() => gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);//Hace que la zona se ilumine, pero hay ciertas zonas que pueden iluminarse de otra forma, por eso esta funcion es virtual
    public void RestoreGlow() => gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);//La zona se hara invisible cuando se llame a este metodo
}