using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Script para el arrastre de las cartas
public abstract class DraggableCard : Card, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public override abstract bool IsPlayable { get; }
    private Transform parentToReturnTo = null;//Padre al que volver cuando se acaba el arrastre
    protected GameObject GetHand => GameObject.Find("Hand" + WhichPlayer);//Devuelve la mano del dueno de la carta
    public bool IsOnHand => transform.parent.gameObject == GetHand;//Devuelve si la carta se encuentra en la mano
    protected int positionInHand;
    private static bool onDrag;//Si se esta arrastrando alguna carta
    public static bool IsOnDrag { get => onDrag; }
    public void OnBeginDrag(PointerEventData eventData)
    {//Este metodo se llama cuando empieza el arrastre de la carta
        if (IsOnHand && Judge.CanPlay)
        {//Solo si esta en la mano y se puede jugar
            onDrag = true;//Comenzamos el arrastre
            //Guarda la posicion a la que volver si soltamos en lugar invalido y crea un espacio en el lugar de la carta
            transform.SetParent(GameObject.Find("Canvas").transform);//Cambia el padre de la carta al canvas para que podamos moverla libremente
            //Activa la penetracion de la carta por el puntero para que podamos soltarla
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {//Este metodo se llama continuamente mientras se arrastra la carta
        if (onDrag)
        {//Solo si se esta arrastrando
            gameObject.GetComponent<IShowZone>()?.ShowZone();//Haciendo que las zonas donde se pueda soltar la carta "brillen"

            transform.position = eventData.position;//Actualiza la posicion de la carta con la del puntero

            int newSiblingIndex = parentToReturnTo.childCount;//Guarda el indice del espacio de la derecha
            for (int i = 0; i < parentToReturnTo.childCount; i++)
            {//Chequeando constantemente si se esta a la izquierda de cada una de las cartas de la mano
                if (transform.position.x < parentToReturnTo.GetChild(i).position.x)
                {//Si la carta arrastrada esta a la izquierda
                    newSiblingIndex = i;//Actualiza el valor guardado con el actual
                    break;
                }
            }
            positionInHand = newSiblingIndex;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {//Este metodo se llama cuando termina el arrastre de la carta
        if (onDrag)
        {//Solo si se estaba arrastrando
            onDrag = false;//Terminamos el arrastre
            if (transform.parent == GameObject.Find("Canvas").transform && !TryPlay())//Si la carta todavia esta en el canvas y no se juega desde ahi
            {
                MoveCardTo(GetHand);//Devuelve la carta a la mano
                transform.SetSiblingIndex(positionInHand);//Posiciona la carta en la mano
            }
            GetComponent<CanvasGroup>().blocksRaycasts = true;//Desactiva la penetracion de la carta para que podamos mostrarla o arrastrarla de nuevo
            GFUtils.RestoreGlow();//Desactivamos el glow de cualquier objeto que hayamos iluminado
        }
    }
    public bool TryPlayCardIn(DropZone zone)
    {//Juega la carta en una zona
        MoveCardTo(zone.gameObject);
        return TryPlay();
    }
    public void MoveCardTo(GameObject zone)
    {
        transform.SetParent(zone.transform, false);//El segundo parametro es necesario para que no se altere la escala de la carta
        parentToReturnTo = zone.transform;
    }
    public void Disappear()
    {//Se deshace de la carta mandandola a la basura
        MoveCardTo(GameObject.Find("Trash"));
    }

}