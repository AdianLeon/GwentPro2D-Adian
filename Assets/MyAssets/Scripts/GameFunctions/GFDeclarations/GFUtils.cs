using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class CustomExtensionMethods
{
    public static IEnumerable<T> CardsInside<T>(this GameObject place)
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
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {//Comodidad extra para realizar una accion simple sobre todos los elementos de un IEnumerable sin declarar ese IEnumerable
        foreach (T item in items) { action(item); }
    }
    public static T RandomElement<T>(this IEnumerable<T> items)
    {
        if (items.Count() == 0) { return default; }
        return items.ElementAt(UnityEngine.Random.Range(0, items.Count()));
    }
    public static Player Field(this GameObject place)
    {//Devuelve el campo de un objeto
        return (Player)Enum.Parse(typeof(Player), "P" + place.name[place.name.Length - 1]);//Funcionara siempre que se siga la convencion de nombrar los objetos con P+(identificador)
    }
    public static void Disappear(this IEnumerable<DraggableCard> cards)
    {//Se deshace una por una de las cartas en el IEnumerable
        cards.ForEach(card => card.Disappear());
    }
}
public static class GFUtils
{
    public static List<T> FindGameObjectsOfType<T>()
    {//Encuentra todos los scripts y luego devuelve todos los que sean tipo T
        GameObject[] allScripts = Resources.FindObjectsOfTypeAll<GameObject>();
        return allScripts.Where(script => script.gameObject.GetComponent<T>() != null).Select(item => item.GetComponent<T>()).ToList();
    }
    public static void RestoreGlow()
    {//Desactiva toda la iluminacion de las zonas y las cartas jugadas
        GameObject.FindObjectsOfType<DropZone>().ForEach(zone => zone.OffGlow());//Hace las zonas invisibles nuevamente
        Field.AllPlayedCards.ForEach(playedCard => playedCard.OnGlow());//Las cartas se dessombrean
    }
}