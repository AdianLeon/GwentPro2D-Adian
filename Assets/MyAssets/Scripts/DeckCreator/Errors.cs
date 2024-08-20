using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para asegurar que no se cometan algunos errores frecuentes en el codigo
public static class Errors
{
    private static int errorCount;
    private static TextMeshProUGUI errorsText = GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>();
    //Escribe el mensaje pasado como error en el objeto ErrorRead
    public static void Write(string message) => errorsText.text = errorsText.text + '\n' + "Error #" + (++errorCount) + ": " + message;

    public static void Write(string message, int line, int col) => errorsText.text = errorsText.text + '\n' + "Error #" + (++errorCount) + ": " + message + ". En linea: " + line + ", columna: " + col;

    public static void Write(string message, Token wrongToken) => errorsText.text = errorsText.text + '\n' + "Error #" + (++errorCount) + ": " + message + ". En linea: " + wrongToken.line + ", columna: " + wrongToken.col;

    public static void Write(Token wrongToken) => errorsText.text = errorsText.text + '\n' + "Error #" + (++errorCount) + ": Token inesperado. Encontrado en linea: " + wrongToken.line + ", columna: " + wrongToken.col;

    public static void Write(Token wrongToken, string text) => errorsText.text = errorsText.text + '\n' + "Error #" + (++errorCount) + ": Token inesperado. Encontrado en linea: " + wrongToken.line + ", columna: " + wrongToken.col + ". Se esperaba : '" + text + "'";

    public static void Clean() { errorsText.text = ""; errorCount = 0; }
    public static bool CheckUnexpectedTokens(List<Token> tokens)
    {//Chequea si hay algun token inesperado
        bool isCorrect = true;
        foreach (Token token in tokens) { if (token.type == TokenType.unexpected) { Write(token); isCorrect = false; } }
        return isCorrect;
    }
}