using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//Script que declara metodos recurrentes que pueden ser utilizados por cualquier script
public static class GFUtils
{
    public static List<T> FindGameObjectsOfType<T>()
    {//Encuentra todos los scripts y luego devuelve todos los que sean tipo T
        return Resources.FindObjectsOfTypeAll<GameObject>().Where(gameObject => gameObject.GetComponent<T>() != null).Select(gameObject => gameObject.GetComponent<T>()).ToList();
    }
}
public static class CustomLinqExtensions
{
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) { foreach (T item in items) { action(item); } }//Comodidad extra para realizar una accion simple sobre todos los elementos de un IEnumerable
    public static T RandomElement<T>(this IEnumerable<T> items) => items.Count() == 0 ? default : items.ElementAt(UnityEngine.Random.Range(0, items.Count()));//Devuelve un elemento random dado un IEnumerable
    public static T MaxBy<T, G>(this IEnumerable<T> items, Func<T, G> selector) where G : IComparable<G> => ComparingBy(items, selector, (first, second) => first.CompareTo(second) > 0);
    public static T MinBy<T, G>(this IEnumerable<T> items, Func<T, G> selector) where G : IComparable<G> => ComparingBy(items, selector, (first, second) => first.CompareTo(second) < 0);
    private static T ComparingBy<T, G>(IEnumerable<T> items, Func<T, G> selector, Func<G, G, bool> predicate) where G : IComparable<G>
    {//Recibe un predicado, itera por todos los items y si se cumple el predicado entonces ese es el mejor valor a retornar
        var enumerator = items.GetEnumerator();
        if (!enumerator.MoveNext()) { throw new Exception("Collection is empty"); }
        T bestItem = enumerator.Current; G bestValue = selector(bestItem);
        while (enumerator.MoveNext()) { if (predicate(selector(enumerator.Current), bestValue)) { bestItem = enumerator.Current; bestValue = selector(bestItem); } }
        return bestItem;
    }
}
public static class CustomGameObjectExtensions
{
    public static IEnumerable<T> CardsInside<T>(this GameObject place)
    {//Devuelve los descendientes carta de un GameObject
        if (place.GetComponent<T>() != null) { return new List<T>() { place.GetComponent<T>() }; }//Si el lugar es una carta se devuelve esa carta

        List<T> cards = new List<T>();//Considerando que no es una carta comenzamos a anadir sus hijos
        foreach (Transform card in place.transform)
        {//Si no tiene hijos se devolvera una lista vacia
            cards.AddRange(card.gameObject.CardsInside<T>());//Anade a su lista lo que sus hijos devuelvan
        }

        return cards;//Devuelve todas las cartas que contiene
    }
    public static Player Field(this GameObject place) => (Player)Enum.Parse(typeof(Player), "P" + place.name[place.name.Length - 1]);//Devuelve el dueno. Funcionara siempre que se siga la convencion de nombrar los objetos de jugadores con P+(identificador)
    public static void Disappear(this IEnumerable<DraggableCard> cards) => cards.ForEach(card => card.Disappear());//Se deshace una por una de las cartas en el IEnumerable
}
