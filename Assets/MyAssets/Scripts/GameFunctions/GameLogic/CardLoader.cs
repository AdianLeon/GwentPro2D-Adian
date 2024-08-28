using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
//Script para instanciar cartas de un json
public class CardLoader : MonoBehaviour, IStateSubscriber
{
    public GameObject errorScreen;
    private static int instantiatedCardsCount;//Cuenta de las cartas instanciadas
    public GameObject CardPrefab;//Referencia al prefab CardPrefab
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (State.Loading, new Execution (stateInfo => LoadCards(), 1))
    };
    private void LoadCards()
    {
        Debug.Log(1);
        instantiatedCardsCount = 0;
        ImportDeckTo(PlayerPrefs.GetString("P1PrefDeck"), GameObject.Find("CardsP1"), GameObject.Find("DeckP1"));
        ImportDeckTo(PlayerPrefs.GetString("P2PrefDeck"), GameObject.Find("CardsP2"), GameObject.Find("DeckP2"));
    }
    public void ImportDeckTo(string faction, GameObject deckPlace, GameObject Deck)
    {//Crea todas las cartas en el directorio asignado en preferencias del jugador en el objeto
        string factionPath = Application.dataPath + "/MyAssets/Database/Decks/" + faction;
        string[] addressesOfCards = Directory.GetFiles(factionPath, "*.txt");//Obtiene dentro del directorio del deck solo la direccion de los archivos con extension txt (ignora los meta)

        bool failedAtInterpretingAnyCard = false;
        foreach (string address in addressesOfCards)
        {//Para cada uno de los archivos con extension json
            string codeCard = File.ReadAllText(address);//Lee el archivo
            CardDeclaration cardDeclaration = CardParser.ProcessCode(codeCard);//Convierte el string en json a un objeto CardSave
            if (cardDeclaration != null) { for (int i = cardDeclaration.TotalCopies; i > 0; i--) { ImportCardTo(cardDeclaration, deckPlace); } }
            else { Errors.Write("No se pudo procesar el texto de la carta en: " + address); failedAtInterpretingAnyCard = true; }
        }
        errorScreen.SetActive(!Executer.LoadedAllEffects || failedAtInterpretingAnyCard);
        //Asignando la imagen del deck
        Deck.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(faction + "/DeckImage");
    }
    public void ImportCardTo(CardDeclaration cardDeclaration, GameObject deckPlace)
    {
        GameObject newCard;
        Player player = deckPlace.Field();
        //Instanciando la carta
        newCard = Instantiate(CardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //Si la carta es lider se envia a la zona de lideres, si no se envia a el contenedor de cartas
        if (cardDeclaration.Type == "LeaderCard") { newCard.transform.SetParent(GameObject.Find("LeaderZone" + player).transform); }
        else { newCard.transform.SetParent(deckPlace.transform); }
        instantiatedCardsCount++;
        newCard.name = cardDeclaration.Name + "(" + instantiatedCardsCount + ")";//Se le cambia el nombre a uno que sera unico: el nombre de la carta junto con la cantidad de cartas instanciadas

        //Anadiendo scripts
        newCard.AddComponent(Type.GetType(cardDeclaration.Type));
        newCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);//Resetea la escala porque cuando se instancia esta desproporcional al resto de objetos

        //Card Properties
        newCard.GetComponent<Card>().Faction = cardDeclaration.Faction;
        newCard.GetComponent<Card>().CardName = cardDeclaration.Name;
        newCard.GetComponent<Card>().Owner = player;
        newCard.GetComponent<Card>().Description = cardDeclaration.Description;

        //Sprites
        newCard.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(cardDeclaration.Faction + "/" + cardDeclaration.Name);//Carga el sprite en Assets/Resources/sourceImage en la carta
        newCard.GetComponent<Card>().Artwork = Resources.Load<Sprite>(cardDeclaration.Faction + "/" + cardDeclaration.Name + "Image");//Carga el sprite en Assets/Resources/artwork en la carta

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
        //Power || Damage || Boost
        if (newCard.GetComponent<PowerCard>() != null) { newCard.GetComponent<PowerCard>().Power = cardDeclaration.Power; }
        else if (newCard.GetComponent<WeatherCard>() != null) { newCard.GetComponent<WeatherCard>().Damage = cardDeclaration.Power; }
        else if (newCard.GetComponent<BoostCard>() != null) { newCard.GetComponent<BoostCard>().Boost = cardDeclaration.Power; }
        //Range
        if (newCard.GetComponent<UnitCard>() != null) { newCard.GetComponent<UnitCard>().Range = cardDeclaration.Range; }
        //OnActivation
        newCard.GetComponent<Card>().OnActivation = cardDeclaration.OnActivation;
        if (newCard.GetComponent<Card>().OnActivation != null) { AddScriptEffects(newCard, newCard.GetComponent<Card>().OnActivation); }
    }
    private void AddScriptEffects(GameObject cardOwner, OnActivation onActivation) { foreach (EffectCall effectCall in onActivation.effectCalls) { if (effectCall is ScriptEffectCall) { cardOwner.AddComponent(Type.GetType(effectCall.EffectName)); } } }
}