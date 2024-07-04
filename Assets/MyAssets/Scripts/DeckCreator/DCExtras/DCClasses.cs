using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Token{//Clase que almacena las propiedades del token
    public string text;//Texto del token
    public int position;//Posicion del token (indice con el que se encuentra en el string codigo)
    public int line;//Linea donde se encuentra en el texto del objeto Compiler
    public int col;//Columna donde se encuentra en el texto del objeto Compiler
    //Tipos de token
    public TokenTypes type;
    public int depth;
    public Token(string text,int position,int line,int col,TokenTypes type,int depth){
        this.text=text;
        this.position=position;
        this.line=line;
        this.col=col;
        this.type=type;
        this.depth=depth;
    }
}