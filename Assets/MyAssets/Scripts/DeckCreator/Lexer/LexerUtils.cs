using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
//Script para guardar algunos metodos auxiliares del Lexer
public static class LexerUtils
{
    //Diccionarios para clasificar los tokens mas facilmente
    public static Dictionary<string,TokenTypes> Simples=new Dictionary<string, TokenTypes>{
        // Punctuators
        {":",TokenTypes.punctuator},{";",TokenTypes.punctuator},{",",TokenTypes.punctuator},{".",TokenTypes.punctuator},{"'",TokenTypes.punctuator},
        //Limitators
        {"(",TokenTypes.limitator},{")",TokenTypes.limitator},{"[",TokenTypes.limitator},{"]",TokenTypes.limitator},{"{",TokenTypes.limitator},{"}",TokenTypes.limitator},
        //BooleanOperators
        {"==",TokenTypes.booleanOp},{"!=",TokenTypes.booleanOp},{"<",TokenTypes.booleanOp},{">",TokenTypes.booleanOp},{"<=",TokenTypes.booleanOp},{">=",TokenTypes.booleanOp},
        {"&&",TokenTypes.booleanOp},{"||",TokenTypes.booleanOp},
        //ArithmeticOperators
        {"+",TokenTypes.arithemticOp},{"-",TokenTypes.arithemticOp},{"*",TokenTypes.arithemticOp},{"/",TokenTypes.arithemticOp},{"^",TokenTypes.arithemticOp},
        {"+=",TokenTypes.arithemticOp},{"-=",TokenTypes.arithemticOp},{"*=",TokenTypes.arithemticOp},{"/=",TokenTypes.arithemticOp},{"^=",TokenTypes.arithemticOp},
        //ConcatenationOperators
        {"@",TokenTypes.concatOp},{"@@",TokenTypes.concatOp},
        //AssignationOperator
        {"=",TokenTypes.asignationOp},
        //UnaryOperators
        {"++",TokenTypes.unaryOp},{"--",TokenTypes.unaryOp},
        //LambdaOperator
        {"=>",TokenTypes.lambdaOp}
    };
    public static Dictionary<string,TokenTypes> ReservedWords=new Dictionary<string, TokenTypes>{
        
    };
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
    public static int DepthCounter(string code,int index){//Cuenta la cantidad de signos de agrupacion que hay que adentrarse para llegar al indice pasado como parametro
        Stack<char> aux=new Stack<char>();
        for(int i=0;i<index;i++){
            if(code[i]=='(' || code[i]=='[' || code[i]=='{'){
                aux.Push(code[i]);
            }else if(code[i]==')' || code[i]==']' || code[i]=='}'){
                if(aux.Peek().ToString()==DeckCreatorUtils.GetMatch(code[i].ToString())){
                    aux.Pop();
                }
            }
        }
        return aux.Count;
    }
}
//Codigo para soportar floats, se debe anadir en los parametros del metodo un bool foundDot, representa si se ha encontrado un punto en algun llamado anterior
// if(code[i]=='.'){//Si la cuenta se para por un punto se entiende que recibiremos un float
//     if(!foundDot){//Si no hemos encontrado un punto antes
//         if(char.IsDigit(code[i+1])){//Y si el proximo numero despues del punto es un numero
//             MakeNumberToken(code,start,i+1,true);//Seguimos contando
//         }else{//Si despues del punto no hay un numero
//             tokenList.Add(new Token(code.Substring(start,i-start+1),start,LexerUtils.NewLineCounter(code,start),LexerUtils.ColumnCounter(code,start),TokenTypes.unexpected,LexerUtils.DepthCounter(code,i)));
//             Tokenize(code,i+1);
//         }
//     }else{//Si ya hemos encontrado un punto antes
//         tokenList.Add(new Token(code.Substring(start,i-start+1),start,LexerUtils.NewLineCounter(code,start),LexerUtils.ColumnCounter(code,start),TokenTypes.unexpected,LexerUtils.DepthCounter(code,i)));
//         Tokenize(code,i+1);
//     }
// }else{
//     //Anade a la lista de tokens el substring desde que empezamos a contar hasta que finalizamos
//     tokenList.Add(new Token(code.Substring(start,i-start),start,LexerUtils.NewLineCounter(code,start),LexerUtils.ColumnCounter(code,start),TokenTypes.number,LexerUtils.DepthCounter(code,i)));
//     Tokenize(code,i);
// }