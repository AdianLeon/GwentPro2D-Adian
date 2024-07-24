using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//Script para soltar las cartas y que se asignen a la dropzone donde corresponden si es la correcta
public abstract class DropZone : MonoBehaviour, IDropHandler, IGlow
{
    public abstract bool IsDropValid(DraggableCard card);//Condiciones para que la zona admita la carta soltada
    public virtual void OnDrop(PointerEventData eventData)
    {//Detecta cuando se suelta una carta en una zona valida
        if (!DraggableCard.IsOnDrag) { return; }//Si no se esta arrastrando
        if (IsDropValid(eventData.pointerDrag.GetComponent<DraggableCard>()))
        {//Solo si se esta arrastrando y es valido segun el IsDropValid de cada DropZone
            eventData.pointerDrag.GetComponent<DraggableCard>().TryPlayCardIn(this);
        }
    }
    public virtual void OnGlow()
    {//Hace que la zona se ilumine, pero hay ciertas zonas que pueden iluminarse de otra forma, por eso esta funcion es virtual
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
    }
    public void OffGlow()
    {//Todas las zonas cuando se llame este metodo se haran invisibles de nuevo
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
}