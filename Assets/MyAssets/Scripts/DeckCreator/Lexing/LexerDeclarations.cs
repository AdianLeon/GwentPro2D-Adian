public class Token
{//Clase que almacena las propiedades del token
    public string Text;//Texto del token
    public int Position;//Posicion en el codigo
    public int Line;//Linea donde se encuentra en el texto del objeto Compiler
    public int Col;//Columna donde se encuentra en el texto del objeto Compiler
    //Tipos de token
    public TokenType Type;
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
