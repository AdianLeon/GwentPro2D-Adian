using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public static class MainCompiler
{
    public static void ProcessText(string allText){
        List <Token> tokenList=Lexer.TokenizeCode(allText);
        if(tokenList==null){
            Errors.Write("No se pudo procesar el codigo!");
            return;
        }
        AssignBlocks(tokenList);
    }
    private static void AssignBlocks(List<Token> tokenList){
        for(int i=0;i<tokenList.Count;i++){
            if(tokenList[i].type!=TokenTypes.blockDeclaration){
                continue;
            }
            if(tokenList[i+1].text!="{"){
                Errors.Write("Se ha encontrado: '"+tokenList[i+1].text+"' en vez de '{' luego de declaracion de bloque", tokenList[i].line, tokenList[i].col);
                return;
            }
            int blockEnd=DeckCreatorUtils.FindMatchingParenthesis(tokenList,i+1);
            if(tokenList[i].text=="card"){
                ProcessCard.CompileAndCreate(tokenList,i+2,blockEnd);
            }else if(tokenList[i].text=="effect"){
                ProcessEffect.StartProcessEffect(tokenList,i+2,blockEnd);
            }
        }
    }
}
