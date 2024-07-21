using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCreatorUtils
{
    public static int FindMatchingParenthesis(List<Token> tokenList,int pos){//Dado un parentesis de apertura halla su respectivo parentesis de clausura
        Stack<Token> aux=new Stack<Token>();
        aux.Push(tokenList[pos]);//Anadimos el parentesis a hallarle su pareja
        for(int i=pos+1;i<tokenList.Count;i++){
            if(tokenList[i].text=="(" || tokenList[i].text=="[" || tokenList[i].text=="{"){//Si es un parentesis de apertura
            aux.Push(tokenList[i]);//Se anade a la lista
            }else if(tokenList[i].text==")" || tokenList[i].text=="]" || tokenList[i].text=="}"){//Si es un parentesis de clausura
                if(aux.Peek().text==GetMatch(tokenList[i].text)){//El ultimo parentesis de apertura coincide
                        aux.Pop();//Quitamos este par pues ambos son validos
                }
                if(aux.Count==0){
                    return i;//Si nos quedamos sin elementos es porque hallamos la pareja de el pasado como parametro
                }
            }
        }
        //Si no encontramos pareja
        Errors.Write("No encontrado parentesis de clausura '"+aux.Peek().text+"' correspondiente a '"+GetMatch(aux.Peek().text)+"'", aux.Peek().line, aux.Peek().col);
        return -1;
    }
    public static string GetMatch(string par){//Devuelve la pareja del parentesis pasado como argumento
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
