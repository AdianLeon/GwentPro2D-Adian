using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//Script para la funcionalidad de los botones en el menu principal
public class MainMenu : MonoBehaviour
{
    public void OnTogglePlayerMode() => PlayerPrefs.SetInt("SinglePlayerMode", GameObject.Find("PlayerModeToggle").GetComponent<Toggle>().isOn ? 1 : 0);
    static bool firstExecuted = true;//Controla la primera ejecucion
    void Awake()
    {//Cuando se inicialice la escena
        if (firstExecuted)
        {//Si es la primera vez que este script se ejecuta
            CheckIfFirstTimeEverExecuted();
            string jsonPrefs = File.ReadAllText(Application.persistentDataPath + "/PlayerPrefs.json");//Lee el archivo
            PlayerPrefsData prefs = JsonUtility.FromJson<PlayerPrefsData>(jsonPrefs);//Se convierte a un objeto que contiene todos los PlayerPrefs
            PlayerPrefs.SetFloat("AllVolume", prefs.Volume);//Se impone como preferencia los valores guardados
            PlayerPrefs.SetString("P1PrefDeck", prefs.DeckPrefP1);
            PlayerPrefs.SetString("P2PrefDeck", prefs.DeckPrefP2);
            PlayerPrefs.SetInt("SinglePlayerMode", prefs.SinglePlayerMode);
            firstExecuted = false;//Ya no se ejecutara este condicional de nuevo
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {//Si estamos en el menu inicial
            GameObject.Find("SoundSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("AllVolume") * 100;//Inicializa el valor del slider del sonido con el valor preferido del jugador
            GameObject.Find("Percentage").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("AllVolume") * 100 + "%";//Actualiza el porcentaje
            GameObject.Find("PlayerModeToggle").GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("SinglePlayerMode") == 1;
        }
    }
    public void Play()
    {//Cambia la escena a la siguiente (Accede a la escena Game)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Menu()
    {//Cambia la escena a la anterior (Regresa al Menu)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void Quit()
    {//Para salir del juego
        SavePlayerPreferences();//Se guardan las preferencias del jugador
        Application.Quit();
    }
    private static void SavePlayerPreferences()
    {//Crea un objeto que contendra las preferencias del jugador y lo exporta en formato json
        PlayerPrefsData savePrefs = new PlayerPrefsData(PlayerPrefs.GetFloat("AllVolume"), PlayerPrefs.GetString("P1PrefDeck"), PlayerPrefs.GetString("P2PrefDeck"), PlayerPrefs.GetInt("SinglePlayerMode"));
        string jsonPlayerPrefs = JsonUtility.ToJson(savePrefs, true);
        File.WriteAllText(Application.persistentDataPath + "/PlayerPrefs.json", jsonPlayerPrefs);
    }
    private static void CheckIfFirstTimeEverExecuted()
    {
        if (!File.Exists(Application.persistentDataPath + "/Code.txt")) { File.Create(Application.persistentDataPath + "/Code.txt"); }
        if (!File.Exists(Application.persistentDataPath + "/PlayerPrefs.json"))
        {
            ReadAndWrite.CreateDefaultDeck();
            PlayerPrefsData savePrefs = new PlayerPrefsData(1f, "Minions", "Minions", 1);
            string jsonPlayerPrefs = JsonUtility.ToJson(savePrefs, true);
            File.WriteAllText(Application.persistentDataPath + "/PlayerPrefs.json", jsonPlayerPrefs);
        }
    }
}