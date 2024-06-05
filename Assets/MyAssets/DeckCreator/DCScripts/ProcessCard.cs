using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessCard : MonoBehaviour
{
    public class SaveInstruction{
        public string key;
        public string value;
    }
    public static void CompileAndCreate(List<CustomClasses.Token> tokenList,int start,int end){
        //CardsToJson.CardSave cardToJson=new CardsToJson.CardSave();
        Dictionary<string,string> instructions=new();

        Debug.Log("Tokens in list:");
        for(int i=start;i<end;i++){
            Debug.Log(tokenList[i].text+"  --  "+tokenList[i].type.ToString()+"  depth: "+tokenList[i].depth);
        }
        for(int i=start;i<end;i++){
            if(tokenList[i].type==CustomClasses.Token.tokenTypes.cardAssignment && tokenList[i+1].text==":"){

                instructions.Add(tokenList[i].text,GetInstructionValue(tokenList,i+2,tokenList[i].text));

                if(instructions[tokenList[i].text]==""){
                    CheckErrors.ErrorWrite("Valor no asignado a "+tokenList[i].text+" en linea: "+tokenList[i].line+" columna: "+tokenList[i].col,"CompileAndCreate");
                }
            }
        }
        Debug.Log("Instructions in list:");
        foreach (string key in instructions.Keys){
            Debug.Log("Key: "+key+" Value: "+instructions[key]);
        }

    }
    private static string GetInstructionValue(List<CustomClasses.Token> tokenList,int index,string nameOfKey){
        if(tokenList[index].type==CustomClasses.Token.tokenTypes.literal){
            return tokenList[index].text;
        }else if(tokenList[index].type==CustomClasses.Token.tokenTypes.number){
            return tokenList[index].text;
        }else{
            if(nameOfKey=="Range" || nameOfKey=="OnActivation"){
                if(tokenList[index].text=="["){
                    string ans="";
                    int matchPos=Utils.FindMatchingParenthesis(tokenList,index);
                    for(int i=index;i<=matchPos;i++){//Copiamos todos los elementos hasta el parentesis (incluyendo el parentesis)
                        ans+=tokenList[i].text;
                    }
                    return ans;
                }else{
                    CheckErrors.ErrorWrite("Propiedad "+nameOfKey+" no definida correctamente (falta iniciar con '[') linea: "+tokenList[index].line+" columna: "+tokenList[index].col,"GetInstructionValue");
                    return "";
                }
            }else{
                return "";
            }
        }
    }
}