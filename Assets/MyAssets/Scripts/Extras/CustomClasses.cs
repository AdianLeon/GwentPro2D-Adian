using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomClasses : MonoBehaviour
{
    public class CardSave{//Clase para guardar todas las propiedades de una carta
        public string faction;//Faccion de la carta
        public string cardRealName;//Nombre de la carta
        public string description;//Descripcion de la carta
        public string effectDescription;//Descripcion del efecto
        public string sourceImage;//Imagen del objeto
        public string artwork;//Imagen para mostrar en el CardView
        public string qualitySprite;//Imagen de la calidad
        public float r;//Dato del color rojo de la carta
        public float g;//Dato del color verde de la carta
        public float b;//Dato del color azul de la carta
        public int powerPoints;//Puntos de la carta sea para el power de las cartas unidades, damage de climas o boost de las cartas aumento
        public string typeComponent;//Nombre del tipo de carta
        public string[] effectComponents;//Lista de nombres de los componentes efecto
        public List<Token> effectCode;//Codigo del efecto
        public string zones;//Zonas donde se puede jugar en caso de que sea tipo unidad
        public string quality;//Calidad de la carta en caso de que sea tipo Unidad
        public CardSave(string faction,string cardRealName,string description,string effectDescription,string sourceImage,string artwork,string qualitySprite,float r,float g,float b,int powerPoints,string typeComponent,string[] effectComponents,string zones,string quality,List<Token> effectCode){
            this.sourceImage=sourceImage;
            this.faction=faction;
            this.cardRealName=cardRealName;
            this.description=description;
            this.effectDescription=effectDescription;
            this.artwork=artwork;
            this.qualitySprite=qualitySprite;
            this.r=r;
            this.g=g;
            this.b=b;
            this.powerPoints=powerPoints;
            this.typeComponent=typeComponent;
            this.effectComponents=effectComponents;
            this.zones=zones;
            this.quality=quality;
            this.effectCode=effectCode;
        }
        public CardSave(string faction,string cardRealName,string description,string effectDescription,string sourceImage,string artwork,string qualitySprite,float r,float g,float b,int powerPoints,string typeComponent,string[] effectComponents,string zones,string quality){
            this.sourceImage=sourceImage;
            this.faction=faction;
            this.cardRealName=cardRealName;
            this.description=description;
            this.effectDescription=effectDescription;
            this.artwork=artwork;
            this.qualitySprite=qualitySprite;
            this.r=r;
            this.g=g;
            this.b=b;
            this.powerPoints=powerPoints;
            this.typeComponent=typeComponent;
            this.effectComponents=effectComponents;
            this.zones=zones;
            this.quality=quality;
            this.effectCode=null;
        }
    }
    public class Token{//Clase que almacena las propiedades del token
        public string text;//Texto del token
        public int position;//Posicion del token (indice con el que se encuentra en el string codigo)
        public int line;//Linea donde se encuentra en el texto del objeto Compiler
        public int col;//Columna donde se encuentra en el texto del objeto Compiler
        //Tipos de token
        public enum tokenTypes{number,identifier,cardAssignment,blockDeclaration,literal,punctuator,binOperator,unexpected};
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
}
