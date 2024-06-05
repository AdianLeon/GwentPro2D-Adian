using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
//Script para instanciar cartas de un json
public class JsonToCards : MonoBehaviour
{
    private static int instantiatedCardsCount;//Cuenta de las cartas instanciadas
    public GameObject cardPrefab;//Referencia al prefab CardPrefab 
    public GameObject leaderPrefab;//Referencia al prefab LeaderPrefab
    void Awake(){
        instantiatedCardsCount=0;
        ImportDeckTo(PlayerPrefs.GetString("P1Deck"),GameObject.Find("CardsP1"),GameObject.Find("Deck"));
        ImportDeckTo(PlayerPrefs.GetString("P2Deck"),GameObject.Find("CardsP2"),GameObject.Find("EnemyDeck"));
    }
    public static void ImportDeckTo(string faction,GameObject DeckPlace,GameObject Deck){//Crea todas las cartas en el directorio asignado en preferencias del jugador en el objeto
        string factionPath=Application.dataPath+"/MyAssets/Database/Decks/"+faction;
        string[] cardsJsonAddress=Directory.GetFiles(factionPath,"*.json");//Obtiene dentro del directorio del deck solo la direccion de los archivos con extension json (ignora los meta)
        for(int i=0;i<cardsJsonAddress.Length;i++){//Para cada uno de los archivos con extension json
            string jsonFormatCard=File.ReadAllText(cardsJsonAddress[i]);//Lee el archivo
            CustomClasses.CardSave cardSave=JsonUtility.FromJson<CustomClasses.CardSave>(jsonFormatCard);//Convierte el string en json a un objeto CardSave
            ImportCardTo(cardSave,DeckPlace);
        }
        //Asignando la imagen del deck
        Deck.GetComponent<Image>().sprite=Resources.Load<Sprite>(faction+"/DeckImage/Deck");
    }
    public static void ImportCardTo(CustomClasses.CardSave cardSave,GameObject DeckPlace){
        GameObject newCard=null;
        //Instanciando la carta
        if(cardSave.typeComponent=="LeaderCard"){//Si la carta a crear es una carta lider
            newCard=Instantiate(GameObject.Find("Canvas").GetComponent<JsonToCards>().leaderPrefab,new Vector3(0,0,0),Quaternion.identity);//Instanciando una carta lider generica
            if(DeckPlace.name=="CardsP1"){//A la carta lider se le debe asignar el campo ya que no formara parte del deck, en el caso del resto de cartas esto se hace en el script DrawCards 
                newCard.GetComponent<LeaderCard>().whichField=Card.fields.P1;
                newCard.transform.SetParent(GameObject.Find("LeaderZone").transform);
            }else if(DeckPlace.name=="CardsP2"){
                newCard.GetComponent<LeaderCard>().whichField=Card.fields.P2;
                newCard.transform.SetParent(GameObject.Find("EnemyLeaderZone").transform);
            }
        }else{//Si la carta no es lider
            newCard=Instantiate(GameObject.Find("Canvas").GetComponent<JsonToCards>().cardPrefab,new Vector3(0,0,0),Quaternion.identity);//Instanciando una carta generica
            newCard.transform.SetParent(DeckPlace.transform);//Seteando esa carta generica a donde pertenece dependiendo del campo
            newCard.AddComponent(Type.GetType(cardSave.typeComponent));//Anade el componente carta
        }
        instantiatedCardsCount++;
        newCard.name=cardSave.cardRealName+"("+instantiatedCardsCount.ToString()+")";//Se le cambia el nombre a uno que sera unico: el nombre de la carta junto con la cantidad de cartas instanciadas
        
        //Anadiendo componentes
        for(int i=0;i<cardSave.effectComponents.Length;i++){//Anade todos los componentes de efecto
            newCard.AddComponent(Type.GetType(cardSave.effectComponents[i]));
        }
        newCard.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);//Resetea la escala porque cuando se instancia esta desproporcional al resto de objetos
        
        //Card Properties
        newCard.GetComponent<Card>().faction=cardSave.faction;//Faction
        newCard.GetComponent<Card>().cardRealName=cardSave.cardRealName;//Name
        newCard.GetComponent<Card>().description=cardSave.description;//Description
        newCard.GetComponent<Card>().effectDescription=cardSave.effectDescription;//EffectDescription
        newCard.GetComponent<Card>().cardColor=new Color(cardSave.r,cardSave.g,cardSave.b,1);//Color
        newCard.GetComponent<Card>().effectCode=cardSave.effectCode;

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

        //zones && quality
        if(newCard.GetComponent<UnitCard>()!=null){
            newCard.GetComponent<UnitCard>().whichZone=(UnitCard.zonesUC)Enum.Parse(typeof(UnitCard.zonesUC),cardSave.zones);//Convierte el string guardado en cardSave a un tipo del enum zones y lo asigna a la carta
            newCard.GetComponent<UnitCard>().whichQuality=(UnitCard.quality)Enum.Parse(typeof(UnitCard.quality),cardSave.quality);//Convierte el string guardado en cardSave a un tipo del enum quality y lo asigna a la carta
        }
    }
}