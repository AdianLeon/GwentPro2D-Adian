using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static int FindMatchingParenthesis(List<Token> tokenList,int pos){//Dado un parentesis de apertura halla su respectivo parentesis de clausura
        List<Token> aux=new List<Token>();
        aux.Add(tokenList[pos]);//Anadimos el parentesis a hallarle su pareja
        for(int i=pos+1;i<tokenList.Count;i++){
            if(tokenList[i].text=="(" || tokenList[i].text=="[" || tokenList[i].text=="{"){//Si es un parentesis de apertura
            aux.Add(tokenList[i]);//Se anade a la lista
            }else if(tokenList[i].text==")" || tokenList[i].text=="]" || tokenList[i].text=="}"){//Si es un parentesis de clausura
                if(aux[aux.Count-1].text==Utils.ParenthesisMatch(tokenList[i].text)){//El ultimo parentesis de apertura coincide
                        aux.RemoveAt(aux.Count-1);//Quitamos este par pues ambos son validos
                }
                if(aux.Count==0){
                    return i;//Si nos quedamos sin elementos es porque hallamos la pareja de el pasado como parametro
                }
            }
        }
        for(int i=0;i<aux.Count;i++){
            CheckErrors.ErrorWrite("No encontrado parentesis de clausura '"+aux[i].text+"' correspondiente a "+ParenthesisMatch(aux[i].text)+" en linea: "+aux[i].line+" columna: "+aux[i].col,"FindMatchingParentesis");
        }
        return tokenList.Count;//Si no encontramos pareja devolvemos un valor fuera de rango
    }
    public static string ParenthesisMatch(string par){//Devuelve la pareja del parentesis pasado como argumento
        string[] allpars={"(",")","[","]","{","}"};
        int posOfPar=0;
        for(int i=0;i<allpars.Length;i++){
            if(allpars[i]==par){
                posOfPar=i;
                break;
            }
        }
        if(posOfPar%2==0){
            return allpars[posOfPar+1];
        }else{
            return allpars[posOfPar-1];
        }
    }
    public static void AddTokensTo(List<Token> targetList,List<Token> tokenList,int start,int end){
        for(int i=start;i<end;i++){
            targetList.Add(tokenList[i]);
        }
    }
}
