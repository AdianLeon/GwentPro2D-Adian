using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
//Script para convertir cartas a formato json e instanciar cartas de un json
public class JsonCards : MonoBehaviour
{
    private static int instantiatedCardsCount;
    public GameObject prefab;//Referencia al prefab CardPrefab 
    void Awake(){
        instantiatedCardsCount=0;
        //ExportCardsInObject();//Para hacer jsons de todas las cartas en el objeto CardsToExport
        ImportDeckTo(PlayerPrefs.GetString("P1Deck"),GameObject.Find("CardsP1"));//Crea todas las cartas en el directorio asignado en preferencias del jugador en el objeto
        ImportDeckTo(PlayerPrefs.GetString("P2Deck"),GameObject.Find("CardsP2"));
    }
    public static void ImportDeckTo(string faction,GameObject DeckPlace){
        string factionPath=Application.dataPath+"/MyAssets/DeckCreator/Decks/"+faction;
        string[] cardsJson=Directory.GetFiles(factionPath,"*.json");//Obtiene dentro del directorio del deck solo los archivos con extension json (ignora los meta)
        for(int i=0;i<cardsJson.Length;i++){//Para cada uno de los archivos con extension json
            string jsonFormatCard=File.ReadAllText(cardsJson[i]);//Lee el archivo
            ImportCardTo(jsonFormatCard,DeckPlace);
        }
        //Asignando la imagen del deck
        if(DeckPlace.name=="CardsP1"){
            GameObject.Find("Deck").GetComponent<Image>().sprite=Resources.Load<Sprite>(faction+"/DeckImage/Deck");
        }else if(DeckPlace.name=="CardsP2"){
            GameObject.Find("EnemyDeck").GetComponent<Image>().sprite=Resources.Load<Sprite>(faction+"/DeckImage/Deck");
        }
    }
    public static void ImportCardTo(string jsonFormatCard,GameObject DeckPlace){
        CardSave cardSave=JsonUtility.FromJson<CardSave>(jsonFormatCard);//Convierte el string en json a un objeto CardSave
        GameObject newCard=Instantiate(GameObject.Find("Canvas").GetComponent<JsonCards>().prefab,new Vector3(0,0,0),Quaternion.identity);//Instanciando una carta generica
        newCard.transform.SetParent(DeckPlace.transform);//Seteando esa carta generica a donde pertenece dependiendo del campo
        instantiatedCardsCount++;
        newCard.name=cardSave.cardRealName+"("+instantiatedCardsCount.ToString()+")";//Se le cambia el nombre a uno que sera unico: el nombre de la carta junto con la cantidad de cartas instanciadas
        newCard.AddComponent(Type.GetType(cardSave.typeComponent));//Anade el componente carta
        for(int i=0;i<cardSave.effectComponents.Count;i++){//Anade todos los componentes de efecto
            newCard.AddComponent(Type.GetType(cardSave.effectComponents[i]));
        }
        newCard.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);//Resetea la escala porque cuando se instancia esta desproporcional al resto de objetos
        
        //Card Properties
        newCard.GetComponent<Card>().faction=cardSave.faction;//faction
        newCard.GetComponent<Card>().cardRealName=cardSave.cardRealName;//cardRealName
        newCard.GetComponent<Card>().description=cardSave.description;//description
        newCard.GetComponent<Card>().effectDescription=cardSave.effectDescription;//effectDescription
        newCard.GetComponent<Card>().cardColor=new Color(cardSave.r,cardSave.g,cardSave.b,cardSave.a);//cardColor

        //Sprites
        newCard.GetComponent<Image>().sprite=Resources.Load<Sprite>(cardSave.sourceImage);//Carga el sprite en Assests/Resources/sourceImage en la carta
        newCard.GetComponent<Card>().artwork=Resources.Load<Sprite>(cardSave.artwork);//Carga el sprite en Assests/Resources/artwork en la carta
        newCard.GetComponent<Card>().qualitySprite=Resources.Load<Sprite>(cardSave.qualitySprite);//Carga el sprite en Assests/Resources/qualitySprite en la carta
        
