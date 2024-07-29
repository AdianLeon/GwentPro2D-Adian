using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
//Script para el arrastre de las cartas
public abstract class DraggableCard : Card, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected GameObject GetHand => GameObject.Find("Hand" + Owner);//Devuelve la mano del dueno de la carta
    protected int positionInHand;//Para guardar la posicion en la mano de la carta
    private static bool onDrag;//Si se esta arrastrando alguna carta
    public static bool IsOnDrag => onDrag;
    public override bool IsPlayable => transform.parent.GetComponent<DropZone>() ? transform.parent.GetComponent<DropZone>().IsDropValid(this) : false;
    public bool IsOnHand => transform.parent.gameObject == GetHand;//Devuelve si la carta se encuentra en la mano
    public void OnBeginDrag(PointerEventData eventData)
    {//Este metodo se llama cuando empieza el arrastre de la carta
        if (!IsOnHand) { return; }//Si no esta en la mano
        if (!Judge.CanPlay) { UserRead.Write("No puedes jugar mas de una carta. Presiona espacio para acabar el turno"); return; }//Si no se puede jugar
        onDrag = true;//Comenzamos el arrastre
        MoveCardTo(GameObject.Find("Canvas"));//Cambia el padre de la carta al canvas para que podamos moverla libremente
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;//Permite la penetracion de la carta por el puntero para que podamos soltarla
    }
    public void OnDrag(PointerEventData eventData)
    {//Este metodo se llama continuamente mientras se arrastra la carta
        if (!onDrag) { return; }//Si no se esta arrastrando alguna carta
        ShowZone();//Haciendo que las zonas donde se pueda soltar la carta "brillen"
        transform.position = eventData.position;//Actualiza la posicion de la carta con la del puntero
        positionInHand = GetHand.transform.childCount;//Guarda el indice del espacio de la derecha
        for (int i = 0; i < GetHand.transform.childCount; i++)
        {//Chequeando constantemente si se esta a la izquierda de cada una de las cartas de la mano, si lo esta entonces esa es su nueva posicion
            if (transform.position.x < GetHand.transform.GetChild(i).position.x) { positionInHand = i; break; }
        }
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
        GFUtils.FindGameObjectsOfType<IGlow>().ForEach(glower => glower.RestoreGlow());//Se dessombrea todo lo que hayamos iluminado
    }
    public override void OnPointerEnter(PointerEventData eventData) { base.OnPointerEnter(eventData); TryShowZone(); }//Se anade que cuando el puntero se pase por encima la carta muestra sus zonas jugables
    private void TryShowZone()
    {//Si el jugador puede jugar, tiene componente DraggableCard, no se esta arrastrando ninguna carta y ademas esta en la mano se iluminan las zonas donde la carta se puede soltar
        if (onDrag) { return; }//Si se esta arrastrando alguna carta
        if (!Judge.CanPlay) { return; }//Si no se puede jugar
        if (!IsOnHand) { return; }//Si no esta en la mano
        ShowZone();
    }
    public virtual void ShowZone() => FindObjectsOfType<DropZone>().Where(zone => zone.IsDropValid(gameObject.GetComponent<DraggableCard>())).ForEach(zone => zone.TriggerGlow());//Ilumina todas las zonas que cumplen que el dropeo de esta carta es valido
    public void PlayCardIn(DropZone zone) { MoveCardTo(zone.gameObject); TryPlay(); }//Intenta jugar la carta en una zona
    public void MoveCardTo(GameObject zone) { transform.SetParent(zone.transform, false); }//El segundo parametro de SetParent() es necesario para que no se altere la escala de la carta
    public void Disappear() { MoveCardTo(GameObject.Find("Trash")); }//Se deshace de la carta mandandola a la basura
}