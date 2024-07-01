using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCompiler : MonoBehaviour
{
    public static void ProcessText(string allText){
        List <Token> tokenList=Lexer.TokenizeCode(allText);
        if(tokenList!=null){
            Assigner.AssignBlocks(tokenList);
        }
    }
}
