using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script que declara las clases a utilizar de todo el proyecto
[System.Serializable]
public class CardSave{//Clase para guardar todas las propiedades de una carta
    public string faction;//Faccion de la carta
    public string cardName;//Nombre de la carta
    public string description;//Descripcion de la carta
    public string effectDescription;//Descripcion del efecto
    public int powerPoints;//Puntos de la carta sea para el power de las cartas unidades, damage de climas o boost de las cartas aumento
    public string[] scriptComponents;//Lista de nombres de los scripts de la carta
    public string onActivationCodeName;//Nombre del Codigo del efecto
    public string zones;//Zonas donde se puede jugar en caso de que sea tipo unidad
}
[System.Serializable]
public class PlayerPrefsData{//Clase para guardar las preferencias del jugador
        public float volume;
        public string deckPrefP1;
        public string deckPrefP2;
        public PlayerPrefsData(float volume,string deckPrefP1,string deckPrefP2){
            this.volume=volume;
            this.deckPrefP1=deckPrefP1;
            this.deckPrefP2=deckPrefP2;
        }
    }
public class Token{//Clase que almacena las propiedades del token
    public string text;//Texto del token
    public int position;//Posicion del token (indice con el que se encuentra en el string codigo)
    public int line;//Linea donde se encuentra en el texto del objeto Compiler
    public int col;//Columna donde se encuentra en el texto del objeto Compiler
    //Tipos de token
    public tokenTypes type;
    public int depth;
    public Token(string text,int position,int line,int col,tokenTypes type,int depth){
        this.text=text;
        this.position=position;
        this.line=line;
        this.col=col;
        this.type=type;
        this.depth=depth;
    }
}