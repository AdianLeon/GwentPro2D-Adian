using System.Collections.Generic;
//Script que contiene algunas utilidades definidas usadas en funciones del DeckCreator
public static class DeckCreatorUtils
{
    public static string FlattenText<T>(this IEnumerable<T> values)
    {//Devuelve los elementos de un IEnumerable convertidos a string entre {}
        string aux = "{";
        values.ForEach(value => aux += " " + value.ToString() + " ");
        return aux + "}";
    }
}