using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
//Script para convertir cartas a formato json
public class JsonCards : MonoBehaviour
{
    void Awake(){
        //ExportCardsInObject();//Para hacer jsons de todas las cartas en el objeto CardsToExport
    }
    public static void ExportCardsInObject(){//Toma todas las cartas y las exporta en formato json
        GameObject t=GameObject.Find("CardsForExporting");
        for(int i=0;i<t.transform.childCount;i++){
            ExportCard(t.transform.GetChild(i).gameObject);
        }
    }
    public static void ExportCard(GameObject card){
        string filePath=Application.dataPath+"/MyAssets/DeckCreator/Decks/Trial/"+card.name+".json";
        Component[] validScripts=CardScriptsGatherer(card);//Todos los scripts validos de card
        string spritesPath=Application.dataPath+"/MyAssets/Sprites/";
        string sourceImage=spritesPath+card.GetComponent<Image>().sprite.name;
        string artwork=spritesPath+card.GetComponent<Card>().artwork.name;
        string qualitySprite=spritesPath+card.GetComponent<Card>().qualitySprite.name;

        ColorData color=new ColorData(card.GetComponent<Card>().cardColor.r,card.GetComponent<Card>().cardColor.g,card.GetComponent<Card>().cardColor.b,card.GetComponent<Card>().cardColor.a);//Color de la carta
        List<string> effectComponentList=new List<string>();//Todos los efectos de card
        string cardTypeComponent=DivideEffectsAndCardScripts(validScripts,effectComponentList);//Componente carta de card
        int powerPoints=0;
        if(card.GetComponent<UnitCard>()!=null){
            powerPoints=card.GetComponent<UnitCard>().power;
        }else if(card.GetComponent<WeatherCard>()!=null){
            powerPoints=card.GetComponent<WeatherCard>().damage;
        }else if(card.GetComponent<BoostCard>()!=null){
            powerPoints=card.GetComponent<BoostCard>().boost;
        }

        string zones="";
        if(card.GetComponent<UnitCard>()!=null){
            zones=card.GetComponent<UnitCard>().whichZone.ToString();
        }

        string quality="";
        if(card.GetComponent<UnitCard>()!=null){
            quality=card.GetComponent<UnitCard>().whichQuality.ToString();
        }

        CardSaveObject saveCard=new CardSaveObject(
            sourceImage,
            card.GetComponent<Card>().faction,
            card.GetComponent<Card>().cardRealName,
            card.GetComponent<Card>().description,
            card.GetComponent<Card>().effectDescription,
            artwork,
            qualitySprite,
            color,
            powerPoints,
            cardTypeComponent,
            effectComponentList,
            zones,
            quality
        );
        string jsonStringCard=JsonUtility.ToJson(saveCard,true);
        File.WriteAllText(filePath,jsonStringCard);
    }
    public class CardSaveObject{//Clase para guardar todas las propiedades de una carta
        public string sourceImage;//Imagen del objeto
        public string faction;//Faccion de la carta
        public string cardRealName;//Nombre de la carta
        public string description;//Descripcion de la carta
        public string effectDescription;//Descripcion del efecto
        public string artwork;//Imagen para mostrar en el CardView
        public string qualitySprite;//Imagen de la calidad
        public ColorData color;//Datos del color de la carta
        public int powerPoints;//Puntos de la carta sea para el power de las cartas unidades, damage de climas o boost de las cartas aumento
        public string typeComponent;//Nombre del tipo de carta
        public List<string> effectComponents;//Lista de nombres de los componentes efecto
        public string zones;//Zonas donde se puede jugar en caso de que sea tipo unidad
        public string quality;//Calidad de la carta en caso de que sea tipo Unidad
        public CardSaveObject(string sourceImage,string faction,string cardRealName,string description,string effectDescription,string artwork,string qualitySprite,ColorData color,int powerPoints,string typeComponent,List<string> effectComponents,string zones,string quality){
            this.sourceImage=sourceImage;
            this.faction=faction;
            this.cardRealName=cardRealName;
            this.description=description;
            this.effectDescription=effectDescription;
            this.artwork=artwork;
            this.qualitySprite=qualitySprite;
            this.color=color;
            this.powerPoints=powerPoints;
            this.typeComponent=typeComponent;
            this.effectComponents=effectComponents;
            this.zones=zones;
            this.quality=quality;
        }
    }
    public class ColorData{//Clase para guardar los datos de un color
        public float r;
        public float g;
        public float b;
        public float a;
        public ColorData(float red,float green,float blue,float alpha){
            r=red;
            g=green;
            b=blue;
            a=alpha;
        }
    }
    private static Component[] CardScriptsGatherer(GameObject card){//Devuelve un array con todos los componentes carta o efecto de card
        Component[] scriptsInCard=card.GetComponents(typeof(MonoBehaviour));//Crea un array con todos los componentes de card
        List<Component> validScripts=new List<Component>();//Crea una lista de componentes vacia
        for(int i=0;i<scriptsInCard.Length;i++){//Itera por todos los componentes de la carta
            //Si contienen la palabra Card o Effect, exceptuando el componente CardView, los anade a la lista de componentes
            if((scriptsInCard[i].ToString().Contains("Card") || scriptsInCard[i].ToString().Contains("Effect")) && !scriptsInCard[i].ToString().Contains("CardView")){
                validScripts.Add(scriptsInCard[i]);
            }
        }
        scriptsInCard=new Component[validScripts.Count];//Ahora reutilizamos el array creado con todos los componentes haciendolo nuevo y del mismo tamano de la lista
        for(int i=0;i<validScripts.Count;i++){//Copiamos todos los componentes de la lista
            scriptsInCard[i]=validScripts[i];
        }
        return scriptsInCard;//Devolvemos el array con todos los componentes validos
    }
    private static string DivideEffectsAndCardScripts(Component[] validScripts,List<string> effectComponentList){
        //Asigna a effectComponentList todos los nombres de los scripts de efecto en validScripts y devuelve el unico script carta
        string cardComponent="";
        for(int i=0;i<validScripts.Length;i++){//Itera por todos los scripts validos
            if(validScripts[i].ToString().Contains("Card")){//Si el script contiene Card en su nombre
                cardComponent=validScripts[i].GetType().Name;//Asigna el nombre del script a la variable a devolver
            }else if(validScripts[i].ToString().Contains("Effect")){//Si el script contiene Effect en su nombre
                effectComponentList.Add(validScripts[i].GetType().Name);//Anade el nombre de ese script a la lista de nombre de efectos
            }
        }
        return cardComponent;
    }
}