        //power || damage || boost
        if(newCard.GetComponent<CardWithPower>()!=null){//Si la carta instanciada es de poder
            newCard.GetComponent<CardWithPower>().power=cardSave.powerPoints;
        }else if(newCard.GetComponent<WeatherCard>()!=null){//Si es clima
            newCard.GetComponent<WeatherCard>().damage=cardSave.powerPoints;
        }else if(newCard.GetComponent<BoostCard>()!=null){//Si es aumento
            newCard.GetComponent<BoostCard>().boost=cardSave.powerPoints;
        }

        if(newCard.GetComponent<UnitCard>()!=null){//zones y quality
            newCard.GetComponent<UnitCard>().whichZone=(UnitCard.zonesUC)Enum.Parse(typeof(UnitCard.zonesUC),cardSave.zones);//Convierte el string guardado en cardSave a un tipo del enum zones y lo asigna a la carta
            newCard.GetComponent<UnitCard>().whichQuality=(UnitCard.quality)Enum.Parse(typeof(UnitCard.quality),cardSave.quality);//Convierte el string guardado en cardSave a un tipo del enum quality y lo asigna a la carta
        }
    }
    public static void ExportCardsInObject(){//Toma todas las cartas y las exporta en formato json
        GameObject t=GameObject.Find("CardsForExporting");
        for(int i=0;i<t.transform.childCount;i++){
            ExportCard(t.transform.GetChild(i).gameObject);
        }
    }
    public static void ExportCard(GameObject card){
        string filePath=Application.dataPath+"/MyAssets/DeckCreator/Decks/"+card.GetComponent<Card>().faction+"/"+card.name+".json";
        Component[] validScripts=CardScriptsGatherer(card);//Todos los scripts validos de card
        string sourceImage=card.GetComponent<Card>().faction+"/"+card.GetComponent<Image>().sprite.name;
        string artwork=card.GetComponent<Card>().faction+"/"+card.GetComponent<Card>().artwork.name;
        string qualitySprite=card.GetComponent<Card>().faction+"/"+card.GetComponent<Card>().qualitySprite.name;

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

        CardSave saveCard=new CardSave(
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
    public class CardSave{//Clase para guardar todas las propiedades de una carta
        public string sourceImage;//Imagen del objeto
        public string faction;//Faccion de la carta
        public string cardRealName;//Nombre de la carta
        public string description;//Descripcion de la carta
        public string effectDescription;//Descripcion del efecto
        public string artwork;//Imagen para mostrar en el CardView
        public string qualitySprite;//Imagen de la calidad
        public float r;//Datos del color de la carta
        public float g;//Datos del color de la carta
        public float b;//Datos del color de la carta
        public float a;//Datos del color de la carta
        public int powerPoints;//Puntos de la carta sea para el power de las cartas unidades, damage de climas o boost de las cartas aumento
        public string typeComponent;//Nombre del tipo de carta
        public List<string> effectComponents;//Lista de nombres de los componentes efecto
        public string zones;//Zonas donde se puede jugar en caso de que sea tipo unidad
        public string quality;//Calidad de la carta en caso de que sea tipo Unidad
        public CardSave(string sourceImage,string faction,string cardRealName,string description,string effectDescription,string artwork,string qualitySprite,ColorData color,int powerPoints,string typeComponent,List<string> effectComponents,string zones,string quality){
            this.sourceImage=sourceImage;
            this.faction=faction;
            this.cardRealName=cardRealName;
            this.description=description;
            this.effectDescription=effectDescription;
            this.artwork=artwork;
            this.qualitySprite=qualitySprite;
            this.r=color.r;
            this.g=color.g;
            this.b=color.b;
            this.a=color.a;
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