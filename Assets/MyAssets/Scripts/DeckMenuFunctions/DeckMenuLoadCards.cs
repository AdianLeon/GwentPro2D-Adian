using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
//Script para crear las cartas de la faccion seleccionada en el menu Deck
public class DeckMenuLoadCards : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject viewCardPrefab;
    public void DeckStart(){//Esta funcion se llama cada vez que se activa el menu deck
        LoadFilesInDropdown();//Carga los decks al dropdown
        dropdown.onValueChanged.AddListener(delegate{OnDropdownValueChanged();});//Ahora cuando el dropdown se modifique la funcion OnDeopDownValueChanged se llama
    }
    private void LoadFilesInDropdown(){//Obtiene todos los directorios en la carpeta Decks y los anade como opcion en el dropdown
        DirectoryInfo dir=new DirectoryInfo(Application.dataPath+"/MyAssets/Database/Decks/");
        DirectoryInfo[] subDirs=dir.GetDirectories();//Carpetas dentro de Decks
        dropdown.ClearOptions();//Quita todas las opciones del dropdown

        for(int i=0;i<subDirs.Length;i++){//Anade todas las carpetas (decks)
            dropdown.options.Add(new TMP_Dropdown.OptionData(subDirs[i].Name));
        }
    }
    public void OnDropdownValueChanged(){//Cuando el valor del dropdown se modifique
        string selectedDeck=dropdown.options[dropdown.value].text;//Se obtiene el deck seleccionado
        LoadAllCardsToShow(selectedDeck);//Se cargan todas las cartas de esa carpeta en el medio del menu
    }
    private static void LoadAllCardsToShow(string faction){//Carga todas las cartas de esa faccion
        int count=GameObject.Find("CardsToShow").transform.childCount;
        for(int i=0;i<count;i++){//Limpia el objeto de cartas anteriores
            Transform previousCard=GameObject.Find("CardsToShow").transform.GetChild(0);
            previousCard.SetParent(GameObject.Find("BG").transform);
            Destroy(previousCard.gameObject);
        }
        string factionPath=Application.dataPath+"/MyAssets/Database/Decks/"+faction;
        string[] cardsJsonAddress=Directory.GetFiles(factionPath,"*.json");//Obtiene dentro del directorio del deck solo la direccion de los archivos con extension json (ignora los meta)
        
        for(int i=0;i<cardsJsonAddress.Length;i++){//Para cada uno de los archivos con extension json
            string jsonFormatCard=File.ReadAllText(cardsJsonAddress[i]);//Lee el archivo
            CustomClasses.CardSave cardSave=JsonUtility.FromJson<CustomClasses.CardSave>(jsonFormatCard);//Convierte el string en json a un objeto CardSave
            LoadCardToShow(cardSave);//Convierte ese objeto a carta
        }

        AlterCardsToShowChildrens();//Se ajusta el tamano de cada carta en dependencia de la cantidad
    }
    private static void LoadCardToShow(CustomClasses.CardSave cardSave){
        GameObject newCard=null;
        newCard=Instantiate(GameObject.Find("Dropdown").GetComponent<DeckMenuLoadCards>().viewCardPrefab,new Vector3(0,0,0),Quaternion.identity);
        newCard.transform.SetParent(GameObject.Find("CardsToShow").transform);//Instanciamos una carta del prefab CardsToShow
        newCard.name=cardSave.cardRealName+"(ViewCard)";//Le ponemos nombre
        newCard.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);//Resetea la escala porque cuando se instancia esta desproporcional al resto de objetos
        
        //Card Properties
        newCard.GetComponent<DeckView>().faction=cardSave.faction;//Faction
        newCard.GetComponent<DeckView>().cardRealName=cardSave.cardRealName;//Name
        newCard.GetComponent<DeckView>().description=cardSave.description;//Description
        newCard.GetComponent<DeckView>().effectDescription=cardSave.effectDescription;//EffectDescription
        newCard.GetComponent<DeckView>().cardColor=new Color(cardSave.r,cardSave.g,cardSave.b,1);//Color

        //Sprites
        newCard.GetComponent<Image>().sprite=Resources.Load<Sprite>(cardSave.sourceImage);//Carga el sprite en Assests/Resources/sourceImage en la carta
        newCard.GetComponent<DeckView>().artwork=Resources.Load<Sprite>(cardSave.artwork);//Carga el sprite en Assests/Resources/artwork en la carta
        newCard.GetComponent<DeckView>().qualitySprite=Resources.Load<Sprite>(cardSave.qualitySprite);//Carga el sprite en Assests/Resources/qualitySprite en la carta
        
        //power || damage || boost
        newCard.GetComponent<DeckView>().power=cardSave.powerPoints;

        //zones && quality
        if(cardSave.typeComponent=="UnitCard"){
            newCard.GetComponent<DeckView>().playableZone=cardSave.zones;
        }else if(cardSave.typeComponent=="WeatherCard"){
            newCard.GetComponent<DeckView>().playableZone="C";
        }else if(cardSave.typeComponent=="ClearWeatherCard"){
            newCard.GetComponent<DeckView>().playableZone="D";
        }else if(cardSave.typeComponent=="BaitCard"){
            newCard.GetComponent<DeckView>().playableZone="S";
        }else if(cardSave.typeComponent=="BoostCard"){
            newCard.GetComponent<DeckView>().playableZone="A";
        }else if(cardSave.typeComponent=="LeaderCard"){
            newCard.GetComponent<DeckView>().playableZone="L";
        }
    }
    private static void AlterCardsToShowChildrens(){
        GridLayoutGroup grid=GameObject.Find("CardsToShow").GetComponent<GridLayoutGroup>();
        if(GameObject.Find("CardsToShow").transform.childCount<13){
            grid.cellSize=new Vector2(60,90);
            grid.spacing=new Vector2(80,130);
        }else if(GameObject.Find("CardsToShow").transform.childCount<17){
            grid.cellSize=new Vector2(50,70);
            grid.spacing=new Vector2(65,100);
        }else if(GameObject.Find("CardsToShow").transform.childCount<24){
            grid.cellSize=new Vector2(40,60);
            grid.spacing=new Vector2(65,85);
        }else if(GameObject.Find("CardsToShow").transform.childCount<30){
            grid.cellSize=new Vector2(35,50);
            grid.spacing=new Vector2(55,80);
        }else if(GameObject.Find("CardsToShow").transform.childCount<40){
            grid.cellSize=new Vector2(30,40);
            grid.spacing=new Vector2(55,60);
        }else if(GameObject.Find("CardsToShow").transform.childCount<60){
            grid.cellSize=new Vector2(22,35);
            grid.spacing=new Vector2(35,60);
        }else{
            grid.cellSize=new Vector2(20,30);
            grid.spacing=new Vector2(30,40);
        }
    }
}
