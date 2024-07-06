using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para asegurar que no se cometan algunos errores frecuentes en el codigo
public static class Errors
{
    private static int errorCount;
    public static void Write(string message){//Escribe el mensaje pasado como error en el objeto ErrorRead
        GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text=GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text+'\n'+"Error #"+(++errorCount)+": "+message;
    }
    public static void Write(string message, int line, int col){
        GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text=GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text+'\n'+"Error #"+(++errorCount)+": "+message+". En linea: "+line+", columna: "+col;
    }
    public static void Write(string message, Token wrongToken){
        GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text=GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text+'\n'+"Error #"+(++errorCount)+": "+message+". En linea: "+wrongToken.line+", columna: "+wrongToken.col;
    }
    public static void Write(Token wrongToken){
        GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text=GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text+'\n'+"Error #"+(++errorCount)+": Token inesperado. Encontrado en linea: "+wrongToken.line+", columna: "+wrongToken.col;
    }
    public static void Clean(){
        GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text="";
    }
    public static bool CheckUnexpectedTokens(List<Token> tokenList){//Chequea si hay algun token inesperado
        bool isCorrect=true;
        for(int i=0;i<tokenList.Count;i++){
            if(tokenList[i].type==TokenTypes.unexpected){
                isCorrect=false;
                Write(tokenList[i]);
            }
        }
        return isCorrect;
    }
}