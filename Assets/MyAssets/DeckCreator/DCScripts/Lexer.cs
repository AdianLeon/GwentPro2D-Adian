using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para transformar el codigo del objeto Compiler a tokens
public class Lexer : MonoBehaviour
{
    public class Token{//Clase que almacena las propiedades del token
        public string text;//Texto del token
        public int position;//Posicion del token (indice con el que se encuentra en el string codigo)
        public int line;//Linea donde se encuentra en el texto del objeto Compiler
        public int col;//Columna donde se encuentra en el texto del objeto Compiler
        //Tipos de token         123   card   ()[]{}     , ;    : =         ' "       + - > < * / || &&
        public enum tokenTypes{number,word,parenthesis,comma,assignation,quote,space,binaryOperator,unexpected};
        public tokenTypes type;
        public Token(string text,int position,int line,int col,tokenTypes type){
            this.text=text;
            this.position=position;
            this.line=line;
            this.col=col;
            this.type=type;
        }
    }

    private static List<Token> tokenList=new List<Token>();//Lista de tokens (traduccion del codigo)
    public static void TokenizeCode(string code){//Transforma el string code a una lista de tokens
        tokenList.Clear();
        CheckTokens.ErrorClean();
        if(code.Length>0){
            if(code[code.Length-1]=='}'){
                Tokenize(code,0);
                if(CheckTokens.IsCorrect(tokenList)){
                    //Iniciar el analisis de tokens
                    Debug.Log("El codigo no tiene errores, inicia el analisis del parser");
                }
            }else{
                CheckTokens.ErrorWrite("Token de fin de archivo '}' esperado en linea: "+NewLineCounter(code,code.Length-1)+" columna: "+ColumnCounter(code,code.Length-1));
            }
        }else{
            CheckTokens.ErrorWrite("No hay codigo");
        }
    }
    public static void Tokenize(string code,int i){
        if(i>=code.Length){//MakeEndToken
            return;
        }else if(char.IsDigit(code[i])){
            MakeNumberToken(code,i,i,false);
        }else if(char.IsLetter(code[i])){
            MakeWordToken(code,i,i);
        }else if(code[i]=='(' || code[i]=='[' || code[i]=='{' || code[i]==')' || code[i]==']' || code[i]=='}'){//MakeParenthesisToken
            tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),Token.tokenTypes.parenthesis));
            Tokenize(code,i+1);
        }else if(code[i]==',' || code[i]==';'){//MakeCommaToken
            tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),Token.tokenTypes.comma));
            Tokenize(code,i+1);
        }else if(code[i]==':' || code[i]=='='){//MakeAssignationToken
            tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),Token.tokenTypes.assignation));
            Tokenize(code,i+1);
        }else if(code[i].ToString()=="'" || code[i]=='"'){//MakeQuoteToken
            tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),Token.tokenTypes.quote));
            Tokenize(code,i+1);
        }else if(char.IsWhiteSpace(code[i])){//MakeSpaceToken
            tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),Token.tokenTypes.space));
            Tokenize(code,i+1);
        }else if(code[i]=='@' || (code[i]=='@' && code[i+1]=='@') ||code[i]=='+' || code[i]=='-' || code[i]=='*' || code[i]=='/' || (code[i]=='&' && code[i+1]=='&') || (code[i]=='|' && code[i+1]=='|')){//MakeBinaryOperatorToken
            tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),Token.tokenTypes.binaryOperator));
            Tokenize(code,i+1);
        }else if(code[i]=='/' && code[i+1]=='/'){//Comentarios
            while(code[i]!='\n' && i<code.Length){i++;}
            Tokenize(code,i+1);
        }else{//MakeBadToken
            tokenList.Add(new Token(code[i].ToString(),i,NewLineCounter(code,i),ColumnCounter(code,i),Token.tokenTypes.unexpected));
            Tokenize(code,i+1);
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
                    tokenList.Add(new Token(code.Substring(start,i-start+1),start,NewLineCounter(code,start),ColumnCounter(code,start),Token.tokenTypes.unexpected));
                    Tokenize(code,i+1);
                }
            }else{//Si ya hemos encontrado un punto antes
                tokenList.Add(new Token(code.Substring(start,i-start+1),start,NewLineCounter(code,start),ColumnCounter(code,start),Token.tokenTypes.unexpected));
                Tokenize(code,i+1);
            }
        }else{
            //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
            tokenList.Add(new Token(code.Substring(start,i-start),start,NewLineCounter(code,start),ColumnCounter(code,start),Token.tokenTypes.number));
            Tokenize(code,i);
        }
    }
    private static void MakeWordToken(string code,int start,int i){//Crea un token palabra
        while(char.IsLetter(code[i])){//Mientras el caracter sea una letra sigue avanzando
            i++;
        }
        //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
        tokenList.Add(new Token(code.Substring(start,i-start),start,NewLineCounter(code,start),ColumnCounter(code,start),Token.tokenTypes.word));
        Tokenize(code,i);
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
}