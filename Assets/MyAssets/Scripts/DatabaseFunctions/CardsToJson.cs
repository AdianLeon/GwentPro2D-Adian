using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
//Script para convertir cartas a archivos json
public class CardsToJson : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Exporting Cards");
        this.ExportAllCards();//Para hacer jsons de todas las cartas en el objeto CardsToExport
    }
    private void ExportAllCards(){//Toma todas las cartas y las exporta en formato json
        for(int i=0;i<this.transform.childCount;i++){
            ExportCard(this.transform.GetChild(i).gameObject);
        }
    }
    private static void ExportCard(GameObject card){
        //Sprites
        string sourceImage=card.GetComponent<Card>().faction+"/"+card.GetComponent<Image>().sprite.name;
        string artwork=card.GetComponent<Card>().faction+"/"+card.GetComponent<Card>().artwork.name;
        string qualitySprite=card.GetComponent<Card>().faction+"/"+card.GetComponent<Card>().qualitySprite.name;
        //Power
        int powerPoints=0;
        if(card.GetComponent<UnitCard>()!=null){
            powerPoints=card.GetComponent<UnitCard>().power;
        }else if(card.GetComponent<WeatherCard>()!=null){
            powerPoints=card.GetComponent<WeatherCard>().damage;
        }else if(card.GetComponent<BoostCard>()!=null){
            powerPoints=card.GetComponent<BoostCard>().boost;
        }
        //Zones && Quality
        string zones="";
        string quality="";
        if(card.GetComponent<UnitCard>()!=null){
            zones=card.GetComponent<UnitCard>().whichZone.ToString();
            quality=card.GetComponent<UnitCard>().whichQuality.ToString();
        }
        CardSave saveCard=new CardSave(
            card.GetComponent<Card>().faction,card.GetComponent<Card>().cardRealName,//Faccion y nombre
            card.GetComponent<Card>().description, card.GetComponent<Card>().effectDescription,//Descripcion y descripcion de efecto
            sourceImage, artwork, qualitySprite,//Sprites
            card.GetComponent<Card>().cardColor.r, card.GetComponent<Card>().cardColor.g, card.GetComponent<Card>().cardColor.b,//Color
            powerPoints,//Power||Damage||Boost
            GetCardScriptName(card), GetEffectScriptsNames(card),//Nombres de scripts
            zones, quality//Enums zones y quality
        );
        string filePath=Application.dataPath+"/MyAssets/Database/Decks/"+card.GetComponent<Card>().faction+"/"+card.name+".json";
        WriteJsonOfCard(saveCard,filePath);
    }
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
        public string zones;//Zonas donde se puede jugar en caso de que sea tipo unidad
        public string quality;//Calidad de la carta en caso de que sea tipo Unidad
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
        }
    }
    public static void WriteJsonOfCard(CardSave saveCard,string address){//Crea un json de la carta guardada en la direccion
        string jsonStringCard=JsonUtility.ToJson(saveCard,true);
        File.WriteAllText(address,jsonStringCard);
    }
    private static string GetCardScriptName(GameObject card){//Devuelve el nombre del script Card
        return card.GetComponent<Card>().GetType().Name;
    }
    private static string[] GetEffectScriptsNames(GameObject card){//Devuelve una lista de nombres de los efectos de la carta
        Component[] effectScripts=card.GetComponents(typeof(CardEffect));
        string[] effectScriptsNames=new string[effectScripts.Length];
        for(int i=0;i<effectScripts.Length;i++){
            effectScriptsNames[i]=effectScripts[i].GetType().Name;
        }
        return effectScriptsNames;
    }
}