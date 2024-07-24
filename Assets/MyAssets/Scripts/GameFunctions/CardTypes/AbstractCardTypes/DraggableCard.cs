using UnityEngine;
using UnityEngine.EventSystems;
//Script para el arrastre de las cartas
public abstract class DraggableCard : Card, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public override bool IsPlayable => transform.parent.gameObject.GetComponent<DropZone>() ? transform.parent.gameObject.GetComponent<DropZone>().IsDropValid(this) : false;
    protected GameObject GetHand => GameObject.Find("Hand" + WhichPlayer);//Devuelve la mano del dueno de la carta
    public bool IsOnHand => transform.parent.gameObject == GetHand;//Devuelve si la carta se encuentra en la mano
    protected int positionInHand;
    private static bool onDrag;//Si se esta arrastrando alguna carta
    public static bool IsOnDrag => onDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {//Este metodo se llama cuando empieza el arrastre de la carta
        if (!IsOnHand) { return; }//Si no esta en la mano
        if (!Judge.CanPlay) { return; }//Si no se puede jugar
        onDrag = true;//Comenzamos el arrastre
        transform.SetParent(GameObject.Find("Canvas").transform);//Cambia el padre de la carta al canvas para que podamos moverla libremente
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;//Activa la penetracion de la carta por el puntero para que podamos soltarla
    }
    public void OnDrag(PointerEventData eventData)
    {//Este metodo se llama continuamente mientras se arrastra la carta
        if (!onDrag) { return; }//Si no se esta arrastrando alguna carta
        gameObject.GetComponent<IShowZone>()?.ShowZone();//Haciendo que las zonas donde se pueda soltar la carta "brillen"
        transform.position = eventData.position;//Actualiza la posicion de la carta con la del puntero
        int newSiblingIndex = GetHand.transform.childCount;//Guarda el indice del espacio de la derecha
        for (int i = 0; i < GetHand.transform.childCount; i++)
        {//Chequeando constantemente si se esta a la izquierda de cada una de las cartas de la mano
            if (transform.position.x < GetHand.transform.GetChild(i).position.x)
            {//Si la carta arrastrada esta a la izquierda
                newSiblingIndex = i;//Actualiza el valor guardado con el actual
                break;
            }
        }
        positionInHand = newSiblingIndex;
    }
    public void OnEndDrag(PointerEventData eventData)
    {//Este metodo se llama cuando termina el arrastre de la carta
        if (!onDrag) { return; }//Si no se esta arrastrando alguna carta
        onDrag = false;//Terminamos el arrastre
        if (transform.parent == GameObject.Find("Canvas").transform && !TryPlay())
        {//Si la carta todavia esta en el canvas y no se juega desde ahi
            MoveCardTo(GetHand);//Devuelve la carta a la mano
            transform.SetSiblingIndex(positionInHand);//Posiciona la carta en la mano
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;//Desactiva la penetracion de la carta para que podamos mostrarla o arrastrarla de nuevo
        GFUtils.RestoreGlow();//Desactivamos el glow de cualquier objeto que hayamos iluminado
    }
    public bool TryPlayCardIn(DropZone zone) { MoveCardTo(zone.gameObject); return TryPlay(); }//Intenta jugar la carta en una zona
    public void MoveCardTo(GameObject zone) { transform.SetParent(zone.transform, false); }//El segundo parametro de SetParent() es necesario para que no se altere la escala de la carta
    public void Disappear() { MoveCardTo(GameObject.Find("Trash")); }//Se deshace de la carta mandandola a la basura
}