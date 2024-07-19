using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class CustomExtensionMethods
{
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {//Comodidad extra para realizar una accion simple sobre todos los elementos de un IEnumerable sin declarar ese IEnumerable
        foreach (T item in items) { action(item); }
    }
    public static IEnumerable<T> TransformToIEnumerable<T>(this Transform zone)
    {//Para cuando se haga deseen usar metodos de Linq sobre un transform
        foreach (Transform card in zone)
        {
            if (card.GetComponent<T>() != null)
            {
                yield return card.GetComponent<T>();
            }
        }
    }
    public static List<T> CardsInside<T>(this GameObject place)
    {//Devuelve los descendientes carta de un GameObject
        if (place.GetComponent<T>() != null)
        {//Si el lugar es una carta se devuelve esa carta
            return new List<T>() { place.GetComponent<T>() };
        }
        List<T> cards = new List<T>();//Considerando que no es una carta comenzamos a anadir sus hijos
        foreach (Transform card in place.transform)
        {//Si no tiene hijos se devolvera una lista vacia
            cards.AddRange(card.gameObject.CardsInside<T>());//Anade a su lista lo que sus hijos devuelvan
        }
        return cards;//Devuelve todas las cartas que contiene
    }
    public static Player Field(this GameObject place)
    {//Devuelve el campo de un objeto
        return (Player)Enum.Parse(typeof(Player), "P" + place.name[place.name.Length - 1]);//Funcionara siempre que se siga la convencion de nombrar los objetos con P+(identificador)
    }
    public static void Disappear(this IEnumerable<Card> cards)
    {//Se deshace una por una de las cartas en el IEnumerable
        cards.ForEach(card => card.Disappear());
    }
    public static void Disappear(this Card card)
    {//Se deshace de la carta mandandola a la basura
        card.GetComponent<DraggableCard>().MoveCardTo(GameObject.Find("Trash"));
    }
}
public static class GFUtils
{
    public static List<T> FindScriptsOfType<T>()
    {//Encuentra todos los scripts y luego devuelve todos los que sean tipo T
        MonoBehaviour[] allScripts = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
        return allScripts.Where(script => script.gameObject.GetComponent<T>() != null).Select(item => item.GetComponent<T>()).ToList();
    }
    public static void GlowOff()
    {//Desactiva toda la iluminacion de las zonas y las cartas jugadas
        //Hace las zonas invisibles nuevamente
        GameObject.FindObjectsOfType<DropZone>().ForEach(zone => zone.OffGlow());
        //Las cartas se dessombrean
        Field.AllPlayedCards.ForEach(playedCard => playedCard.OnGlow());
    }
}