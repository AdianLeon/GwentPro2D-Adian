using System.Collections.Generic;
//Script para guardar algunos metodos auxiliares del Lexer
public static class LexerUtils
{
    //Diccionarios para clasificar los tokens mas facilmente
    public static Dictionary<string, TokenType> Simples = new Dictionary<string, TokenType>
    {
        // Punctuators
        {":",TokenType.punctuator},{";",TokenType.punctuator},{",",TokenType.punctuator},{".",TokenType.punctuator},{"'",TokenType.punctuator},
        //Limitators
        {"(",TokenType.limitator},{")",TokenType.limitator},{"[",TokenType.limitator},{"]",TokenType.limitator},{"{",TokenType.limitator},{"}",TokenType.limitator},
        //BooleanOperators
        {"==",TokenType.booleanOp},{"!=",TokenType.booleanOp},{"<",TokenType.booleanOp},{">",TokenType.booleanOp},{"<=",TokenType.booleanOp},{">=",TokenType.booleanOp},
        {"&&",TokenType.booleanOp},{"||",TokenType.booleanOp},
        //ArithmeticOperators
        {"+",TokenType.arithmeticOp},{"-",TokenType.arithmeticOp},{"*",TokenType.arithmeticOp},{"/",TokenType.arithmeticOp},{"^",TokenType.arithmeticOp},
        {"+=",TokenType.arithmeticOp},{"-=",TokenType.arithmeticOp},{"*=",TokenType.arithmeticOp},{"/=",TokenType.arithmeticOp},{"^=",TokenType.arithmeticOp},
        //ConcatenationOperators
        {"@",TokenType.concatenationOp},{"@@",TokenType.concatenationOp},
        //AssignationOperator
        {"=",TokenType.asignationOp},
        //UnaryOperators
        {"++",TokenType.unaryOp},{"--",TokenType.unaryOp},
        //LambdaOperator
        {"=>",TokenType.lambdaOp},
    };
    public static Dictionary<string, TokenType> ReservedWords = new Dictionary<string, TokenType>
    {
        //BlockDeclaration
        {"card",TokenType.blockDeclaration},{"effect",TokenType.blockDeclaration},
        //Assignments
        {"Name",TokenType.assignment},{"Params",TokenType.assignment},{"Action",TokenType.assignment},{"Type",TokenType.assignment},{"Effect",TokenType.assignment},{"Description",TokenType.assignment},
        {"Selector",TokenType.assignment},{"Source",TokenType.assignment},{"Single",TokenType.assignment},{"Predicate",TokenType.assignment},{"PostAction",TokenType.assignment},
        {"Faction",TokenType.assignment},{"Power",TokenType.assignment},{"Range",TokenType.assignment},{"OnActivation",TokenType.assignment},{"TotalCopies",TokenType.assignment},
        //Cycle
        {"for",TokenType.cycle},{"while",TokenType.cycle},{"in",TokenType.cycle},
        //VariableTypes
        {"Number",TokenType.varType},{"String",TokenType.varType},{"Bool",TokenType.varType},
        //Added
        {"ScriptEffect",TokenType.assignment}
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