using System;
using System.Collections.Generic;

public static class DeckCreatorUtils
{
    public static string GetText<T>(this IEnumerable<T> values)
    {
        string aux = "{";
        values.ForEach(value => aux += " " + value.ToString() + " ");
        return aux + "}";
    }
    public static int FindMatchingParenthesis(List<Token> tokenList, int pos)
    {//Dado un parentesis de apertura halla su respectivo parentesis de clausura
        Stack<Token> aux = new Stack<Token>();
        aux.Push(tokenList[pos]);//Anadimos el parentesis a hallarle su pareja
        for (int i = pos + 1; i < tokenList.Count; i++)
        {
            if (tokenList[i].Text == "(" || tokenList[i].Text == "[" || tokenList[i].Text == "{")
            {//Si es un parentesis de apertura
                aux.Push(tokenList[i]);//Se anade a la lista
            }
            else if (tokenList[i].Text == ")" || tokenList[i].Text == "]" || tokenList[i].Text == "}")
            {//Si es un parentesis de clausura
                if (aux.Peek().Text == GetMatchOf(tokenList[i].Text))
                {//El ultimo parentesis de apertura coincide
                    aux.Pop();//Quitamos este par pues ambos son validos
                }
                if (aux.Count == 0)
                {
                    return i;//Si nos quedamos sin elementos es porque hallamos la pareja de el pasado como parametro
                }
            }
        }
        //Si no encontramos pareja
        Errors.Write("No encontrado parentesis de clausura '" + aux.Peek().Text + "' correspondiente a '" + GetMatchOf(aux.Peek().Text) + "'", aux.Peek());
        return -1;
    }
    public static string GetMatchOf(string par)
    {//Devuelve la pareja del parentesis pasado como argumento
        string[] allpars = { "(", ")", "[", "]", "{", "}" };
        int posOfPar = Array.IndexOf(allpars, par);

        if (posOfPar % 2 == 0) { return allpars[posOfPar + 1]; }
        else { return allpars[posOfPar - 1]; }
    }
}
