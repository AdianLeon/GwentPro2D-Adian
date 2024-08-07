using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
//Script para crear las cartas de la faccion seleccionada en el menu Deck
public class DeckMenuLoadCards : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject viewCardPrefab;
    public void DeckStart()
    {//Esta funcion se llama cada vez que se activa el menu deck
        DeckDropdowns.LoadFilesInDropdown(dropdown);//Carga los decks al dropdown
        dropdown.SetValueWithoutNotify(0);//Elige siempre la opcion 0 y la carga en el dropdown
        OnDropdownValueChanged();//Se cargan todas las cartas de la opcion 0
        dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(); });//Ahora cuando el dropdown se modifique la funcion OnDropDownValueChanged se llama
    }
    public void OnDropdownValueChanged()
    {//Cuando el valor del dropdown se modifique
        LoadAllCardsToShow(dropdown.options[dropdown.value].text);//Se cargan todas las cartas de esa carpeta en el medio del menu
    }
    private static void LoadAllCardsToShow(string faction)
    {//Carga todas las cartas de esa faccion
        int count = GameObject.Find("CardsToShow").transform.childCount;
        for (int i = 0; i < count; i++)
        {//Limpia el objeto de cartas anteriores
            Transform previousCard = GameObject.Find("CardsToShow").transform.GetChild(0);
            previousCard.SetParent(GameObject.Find("BG").transform);
            Destroy(previousCard.gameObject);
        }
        string factionPath = Application.dataPath + "/MyAssets/Database/Decks/" + faction;
        string[] cardsJsonAddress = Directory.GetFiles(factionPath, "*.json");//Obtiene dentro del directorio del deck solo la direccion de los archivos con extension json (ignora los meta)

        foreach (string cardJsonAddress in cardsJsonAddress)
        {//Para cada uno de los archivos con extension json
            string jsonFormatCard = File.ReadAllText(cardJsonAddress);//Lee el archivo
            CardSave cardSave = JsonUtility.FromJson<CardSave>(jsonFormatCard);//Convierte el string en json a un objeto CardSave
            LoadCardToShow(cardSave);//Convierte ese objeto a carta
        }

        AlterCardsToShowChildrens();//Se ajusta el tamano de cada carta en dependencia de la cantidad
    }
    private static void LoadCardToShow(CardSave cardSave)
    {
        GameObject newCard = Instantiate(GameObject.Find("Dropdown").GetComponent<DeckMenuLoadCards>().viewCardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newCard.transform.SetParent(GameObject.Find("CardsToShow").transform);//Instanciamos una carta del prefab CardsToShow
        newCard.name = cardSave.cardName + "(ViewCard)";//Le ponemos nombre
        newCard.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);//Resetea la escala porque cuando se instancia esta desproporcional al resto de objetos

        //Propiedades
        newCard.GetComponent<DeckView>().faction = cardSave.faction;//Faction
        newCard.GetComponent<DeckView>().cardName = cardSave.cardName;//Name
        newCard.GetComponent<DeckView>().typeComponent = cardSave.scriptComponent;
        //Sprites
        newCard.GetComponent<Image>().sprite = Resources.Load<Sprite>(cardSave.faction + "/" + cardSave.cardName);//Carga el sprite en Assets/Resources/sourceImage en la carta
        newCard.GetComponent<DeckView>().artwork = Resources.Load<Sprite>(cardSave.faction + "/" + cardSave.cardName + "Image");//Carga el sprite en Assets/Resources/artwork en la carta

        if (newCard.GetComponent<Image>().sprite == null)
        {
            string randomImagesPath = Application.dataPath + "/Resources/RandomImages";
            int max = Directory.GetFiles(randomImagesPath, "*.png").Length;
            newCard.GetComponent<Image>().sprite = Resources.Load<Sprite>("RandomImages/" + Random.Range(1, max + 1).ToString());
        }
        if (newCard.GetComponent<DeckView>().artwork == null)
        {
            newCard.GetComponent<DeckView>().artwork = newCard.GetComponent<Image>().sprite;
        }
        //power || damage || boost
        newCard.GetComponent<DeckView>().power = cardSave.powerPoints;

        //zones
        switch (cardSave.scriptComponent)
        {
            case "LeaderCard":
                newCard.GetComponent<DeckView>().playableZone = "L";
                break;
            case "BoostCard":
                newCard.GetComponent<DeckView>().playableZone = "A";
                break;
            case "WeatherCard":
                newCard.GetComponent<DeckView>().playableZone = "C";
                break;
            case "ClearWeatherCard":
                newCard.GetComponent<DeckView>().playableZone = "D";
                break;
            case "SilverCard":
            case "GoldCard":
                newCard.GetComponent<DeckView>().playableZone = cardSave.zones;
                break;
            case "BaitCard":
                newCard.GetComponent<DeckView>().playableZone = "S";
                break;
        }
    }
    private static void AlterCardsToShowChildrens()
    {
        GridLayoutGroup grid = GameObject.Find("CardsToShow").GetComponent<GridLayoutGroup>();
        if (GameObject.Find("CardsToShow").transform.childCount < 13)
        {
            grid.cellSize = new Vector2(60, 90);
            grid.spacing = new Vector2(80, 130);
        }
        else if (GameObject.Find("CardsToShow").transform.childCount < 17)
        {
            grid.cellSize = new Vector2(50, 70);
            grid.spacing = new Vector2(65, 100);
        }
        else if (GameObject.Find("CardsToShow").transform.childCount < 24)
        {
            grid.cellSize = new Vector2(40, 60);
            grid.spacing = new Vector2(65, 85);
        }
        else if (GameObject.Find("CardsToShow").transform.childCount < 30)
        {
            grid.cellSize = new Vector2(35, 50);
            grid.spacing = new Vector2(55, 80);
        }
        else if (GameObject.Find("CardsToShow").transform.childCount < 40)
        {
            grid.cellSize = new Vector2(30, 40);
            grid.spacing = new Vector2(55, 60);
        }
        else if (GameObject.Find("CardsToShow").transform.childCount < 60)
        {
            grid.cellSize = new Vector2(22, 35);
            grid.spacing = new Vector2(35, 60);
        }
        else
        {
            grid.cellSize = new Vector2(20, 30);
            grid.spacing = new Vector2(30, 40);
        }
    }
}
