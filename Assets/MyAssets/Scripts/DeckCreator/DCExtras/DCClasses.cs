using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardSave{//Clase para guardar todas las propiedades de una carta
    public string faction;//Faccion de la carta
    public string cardName;//Nombre de la carta
    public string description;//Descripcion de la carta
    public string effectDescription;//Descripcion del efecto
    public int powerPoints;//Puntos de la carta sea para el power de las cartas unidades, damage de climas o boost de las cartas aumento
    public string scriptComponent;//Nombre del script de la carta
    public string onActivationName;//Nombre del OnActivation
    public string zones;//Zonas donde se puede jugar en caso de que sea tipo unidad
}
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