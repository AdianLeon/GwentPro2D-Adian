using UnityEngine;
using UnityEngine.EventSystems;
//Script para algunos efectos cuando se pase el mouse por encima de la carta
public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Objetos a utilizar
    private static GameObject selectedCard;//En esta variable se guarda el objeto debajo del puntero el cual mostramos en CardView
    public static GameObject GetSelectedCard{get=>selectedCard;}
    public void OnPointerEnter(PointerEventData eventData){//Se activa cuando el mouse entra en la carta
        selectedCard=this.gameObject;
        selectedCard.GetComponent<Card>().OffGlow();//La carta se sombrea cuando pasamos por encima
        selectedCard.GetComponent<Card>().LoadInfo();//Se carga toda la informacion de esta carta en el CardView

        if(!Judge.CanPlay){return;}//Si no se pude jugar
        if(selectedCard.GetComponent<Dragging>()==null){return;}//Si no tiene componente Dragging
        if(Dragging.IsOnDrag){return;}//Si se esta arrastrando
        if(!selectedCard.GetComponent<Dragging>().IsOnHand){return;}//Si no esta en la mano

        //Si el jugador puede jugar, tiene componente Dragging, no se esta arrastrando ninguna carta y ademas esta en la mano
        selectedCard.GetComponent<IShowZone>()?.ShowZone();//Se iluminan las zonas donde la carta se puede soltar
    }
    public void OnPointerExit(PointerEventData eventData){//Se llama cuando el mouse sale de la carta
        GFUtils.GlowOff();//Se dessombrean todas las cartas jugadas y se desactiva la iluminacion de todas las zonas
        selectedCard.GetComponent<Card>().OnGlow();//La carta se dessombrea
        selectedCard=null;//Ya no se esta encima de ninguna carta
    }

}