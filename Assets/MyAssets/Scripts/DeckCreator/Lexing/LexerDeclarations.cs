public class Token
{//Clase que almacena las propiedades del token
    public string text;//Texto del token
    public int position;//Posicion en el codigo
    public int line;//Linea donde se encuentra en el texto del objeto Compiler
    public int col;//Columna donde se encuentra en el texto del objeto Compiler
    //Tipos de token
    public TokenType type;
    public Token(string code, int pos, string tokenText, TokenType expectedType = TokenType.unexpected)
    {
        text = tokenText;
        position = pos;
        line = LexerUtils.NewLineCounter(code, position);
        col = LexerUtils.ColumnCounter(code, position);
        if (expectedType != TokenType.unexpected) { type = expectedType; }
        else if (LexerUtils.Simples.ContainsKey(text)) { type = LexerUtils.Simples[text]; }
        else if (LexerUtils.ReservedWords.ContainsKey(text)) { type = LexerUtils.ReservedWords[text]; }
        else { type = TokenType.unexpected; }
    }
    public override string ToString() => "   " + text + "  ---  " + type + "  linea: " + line + "   columna: " + col;
    public bool Is(TokenType type, bool throwError = false) { if (throwError && this.type != type) { Errors.Write(this, type.ToString()); } return this.type == type; }
    public bool Is(string text, bool throwError = false) { if (throwError && this.text != text) { Errors.Write(this, text); } return this.text == text; }
}
public enum TokenType
{//Tipos de tokens
    number,// ... -3 -2 -1 0 1 2 3 ...
    boolean,// true false
    literal,// "Un string"

    //Identificadores
    identifier,
    assignment,// Name Params Action Type Effect Selector Source Single Predicate Faction Power Range OnActivation PostAction ScriptEffect
    cycle,// for in while
    varType,// Number String Bool
    blockDeclaration,//card effect

    //Simples
    punctuator,// : ; , . " '
    limitator,// ( ) [ ] { }
              //Operators
    booleanOp,// == != < > <= >= && ||
    arithmeticOp,// + - * / ^ += -= *= /= ^=
    concatenationOp,// @ @@
    asignationOp,//=
    unaryOp,// ++ --
    lambdaOp,// =>

    //Otros
    end,// $
    unexpected// ?
};
