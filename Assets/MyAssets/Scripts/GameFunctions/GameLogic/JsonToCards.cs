using System;
using UnityEngine;
using System.IO;
//Script para instanciar cartas de un json
public class JsonToCards : MonoBehaviour, IStateListener
{
    public int GetPriority => 0;
    private static int instantiatedCardsCount;//Cuenta de las cartas instanciadas
    public GameObject CardPrefab;//Referencia al prefab CardPrefab
    public void CheckState()
    {
        if (Judge.CurrentState == State.LoadingCards)
        {
            instantiatedCardsCount = 0;
            ImportDeckTo(PlayerPrefs.GetString("P1PrefDeck"), GameObject.Find("CardsP1"), GameObject.Find("DeckP1"));
            ImportDeckTo(PlayerPrefs.GetString("P2PrefDeck"), GameObject.Find("CardsP2"), GameObject.Find("DeckP2"));
        }
    }
    public static void ImportDeckTo(string faction, GameObject deckPlace, GameObject Deck)
    {//Crea todas las cartas en el directorio asignado en preferencias del jugador en el objeto
        string factionPath = Application.dataPath + "/MyAssets/Database/Decks/" + faction;
        string[] cardsJsonAddress = Directory.GetFiles(factionPath, "*.json");//Obtiene dentro del directorio del deck solo la direccion de los archivos con extension json (ignora los meta)
        foreach (string address in cardsJsonAddress)
        {//Para cada uno de los archivos con extension json
            string jsonFormatCard = File.ReadAllText(address);//Lee el archivo
            CardSave cardSave = JsonUtility.FromJson<CardSave>(jsonFormatCard);//Convierte el string en json a un objeto CardSave
            ImportCardTo(cardSave, deckPlace);
        }
        //Asignando la imagen del deck
        Deck.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(faction + "/DeckImage");
    }
    public static void ImportCardTo(CardSave cardSave, GameObject deckPlace)
    {
        GameObject newCard;
        Player player = deckPlace.Field();
        //Instanciando la carta
        newCard = Instantiate(GameObject.Find("Canvas").GetComponent<JsonToCards>().CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //Si la carta es lider se envia a la zona de lideres, si no se envia a el contenedor de cartas
        if (cardSave.scriptComponent == "LeaderCard") { newCard.transform.SetParent(GameObject.Find("LeaderZone" + player).transform); }
        else { newCard.transform.SetParent(deckPlace.transform); }
        instantiatedCardsCount++;
        newCard.name = cardSave.cardName + "(" + instantiatedCardsCount + ")";//Se le cambia el nombre a uno que sera unico: el nombre de la carta junto con la cantidad de cartas instanciadas

        //Anadiendo scripts
        newCard.AddComponent(Type.GetType(cardSave.scriptComponent));
        newCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);//Resetea la escala porque cuando se instancia esta desproporcional al resto de objetos

        //Card Properties
        newCard.GetComponent<Card>().Faction = cardSave.faction;
        newCard.GetComponent<Card>().CardName = cardSave.cardName;
        newCard.GetComponent<Card>().WhichPlayer = player;
        newCard.GetComponent<Card>().OnActivationName = cardSave.onActivationName;

        //Sprites
        newCard.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(cardSave.faction + "/" + cardSave.cardName);//Carga el sprite en Assets/Resources/sourceImage en la carta
        newCard.GetComponent<Card>().Artwork = Resources.Load<Sprite>(cardSave.faction + "/" + cardSave.cardName + "Image");//Carga el sprite en Assets/Resources/artwork en la carta

        if (newCard.GetComponent<UnityEngine.UI.Image>().sprite == null)
        {//Si no se encontro una imagen se selecciona una random
            string randomImagesPath = Application.dataPath + "/Resources/RandomImages";
            int max = Directory.GetFiles(randomImagesPath, "*.png").Length;
            newCard.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("RandomImages/" + UnityEngine.Random.Range(1, max + 1).ToString());
        }
        if (newCard.GetComponent<Card>().Artwork == null)
        {//Si no se encontro artwork se selecciona la propia imagen
            newCard.GetComponent<Card>().Artwork = newCard.GetComponent<UnityEngine.UI.Image>().sprite;
        }

        //power || damage || boost
        if (newCard.GetComponent<PowerCard>() != null) { newCard.GetComponent<PowerCard>().Power = cardSave.powerPoints; }
        else if (newCard.GetComponent<WeatherCard>() != null) { newCard.GetComponent<WeatherCard>().Damage = cardSave.powerPoints; }
        else if (newCard.GetComponent<BoostCard>() != null) { newCard.GetComponent<BoostCard>().Boost = cardSave.powerPoints; }

        //zones && quality
        if (newCard.GetComponent<UnitCard>() != null)
        {//Convierte el string guardado en cardSave a un tipo del enum zones y lo asigna a la carta
            newCard.GetComponent<UnitCard>().WhichZone = (UnitCardZone)Enum.Parse(typeof(UnitCardZone), cardSave.zones);
        }
    }
}