using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public static class GameObjectExtensionMethods
{
    public static List<DraggableCard> CardsInside(this GameObject place)
    {//Devuelve los descendientes carta de un GameObject
        if (place.GetComponent<DraggableCard>() != null)
        {//Si el lugar es una carta se devuelve esa carta
            return new List<DraggableCard>() { place.GetComponent<DraggableCard>() };
        }
        List<DraggableCard> cards = new List<DraggableCard>();//Considerando que no es una carta comenzamos a anadir sus hijos
        foreach (Transform card in place.transform)
        {//Si no tiene hijos se devolvera una lista vacia
            cards.AddRange(card.gameObject.CardsInside());//Anade a su lista lo que sus hijos devuelvan
        }
        return cards;//Devuelve todas las cartas que contiene
    }
    public static Player Field(this GameObject place)
    {//Devuelve el campo de un objeto
        return (Player)Enum.Parse(typeof(Player), "P" + place.name[place.name.Length - 1]);//Funcionara siempre que se siga la convencion de nombrar los objetos con P+(identificador)
    }
    public static void Disappear(this IEnumerable<Card> cards)
    {//Se deshace una por una de las cartas en el IEnumerable
        foreach (Card card in cards)
        {
            Disappear(card);
        }
    }
    public static void Disappear(this Card card)
    {//Se deshace de la carta mandandola a la basura
        card.GetComponent<DraggableCard>().MoveCardTo(GameObject.Find("Trash"));
    }
}
public static class GFUtils
{
    public static List<T> FindScriptsOfType<T>()
    {
        List<T> implementers = new List<T>();
        MonoBehaviour[] allScripts = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
        foreach (MonoBehaviour script in allScripts)
        {
            if (script.gameObject.GetComponent<T>() != null)
            {
                implementers.Add(script.gameObject.GetComponent<T>());
            }
        }
        return implementers;
    }
    public static void GlowOff()
    {//Desactiva toda la iluminacion de las zonas y las cartas jugadas
        foreach (DropZone zone in GameObject.FindObjectsOfType<DropZone>())
        {//Hace las zonas invisibles nuevamente
            zone.OffGlow();
        }
        foreach (DraggableCard playedCard in Field.AllPlayedCards)
        {//Las cartas se dessombrean
            playedCard.OnGlow();
        }
    }
}