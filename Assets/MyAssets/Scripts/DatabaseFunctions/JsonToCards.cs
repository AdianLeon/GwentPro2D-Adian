using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.UIElements;
//Script para instanciar cartas de un json
public class JsonToCards : MonoBehaviour
{
    private static int instantiatedCardsCount;//Cuenta de las cartas instanciadas
    public GameObject cardPrefab;//Referencia al prefab CardPrefab 
    public GameObject leaderPrefab;//Referencia al prefab LeaderPrefab
    void Awake(){
        instantiatedCardsCount=0;
        ImportDeckTo(PlayerPrefs.GetString("P1PrefDeck"),GameObject.Find("CardsP1"),GameObject.Find("DeckP1"));
        ImportDeckTo(PlayerPrefs.GetString("P2PrefDeck"),GameObject.Find("CardsP2"),GameObject.Find("DeckP2"));
    }
    public static void ImportDeckTo(string faction,GameObject deckPlace,GameObject Deck){//Crea todas las cartas en el directorio asignado en preferencias del jugador en el objeto
        string factionPath=Application.dataPath+"/MyAssets/Database/Decks/"+faction;
        string[] cardsJsonAddress=Directory.GetFiles(factionPath,"*.json");//Obtiene dentro del directorio del deck solo la direccion de los archivos con extension json (ignora los meta)
        for(int i=0;i<cardsJsonAddress.Length;i++){//Para cada uno de los archivos con extension json
            string jsonFormatCard=File.ReadAllText(cardsJsonAddress[i]);//Lee el archivo
            CardSave cardSave=JsonUtility.FromJson<CardSave>(jsonFormatCard);//Convierte el string en json a un objeto CardSave
            ImportCardTo(cardSave,deckPlace);
        }
        //Asignando la imagen del deck
        Deck.GetComponent<UnityEngine.UI.Image>().sprite=Resources.Load<Sprite>(faction+"/DeckImage");
    }
    public static void ImportCardTo(CardSave cardSave,GameObject deckPlace){
        GameObject newCard;
        //Instanciando la carta
        if(cardSave.typeComponent=="LeaderCard"){//Si la carta a crear es una carta lider
            newCard=Instantiate(GameObject.Find("Canvas").GetComponent<JsonToCards>().leaderPrefab,new Vector3(0,0,0),Quaternion.identity);//Instanciando una carta lider generica
            string player=deckPlace.name[deckPlace.name.Length-2].ToString()+deckPlace.name[deckPlace.name.Length-1].ToString();
            newCard.GetComponent<LeaderCard>().WhichField=(fields)Enum.Parse(typeof(fields),player);
            newCard.transform.SetParent(GameObject.Find("LeaderZone"+player).transform);
        }else{//Si la carta no es lider
            newCard=Instantiate(GameObject.Find("Canvas").GetComponent<JsonToCards>().cardPrefab,new Vector3(0,0,0),Quaternion.identity);//Instanciando una carta generica
            newCard.transform.SetParent(deckPlace.transform);//Seteando esa carta generica a donde pertenece dependiendo del campo
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
        newCard.GetComponent<Card>().OnActivationCode=cardSave.onActivationCodeName;

        //Sprites
        newCard.GetComponent<UnityEngine.UI.Image>().sprite=Resources.Load<Sprite>(cardSave.faction+"/"+cardSave.cardRealName);//Carga el sprite en Assets/Resources/sourceImage en la carta
        newCard.GetComponent<Card>().artwork=Resources.Load<Sprite>(cardSave.faction+"/"+cardSave.cardRealName+"Image");//Carga el sprite en Assets/Resources/artwork en la carta

        if(newCard.GetComponent<UnityEngine.UI.Image>().sprite is null){
            string randomImagesPath=Application.dataPath+"/Resources/RandomImages";
            int max=Directory.GetFiles(randomImagesPath,"*.png").Length;
            newCard.GetComponent<UnityEngine.UI.Image>().sprite=Resources.Load<Sprite>("RandomImages/"+UnityEngine.Random.Range(1,max+1).ToString());
        }
        if(newCard.GetComponent<Card>().artwork is null){
            newCard.GetComponent<Card>().artwork=Resources.Load<Sprite>("BlankImage");
        }

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
        }
    }
}