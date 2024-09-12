using System.Collections.Generic;
//Script para guardar algunos metodos y declaraciones auxiliares del Lexer
public class Token
{//Clase que almacena las propiedades del token
    public string Text;//Texto del token
    public int Position;//Posicion en el codigo
    public int Line;//Linea donde se encuentra en el texto del objeto Compiler
    public int Col;//Columna donde se encuentra en el texto del objeto Compiler
    public TokenType Type;//Tipos de token
    public Token(string code, int pos, string tokenText, TokenType expectedType = TokenType.unexpected)
    {
        Text = tokenText;
        Position = pos;
        Line = LexerUtils.NewLineCounter(code, pos);
        Col = LexerUtils.ColumnCounter(code, pos);
        if (expectedType != TokenType.unexpected) { Type = expectedType; }
        else if (LexerUtils.Simples.ContainsKey(Text)) { Type = LexerUtils.Simples[Text]; }
        else if (LexerUtils.ReservedWords.ContainsKey(Text)) { Type = LexerUtils.ReservedWords[Text]; }
        else { Type = TokenType.unexpected; }
    }
    public override string ToString() => "   " + Text + "  ---  " + Type + "  linea: " + Line + "   columna: " + Col;
    public bool Is(TokenType type, bool throwError = false) { if (throwError && Type != type) { Errors.Write(this, type.ToString()); } return Type == type; }
    public bool Is(string text, bool throwError = false) { if (throwError && Text != text) { Errors.Write(this, text); } return Text == text; }
}
public enum TokenType
{//Tipos de tokens
    unexpected,
    number,// ... -3 -2 -1 0 1 2 3 ...
    boolean,// true false
    literal,// "Un string"

    //Identificadores
    identifier,
    // assignment,// Name Params Action Type Effect Selector Source Single Predicate Faction Power Range OnActivation PostAction ScriptEffect
    cycle,// for in while
    varType,// Number String Bool

    //Simples
    punctuator,// : ; , .
    limitator,// ( ) [ ] { }
    operatorToken,
    //Operators
    // booleanOp,// == != < > <= >= && ||
    // arithmeticOp,// + - * / ^ += -= *= /= ^=
    // concatenationOp,// @ @@
    // asignationOp,//=
    // unaryOp,// ++ --
    // lambdaOp,// =>
    //Otros
    end// $ 
};
public static class LexerUtils
{
    //Diccionarios para clasificar los tokens mas facilmente
    public static Dictionary<string, TokenType> Simples = new Dictionary<string, TokenType>
    {
        // Punctuators
        {":",TokenType.punctuator},{";",TokenType.punctuator},{",",TokenType.punctuator},{".",TokenType.punctuator},
        //Limitators
        {"(",TokenType.limitator},{")",TokenType.limitator},{"[",TokenType.limitator},{"]",TokenType.limitator},{"{",TokenType.limitator},{"}",TokenType.limitator},
        //BooleanOperators
        {"==",TokenType.operatorToken},{"!=",TokenType.operatorToken},{"<",TokenType.operatorToken},{">",TokenType.operatorToken},{"<=",TokenType.operatorToken},{">=",TokenType.operatorToken},
        {"&&",TokenType.operatorToken},{"||",TokenType.operatorToken},
        //ArithmeticOperators
        {"+",TokenType.operatorToken},{"-",TokenType.operatorToken},{"*",TokenType.operatorToken},{"/",TokenType.operatorToken},{"^",TokenType.operatorToken},
        {"+=",TokenType.operatorToken},{"-=",TokenType.operatorToken},{"*=",TokenType.operatorToken},{"/=",TokenType.operatorToken},{"^=",TokenType.operatorToken},
        //ConcatenationOperators
        {"@",TokenType.operatorToken},{"@@",TokenType.operatorToken},
        //AssignationOperator
        {"=",TokenType.operatorToken},
        //UnaryOperators
        {"++",TokenType.operatorToken},{"--",TokenType.operatorToken},
        //LambdaOperator
        {"=>",TokenType.operatorToken},
    };
    public static Dictionary<string, TokenType> ReservedWords = new Dictionary<string, TokenType>
    {
        //Bool
        {"true",TokenType.boolean},{"false",TokenType.boolean},
        //Cycle
        {"for",TokenType.cycle},{"while",TokenType.cycle},{"in",TokenType.cycle},
        //VariableTypes
        {"Number",TokenType.varType},{"String",TokenType.varType},{"Bool",TokenType.varType}
    };
    public static int NewLineCounter(string code, int index)
    {//Dados el codigo y la posicion cuenta la cantidad de saltos de linea
        int lines = 1;
        for (int i = 0; i < index; i++) { if (code[i] == '\n') { lines++; } }
        return lines;
    }
    public static int ColumnCounter(string code, int index)
    {//Dados el codigo y la posicion cuenta los caracteres anteriores a la palabra en su linea
        int col = 0;
        for (int i = 0; i < index; i++) { col++; if (code[i] == '\n') { col = 0; } }
        return col;
    }
}