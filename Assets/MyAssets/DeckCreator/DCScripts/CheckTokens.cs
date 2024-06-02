using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckTokens : MonoBehaviour
{
    public static bool IsCorrect(List<Lexer.Token> tokenList){
        bool errorsFound=false;
        List<Lexer.Token> parenthesisList=new List<Lexer.Token>();
        List<Lexer.Token> quotesList=new List<Lexer.Token>(); 
        for(int i=0;i<tokenList.Count;i++){
            if(tokenList[i].type==Lexer.Token.tokenTypes.parenthesis){
                parenthesisList.Add(tokenList[i]);
            }else if(tokenList[i].type==Lexer.Token.tokenTypes.quote){
                quotesList.Add(tokenList[i]);
            }
        }
        errorsFound=CheckMatchingParenthesis(parenthesisList) || errorsFound;
        errorsFound=CheckMatchingQuotes(quotesList) || errorsFound;
        for(int i=0;i<tokenList.Count;i++){
            if(tokenList[i].type==Lexer.Token.tokenTypes.unexpected){
                //Se escribe el error en la consola
                ErrorWrite("Token inesperado: '"+tokenList[i].text+"' en linea: "+tokenList[i].line+" columna: "+tokenList[i].col);
                errorsFound=true;
            }
        }
        return !errorsFound;//Si se encuentran errores se devuelve false, y si no se devuelve true
    }
    public static void ErrorWrite(string message){
        GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text=GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text+'\n'+"Error: "+message;
    }
    public static void ErrorClean(){
        GameObject.Find("ErrorRead").GetComponent<TextMeshProUGUI>().text="";
    }
    public static bool CheckMatchingParenthesis(List<Lexer.Token> all){//Chequea si los parentesis del codigo estan balanceados
        bool parError=false;
        List<Lexer.Token> aux=new List<Lexer.Token>();//Creamos otra lista auxiliar
        for(int i=0;i<all.Count;i++){
            if(all[i].text=="(" || all[i].text=="[" || all[i].text=="{"){//Si es un parentesis de apertura
                aux.Add(all[i]);//Se anade a la lista
            }else{//Si es un parentesis de clausura
                if(aux.Count==0){//No hay parentesis de apertura
                    ErrorWrite("Token '"+ParMatch(all[i].text)+"' esperado antes de linea: "+all[i].line+" columna: "+all[i].col);
                    parError=true;
                }else{
                    if(aux[aux.Count-1].text==ParMatch(all[i].text)){//El ultimo parentesis de apertura coincide
                        aux.RemoveAt(aux.Count-1);//Quitamos este par pues ambos son validos
                    }else{//No coincide
                        ErrorWrite("Token '"+ParMatch(all[i].text)+"' esperado antes de linea: "+all[i].line+" columna: "+all[i].col);
                        parError=true;
                    }
                }
            }
        }
        if(aux.Count>0){
            parError=true;
            for(int i=0;i<aux.Count;i++){
                ErrorWrite("Token '"+all[i].text+"' no tiene parentesis de clausura correspondiente linea: "+all[i].line+" columna: "+all[i].col);
            }
        }
        return parError;
    }
    public static bool CheckMatchingQuotes(List<Lexer.Token> quotesList){
        if(quotesList.Count>0){
            bool quoteError=false;
            int sing=0;//Cantidad de '
            int doub=0;//Cantidad de "
            Lexer.Token singT=quotesList[0];
            Lexer.Token doubT=quotesList[0];
            for(int i=0;i<quotesList.Count;i++){
                if(quotesList[i].text=="'"){
                    sing++;
                    singT=quotesList[i];
                }else{
                    doub++;
                    doubT=quotesList[i];
                }
            }
            if(sing%2!=0){
                ErrorWrite("Token comilla simple respectivo a linea: "+singT.line+" columna: "+singT.col+" no encontrado ");
                quoteError=true;
            }else if(doub%2!=0){
                ErrorWrite("Token comilla doble respectivo a linea: "+doubT.line+" columna: "+doubT.col+" no encontrado ");
                quoteError=true;
            }
            return quoteError;
        }else{
            return false;
        }
    }
    public static string ParMatch(string par){
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
}