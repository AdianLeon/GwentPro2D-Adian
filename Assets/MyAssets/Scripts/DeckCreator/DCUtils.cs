using System;
using System.Collections.Generic;

public static class DeckCreatorUtils
{
    public static string FlattenText<T>(this IEnumerable<T> values)
    {
        string aux = "{";
        values.ForEach(value => aux += " " + value.ToString() + " ");
        return aux + "}";
    }
}