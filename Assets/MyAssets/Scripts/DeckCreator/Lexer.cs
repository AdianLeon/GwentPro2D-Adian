using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para transformar el codigo del objeto Compiler a tokens
public class Lexer : MonoBehaviour
{
    private static List<Token> tokenList=new List<Token>();//Lista de tokens (traduccion del codigo)
    public static List<Token> TokenizeCode(string code){//Transforma el string code a una lista de tokens
        tokenList.Clear();
        CheckErrors.ErrorClean();
        if(code.Length>0){
            int lastPos=code.Length-1;
            while(char.IsWhiteSpace(code[lastPos])){lastPos--;}
            if(code[lastPos]=='}'){
                Tokenize(code,0);
                SpecializeWordTokens(tokenList);
                if(CheckErrors.IsCorrect(tokenList)){
                    //Iniciar el analisis de tokens
                    return tokenList;
                }
            }else{
                CheckErrors.ErrorWrite("Token de fin de archivo '}' esperado en linea: "+NewLineCounter(code,code.Length-1)+" columna: "+ColumnCounter(code,code.Length-1),"TokenizeCode");
            }
        }else{
            CheckErrors.ErrorWrite("No hay codigo","TokenizeCode");
        }
        return null;
    }
    private static void Tokenize(string code,int i){
        if(i>=code.Length){//MakeEndToken
            tokenList.Add(new Token("$",i,NewLineCounter(code,i),ColumnCounter(code,i),TokenTypes.end,DepthCounter(code,i)));
            return;
        }else if(char.IsDigit(code[i])){
            MakeNumberToken(code,i,i,false);
        }else if(char.IsLetter(code[i]) || code[i]=='_'){
            MakeWordToken(code,i,i);
        }else if(code[i]=='"'){//MakeLiteralToken
            MakeLiteralToken(code,i,i+1);
        }else if(code[i]=='(' || code[i]=='[' || code[i]=='{' || code[i]==')' || code[i]==']' || code[i]=='}' || code[i].ToString()=="'" ||
                code[i]==',' || code[i]==';' || code[i]=='.' || code[i]==':'){//MakePunctuatorToken
            tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),TokenTypes.punctuator,DepthCounter(code,i)));
            Tokenize(code,i+1);
        }else if(char.IsWhiteSpace(code[i])){//MakeSpaceToken
            //tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),Token.TokenTypes.space,DepthCounter(code,i)));
            Tokenize(code,i+1);
        }else if(code[i]=='@' || (code[i]=='@' && code[i+1]=='@') ||code[i]=='+' || code[i]=='-' || code[i]=='*' || code[i]=='/' || 
                (code[i]=='&' && code[i+1]=='&') || (code[i]=='|' && code[i+1]=='|') || code[i]=='<' || (code[i]=='<' && code[i+1]=='=') ||
                code[i]=='>' || (code[i]=='>' && code[i]+1=='=') || code[i]=='=' || (code[i]=='=' && code[i+1]=='=')){//MakeBinaryOperatorToken
            //********** Make a Method to read the double char ones 
            tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),TokenTypes.binOperator,DepthCounter(code,i)));
            Tokenize(code,i+1);
        }else if(code[i]=='/' && code[i+1]=='/'){//Comentarios
            while(code[i]!='\n' && i<code.Length){i++;}
            Tokenize(code,i+1);
        }else{//MakeBadToken
            tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),TokenTypes.unexpected,DepthCounter(code,i)));
            Tokenize(code,i+1);
        }
    }
    private static void SpecializeWordTokens(List<Token> tokenList){
        string w="";
        for(int i=0;i<tokenList.Count;i++){
            w=tokenList[i].text;
            if((w=="Type" || w=="Name" || w=="Faction" || w=="Power" || w=="Range" || w=="OnActivation") && tokenList[i].depth==1 && tokenList[i+1].text==":"){
                tokenList[i].type=TokenTypes.cardAssignment;
            }else if((w=="card" || w=="effect") && tokenList[i].depth==0 && tokenList[i+1].text=="{"){
                tokenList[i].type=TokenTypes.blockDeclaration;
            }
        }
    }
    private static void MakeNumberToken(string code,int start,int i,bool foundDot){//Crea un token numerico (puede ser int o float) 100.2.2
        while(char.IsDigit(code[i])){//Incrementa el largo del numero mientras se obtengan numeros
            i++;
        }
        if(code[i]=='.'){//Si la cuenta se para por un punto se entiende que recibiremos un float
            if(!foundDot){//Si no hemos encontrado un punto antes
                if(char.IsDigit(code[i+1])){//Y si el proximo numero despues del punto es un numero
                    MakeNumberToken(code,start,i+1,true);//Seguimos contando
                }else{//Si despues del punto no hay un numero
                    tokenList.Add(new Token(code.Substring(start,i-start+1),start,NewLineCounter(code,start),ColumnCounter(code,start),TokenTypes.unexpected,DepthCounter(code,i)));
                    Tokenize(code,i+1);
                }
            }else{//Si ya hemos encontrado un punto antes
                tokenList.Add(new Token(code.Substring(start,i-start+1),start,NewLineCounter(code,start),ColumnCounter(code,start),TokenTypes.unexpected,DepthCounter(code,i)));
                Tokenize(code,i+1);
            }
        }else{
            //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
            tokenList.Add(new Token(code.Substring(start,i-start),start,NewLineCounter(code,start),ColumnCounter(code,start),TokenTypes.number,DepthCounter(code,i)));
            Tokenize(code,i);
        }
    }
    private static void MakeWordToken(string code,int start,int i){//Crea un token palabra
        while(char.IsLetter(code[i]) || char.IsDigit(code[i])){//Mientras el caracter sea una letra sigue avanzando
            i++;
        }
        //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
        tokenList.Add(new Token(code.Substring(start,i-start),start,NewLineCounter(code,start),ColumnCounter(code,start),TokenTypes.identifier,DepthCounter(code,i)));
        Tokenize(code,i);
    }
    private static void MakeLiteralToken(string code,int start,int i){//Crea un token string
        while(code[i]!='"'){//Mientras el caracter no sea una comilla sigue avanzando
            i++;
            if(i==code.Length){
                CheckErrors.ErrorWrite("Pareja de caracter doble comilla no encontrado linea: "+NewLineCounter(code,start)+" columna: "+ColumnCounter(code,start),"MakeLiteralToken");
                tokenList.Add(new Token("",start,NewLineCounter(code,start),ColumnCounter(code,start),TokenTypes.unexpected,DepthCounter(code,i)));
                return;
            }
        }
        //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
        tokenList.Add(new Token(code.Substring(start+1,i-start-1),start,NewLineCounter(code,start),ColumnCounter(code,start),TokenTypes.literal,DepthCounter(code,i)));
        Tokenize(code,i+1);//Hay que llamar la funcion principal una posicion despues de la comilla
    }
    public static int NewLineCounter(string code,int i){//Dados el codigo y la posicion cuenta la cantidad de saltos de linea
        int lines=1;
        for(int j=0;j<i;j++){
            if(code[j]=='\n'){
                lines++;
            }
        }
        return lines;
    }
    public static int ColumnCounter(string code,int i){//Dados el codigo y la posicion cuenta los caracteres anteriores a la palabra en su linea
        int col=0;
        for(int j=0;j<i;j++){
            col++;
            if(code[j]=='\n'){
                col=0;;
            }
        }
        return col;
    }
    private static int DepthCounter(string code,int index){
        List<char> aux=new List<char>();
        for(int i=0;i<index;i++){
            if(code[i]=='(' || code[i]=='[' || code[i]=='{'){
                aux.Add(code[i]);
            }else if(code[i]==')' || code[i]==']' || code[i]=='}'){
                if(aux[aux.Count-1].ToString()==DeckCreatorUtils.ParenthesisMatch(code[i].ToString())){
                    aux.RemoveAt(aux.Count-1);
                }
            }
        }
        return aux.Count;
    }
}