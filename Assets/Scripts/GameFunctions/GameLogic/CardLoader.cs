using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
//Script para instanciar cartas de un json
public class CardLoader : MonoBehaviour
{
    public GameObject CardPrefab;//Referencia al prefab CardPrefab
    private static Sprite[] randomImages => Resources.LoadAll<Sprite>("RandomImages");
    private static bool failedAtInterpretingAnyCard = false;
    private static int instantiatedCardsCount;//Cuenta de las cartas instanciadas
    private Dictionary<string, string> cardTypes = new Dictionary<string, string>()
    {
        {"Oro","GoldCard"},
        {"Plata","SilverCard"},
        {"Clima","WeatherCard"},
        {"Despeje","ClearWeatherCard"},
        {"Aumento","BoostCard"},
        {"Senuelo","BaitCard"},
        {"Lider","LeaderCard"}
    };
    private static IEnumerable<string> allLoadedEffects;
    public void LoadCards(IEnumerable<string> loadedEffects)
    {
        allLoadedEffects = loadedEffects;
        instantiatedCardsCount = 0;
        ImportDeckTo(PlayerPrefs.GetString("P1PrefDeck"), GameObject.Find("CardsP1"), GameObject.Find("DeckP1"));
        ImportDeckTo(PlayerPrefs.GetString("P2PrefDeck"), GameObject.Find("CardsP2"), GameObject.Find("DeckP2"));
        Executer.ErrorScreen.SetActive(Executer.FailedAtLoadingAnyEffect || failedAtInterpretingAnyCard);
        allLoadedEffects = null;
    }
    public void ImportDeckTo(string faction, GameObject deckPlace, GameObject Deck)
    {//Crea todas las cartas en el directorio asignado en preferencias del jugador en el objeto
        string factionPath = Application.persistentDataPath + "/Decks/" + faction;
        string[] addressesOfCards = Directory.GetFiles(factionPath, "*.txt");//Obtiene dentro del directorio del deck solo la direccion de los archivos con extension txt (ignora los meta)

        foreach (string address in addressesOfCards)
        {//Para cada uno de los archivos con extension .txt
            string codeCard = File.ReadAllText(address);//Lee el archivo
            CardDeclaration cardDeclaration = Parser.ProcessCardCode(codeCard);//Convierte el string en json a un objeto CardSave
            if (cardDeclaration != null) { for (int i = cardDeclaration.TotalCopies.Evaluate(); i > 0; i--) { ImportCardTo(cardDeclaration, deckPlace); } }
            else { Errors.Write("No se pudo procesar el texto de la carta en: " + address); failedAtInterpretingAnyCard = true; }
        }
        //Asignando la imagen del deck
        Deck.GetComponent<UnityEngine.UI.Image>().sprite = LoadSprite(Application.persistentDataPath + "/Images/" + faction + "/DeckImage.png");
    }
    public void ImportCardTo(CardDeclaration cardDeclaration, GameObject deckPlace)
    {
        GameObject newCard;
        Player player = deckPlace.Field();
        string cardType = cardTypes[cardDeclaration.Type.Evaluate()];
        //Instanciando la carta
        newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //Si la carta es lider se envia a la zona de lideres, si no se envia a el contenedor de cartas
        if (cardType == "LeaderCard") { newCard.transform.SetParent(GameObject.Find("LeaderZone" + player).transform); }
        else { newCard.transform.SetParent(deckPlace.transform); }
        instantiatedCardsCount++;
        newCard.name = cardDeclaration.Name + "(" + instantiatedCardsCount + ")";//Se le cambia el nombre a uno que sera unico: el nombre de la carta junto con la cantidad de cartas instanciadas

        //Anadiendo scripts
        newCard.AddComponent(Type.GetType(cardType));
        newCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);//Resetea la escala porque cuando se instancia esta desproporcional al resto de objetos

        //Card Properties
        newCard.GetComponent<Card>().Faction = cardDeclaration.Faction.Evaluate();
        newCard.GetComponent<Card>().CardName = cardDeclaration.Name.Evaluate();
        newCard.GetComponent<Card>().Owner = player;
        newCard.GetComponent<Card>().Description = cardDeclaration.Description.Evaluate();

        //Sprite
        newCard.GetComponent<UnityEngine.UI.Image>().sprite = LoadSprite(Application.persistentDataPath + "/Images/" + cardDeclaration.Faction + "/" + cardDeclaration.Name + ".png");//Carga el sprite
        if (newCard.GetComponent<UnityEngine.UI.Image>().sprite == null) { newCard.GetComponent<UnityEngine.UI.Image>().sprite = randomImages.RandomElement(); }//Si no se encontro una imagen se selecciona una random
        //Power || Damage || Boost
        if (newCard.GetComponent<PowerCard>() != null) { newCard.GetComponent<PowerCard>().Power = cardDeclaration.Power.Evaluate(); }
        else if (newCard.GetComponent<WeatherCard>() != null) { newCard.GetComponent<WeatherCard>().Damage = cardDeclaration.Power.Evaluate(); }
        else if (newCard.GetComponent<BoostCard>() != null) { newCard.GetComponent<BoostCard>().Boost = cardDeclaration.Power.Evaluate(); }
        //Range
        if (newCard.GetComponent<UnitCard>() != null) { newCard.GetComponent<UnitCard>().Range = cardDeclaration.Range; }
        //OnActivation
        newCard.GetComponent<Card>().OnActivation = cardDeclaration.OnActivation;
        if (newCard.GetComponent<Card>().OnActivation != null) { AddScriptEffectsAndCheckIfDefined(newCard, newCard.GetComponent<Card>().OnActivation); }
    }
    private void AddScriptEffectsAndCheckIfDefined(GameObject cardOwner, OnActivation onActivation)
    {
        foreach (EffectCall effectCall in onActivation.effectCalls)
        {
            if (effectCall is ScriptEffectCall) { cardOwner.AddComponent(Type.GetType(effectCall.EffectName.Evaluate())); }
            else if (effectCall is CreatedEffectCall)
            {
                if (!allLoadedEffects.Contains(effectCall.EffectName.Evaluate()))
                {
                    Errors.Write("El efecto mencionado por la carta: " + cardOwner.GetComponent<Card>().CardName + ", llamado: '" + effectCall.EffectName + "' no fue cargado o no existe");
                    failedAtInterpretingAnyCard = true;
                }
            }
        }
    }
    private Sprite LoadSprite(string filePath)
    {
        if (!File.Exists(filePath)) { Debug.Log("No se encontro el archivo: " + filePath); return null; }
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(File.ReadAllBytes(filePath));
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}