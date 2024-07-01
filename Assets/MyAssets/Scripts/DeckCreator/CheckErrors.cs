using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para asegurar que no se cometan algunos errores frecuentes en el codigo
public class CheckErrors : MonoBehaviour
{
    private static int errorCount;
    public static bool IsCorrect(List<Token> tokenList){
        return CheckUnexpectedTokens(tokenList);//No existan token sin clasificar
    }
    public static void ErrorWrite(string message,string writer){//Escribe el mensaje pasado como error en el objeto ErrorRead
        GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text=GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text+'\n'/*+"Error #"+(++errorCount)+": "+message+". Encontrado por: "+writer*/;
    }
    public static void ErrorClean(){
        GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text="";
    }
    private static bool CheckUnexpectedTokens(List<Token> tokenList){//Chequea si hay algun token inesperado
        bool isCorrect=true;
        for(int i=0;i<tokenList.Count;i++){
            if(tokenList[i].type==tokenTypes.unexpected){
                isCorrect=false;
            }
            if(!(tokenList[i].text=="")){
                ErrorWrite("Token inesperado: '"+tokenList[i].text+"' en linea: "+tokenList[i].line+" columna: "+tokenList[i].col,"CheckUnexpectedTokens");
            }
        }
        return isCorrect;
    }
}