using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Script para transformar el codigo del objeto Compiler a tokens
public static class Lexer
{
    private static List<Token> tokenList=new List<Token>();//Lista de tokens
    public static List<Token> TokenizeCode(string code){//Transforma el string code a una lista de tokens
        tokenList.Clear();
        Errors.Clean();
        if(code.Length==0){//Si no hay codigo
            Errors.Write("No hay codigo");
            return null;
        }
        int lastPos=code.Length-1;//Limpiamos todos los espacios que se encuentren al final
        while(char.IsWhiteSpace(code[lastPos])){lastPos--;}
        if(code[lastPos]!='}'){//Si el ultimo caracter no es una llave de cierre
            Errors.Write("Esperado token de fin de archivo '}'.", LexerUtils.NewLineCounter(code,code.Length-1), LexerUtils.ColumnCounter(code,code.Length-1));
            return null;
        }
        code+="$";
        //Comenzamos a tokenizar
        Tokenize(code,0);
        if(!Errors.CheckUnexpectedTokens(tokenList)){//Si hay errores no devolvemos lista
            return null;
        }
        //Iniciar el analisis avanzado de tokens
        SpecializeIdentifierTokens(tokenList);
        return tokenList;
    }
    private static void Tokenize(string code,int i){
        if(code[i]=='$' || i>=code.Length){//EndToken
            tokenList.Add(new Token("$",LexerUtils.NewLineCounter(code,i),LexerUtils.ColumnCounter(code,i),TokenTypes.end,LexerUtils.DepthCounter(code,i)));
            return;
        }
        //Numeros y palabras
        else if(char.IsDigit(code[i])){
            MakeNumberToken(code,i,i);
        }else if(char.IsLetter(code[i]) || code[i]=='_'){
            MakeIdentifierToken(code,i,i);
        }
        //Literales (strings)
        else if(code[i]=='"'){
            MakeLiteralToken(code,i,i+1);
        }
        //Tokens simples (puntuadores, parentesis y operadores)
        else if(LexerUtils.Simples.ContainsKey(code[i].ToString()+code[i+1].ToString())){//MakeSimpleToken si ocupa dos caracteres
            tokenList.Add(new Token(code[i].ToString()+code[i+1].ToString(),LexerUtils.NewLineCounter(code,i),LexerUtils.ColumnCounter(code,i),LexerUtils.Simples[code[i].ToString()+code[i+1].ToString()],LexerUtils.DepthCounter(code,i)));
            Tokenize(code,i+2);
        }else if(LexerUtils.Simples.ContainsKey(code[i].ToString())){//MakeSimpleToken si ocupa un caracter
            tokenList.Add(new Token(code[i].ToString(),LexerUtils.NewLineCounter(code,i),LexerUtils.ColumnCounter(code,i),LexerUtils.Simples[code[i].ToString()],LexerUtils.DepthCounter(code,i)));
            Tokenize(code,i+1);
        }
        //Extra
        else if(code[i]=='/' && code[i+1]=='/'){//Comentarios
            while(code[i]!='\n' && i<code.Length){i++;}
            Tokenize(code,i+1);
        }else if(char.IsWhiteSpace(code[i])){//Ignora los espacios
            Tokenize(code,i+1);
        }else{//Inesperado
            tokenList.Add(new Token(code[i].ToString(),LexerUtils.NewLineCounter(code,i),LexerUtils.ColumnCounter(code,i),TokenTypes.unexpected,LexerUtils.DepthCounter(code,i)));
        }
    }
    private static void SpecializeIdentifierTokens(List<Token> tokenList){
        for(int i=0;i<tokenList.Count;i++){
            if(tokenList[i].type!=TokenTypes.identifier){
                continue;
            }
            string w=tokenList[i].text;
            if(LexerUtils.ReservedWords.ContainsKey(w)){
                tokenList[i].type=LexerUtils.ReservedWords[w];
            }
        }
    }
    private static void MakeNumberToken(string code,int start,int end){//Crea un token numerico
        while(char.IsDigit(code[end])){//Incrementa el largo del numero mientras se obtengan numeros
            end++;
        }
        //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
        tokenList.Add(new Token(code.Substring(start,end-start),LexerUtils.NewLineCounter(code,start),LexerUtils.ColumnCounter(code,start),TokenTypes.number,LexerUtils.DepthCounter(code,end)));
        Tokenize(code,end);
    }
    private static void MakeIdentifierToken(string code,int start,int end){//Crea un token identificador
        while(char.IsLetter(code[end]) || char.IsDigit(code[end])){//Mientras el caracter sea una letra o un numero sigue avanzando
            end++;
        }
        //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
        tokenList.Add(new Token(code.Substring(start,end-start),LexerUtils.NewLineCounter(code,start),LexerUtils.ColumnCounter(code,start),TokenTypes.identifier,LexerUtils.DepthCounter(code,end)));
        Tokenize(code,end);
    }
    private static void MakeLiteralToken(string code,int start,int end){//Crea un token string
        while(code[end]!='"'){//Mientras el caracter no sea una comilla sigue avanzando
            end++;
            if(end==code.Length){
                Errors.Write("Pareja de caracter doble comilla no encontrado", LexerUtils.NewLineCounter(code,start), LexerUtils.ColumnCounter(code,start));
                tokenList.Add(new Token("",LexerUtils.NewLineCounter(code,start),LexerUtils.ColumnCounter(code,start),TokenTypes.unexpected,LexerUtils.DepthCounter(code,end)));
                return;
            }
        }
        //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
        tokenList.Add(new Token(code.Substring(start+1,end-start-1),LexerUtils.NewLineCounter(code,start),LexerUtils.ColumnCounter(code,start),TokenTypes.literal,LexerUtils.DepthCounter(code,end)));
        Tokenize(code,end+1);//Hay que llamar la funcion principal una posicion despues de la comilla
    }
}