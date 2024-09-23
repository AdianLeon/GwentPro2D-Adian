using UnityEngine;
using TMPro;
//Script para mostrar errores en una pantalla a la derecha del editor de texto
public static class Errors
{
    private static int errorCount;
    private static TextMeshProUGUI errorsText => GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>();
    public static void PureWrite(string message) => errorsText.text = errorsText.text + '\n' + message;//Escribe un mensaje sin mostrar el numero de error

    //Escribe el mensaje pasado como error en el objeto ErrorRead
    public static void Write(string message) => errorsText.text = errorsText.text + '\n' + "Error #" + (++errorCount) + ": " + message;
    public static void Write(string message, Token wrongToken) => errorsText.text = errorsText.text + '\n' + "Error #" + (++errorCount) + ": " + message + ". En linea: " + wrongToken.Line + ", columna: " + wrongToken.Col;
    public static void Write(Token wrongToken) => errorsText.text = errorsText.text + '\n' + "Error #" + (++errorCount) + ": Token inesperado: '" + wrongToken.Text + "'. En linea: " + wrongToken.Line + ", columna: " + wrongToken.Col;
    public static void Write(Token wrongToken, string text) => errorsText.text = errorsText.text + '\n' + "Error #" + (++errorCount) + ": Token inesperado: '" + wrongToken.Text + "'. En linea: " + wrongToken.Line + ", columna: " + wrongToken.Col + ". Se esperaba : '" + text + "'";
    public static void Clean() { errorsText.text = ""; errorCount = 0; }//Reinicia todo en la pantalla de errores
}