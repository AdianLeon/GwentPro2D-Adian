using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assigner : MonoBehaviour
{
    public static void AssignBlocks(List<Token> tokenList){
        for(int i=0;i<tokenList.Count;i++){
            if(tokenList[i].type==tokenTypes.blockDeclaration){
                if(tokenList[i+1].text=="{"){
                    int blockEnd=Utils.FindMatchingParenthesis(tokenList,i+1);
                    if(tokenList[i].text=="card"){
                    ProcessCard.CompileAndCreate(tokenList,i+2,blockEnd);
                    }else if(tokenList[i].text=="effect"){
                    ProcessEffect.SaveEffectOnJson(tokenList,i+2,blockEnd);
                    }else{
                        Debug.Log("Error, cardAssignmentWord is not card or effect");
                    }
                }else{
                    CheckErrors.ErrorWrite("Se ha encontrado caracter "+tokenList[i+1].text+" en vez de '{' luego de declaracion de bloque linea: "+tokenList[i].line+" columna: "+tokenList[i].col,"AssignBlocks");
                }
            }
        }
    }
    
}
