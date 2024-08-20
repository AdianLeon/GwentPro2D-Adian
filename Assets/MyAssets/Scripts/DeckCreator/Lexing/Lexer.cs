using System.Collections.Generic;
//Script para transformar el codigo del objeto Compiler a tokens
public static class Lexer
{
    private static List<Token> tokenList;//Lista de tokens 
    public static List<Token> TokenizeCode(string code)
    {//Transforma el string code a una lista de tokens
        tokenList = new List<Token>();
        if (code.Length == 0) { Errors.Write("No hay codigo"); return null; }//Si no hay texto
        code += "$";
        Tokenize(code, 0);//Comenzamos a tokenizar
        if (!Errors.CheckUnexpectedTokens(tokenList)) { return null; }//Si hay errores no devolvemos lista
        return tokenList;
    }
    private static void Tokenize(string code, int i)
    {
        //EndToken---------------------------------------------------------------------------------------------------------
        if (i >= code.Length || code[i] == '$') { tokenList.Add(new Token(code, i, "$", TokenType.end)); return; }
        //Numeros---------------------------------------------------------------------------------------------------------
        else if (char.IsDigit(code[i])) { MakeNumberToken(code, i); }
        //Identificadores---------------------------------------------------------------------------------------------------------
        else if (char.IsLetter(code[i]) || code[i] == '_') { MakeIdentifierToken(code, i); }
        //Literales (strings)---------------------------------------------------------------------------------------------------------
        else if (code[i] == '"') { MakeLiteralToken(code, i); }
        //Comentarios
        else if (code[i] == '/' && code[i + 1] == '/') { while (i < code.Length && code[i] != '\n') { i++; }; Tokenize(code, i + 1); }
        //Tokens simples (puntuadores, parentesis y operadores)---------------------------------------------------------------------------------------------------------
        else if (LexerUtils.Simples.ContainsKey(code[i].ToString() + code[i + 1].ToString()))
        {//MakeSimpleToken si ocupa dos caracteres
            tokenList.Add(new Token(code, i, code[i].ToString() + code[i + 1].ToString()));
            Tokenize(code, i + 2);
        }
        else if (LexerUtils.Simples.ContainsKey(code[i].ToString()))
        {//MakeSimpleToken si ocupa un caracter
            tokenList.Add(new Token(code, i, code[i].ToString()));
            Tokenize(code, i + 1);
        }
        else if (char.IsWhiteSpace(code[i]))
        {//Ignora los espacios
            Tokenize(code, i + 1);
        }
        //Inesperado---------------------------------------------------------------------------------------------------------
        else { tokenList.Add(new Token(code, i, code[i].ToString())); }
    }
    private static void MakeNumberToken(string code, int start)
    {//Crea un token numerico
        int end = start;
        while (char.IsDigit(code[end])) { end++; }//Incrementa el largo del numero mientras se obtengan numeros
        //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
        tokenList.Add(new Token(code, start, code.Substring(start, end - start), TokenType.number));
        Tokenize(code, end);
    }
    private static void MakeIdentifierToken(string code, int start)
    {//Crea un token identificador
        int end = start;
        while (char.IsLetter(code[end]) || char.IsDigit(code[end])) { end++; }//Mientras el caracter sea una letra o un numero sigue avanzando
        //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
        string identifierText = code.Substring(start, end - start);
        TokenType identifierType = TokenType.identifier;
        if (LexerUtils.ReservedWords.ContainsKey(identifierText)) { identifierType = LexerUtils.ReservedWords[identifierText]; }
        tokenList.Add(new Token(code, start, identifierText, identifierType));
        Tokenize(code, end);
    }
    private static void MakeLiteralToken(string code, int start)
    {//Crea un token string
        int end = start + 1;
        while (code[end] != '"')
        {//Mientras el caracter no sea una comilla sigue avanzando
            end++;
            if (end == code.Length)
            {
                Errors.Write("Pareja de caracter doble comilla no encontrado", LexerUtils.NewLineCounter(code, start), LexerUtils.ColumnCounter(code, start));
                tokenList.Add(new Token(code, start, "", TokenType.unexpected));
                return;
            }
        }
        //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
        tokenList.Add(new Token(code, start, code.Substring(start + 1, end - start - 1), TokenType.literal));
        Tokenize(code, end + 1);//Hay que llamar la funcion principal una posicion despues de la comilla
    }
}