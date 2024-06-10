using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//Script para la funcionalidad de los botones en el menu principal
public class MainMenu : MonoBehaviour
{
    static bool firstExecuted=true;//Controla la primera ejecucion
    void Awake(){//Cuando se inicialice la escena
        if(firstExecuted){//Si es la primera vez que este script se ejecuta
            string jsonPrefs=File.ReadAllText(Application.dataPath+"/MyAssets/Database/PlayerPreferences/PlayerPrefs.json");//Lee el archivo
            PlayerPrefsData prefs=JsonUtility.FromJson<PlayerPrefsData>(jsonPrefs);//Se convierte a un objeto que contiene todos los PlayerPrefs
            PlayerPrefs.SetFloat("allVolume",prefs.volume);//Se impone como preferencia los valores guardados
            PlayerPrefs.SetString("P1Deck",prefs.deckP1);
            PlayerPrefs.SetString("P2Deck",prefs.deckP2);
            firstExecuted=false;//Ya no se ejecutara este condicional de nuevo
        }
        if(SceneManager.GetActiveScene().buildIndex==0){//Si estamos en el menu inicial
            GameObject.Find("SoundSlider").GetComponent<Slider>().value=PlayerPrefs.GetFloat("allVolume")*100;//Inicializa el valor del slider del sonido con el valor preferido del jugador
            GameObject.Find("Percentage").GetComponent<TextMeshProUGUI>().text=PlayerPrefs.GetFloat("allVolume")*100+"%";//Actualiza el porcentaje
        }
    }
    public void Play(){//Cambia la escena a la siguiente (Accede a la escena Game)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void Menu(){//Cambia la escena a la anterior (Regresa al Menu)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
    public void Quit(){//Para salir del juego
        SavePlayerPreferences();//Se guardan las preferencias del jugador
        Application.Quit();
    }
    public static void SavePlayerPreferences(){//Crea un objeto que contendra las preferencias del jugador y lo exporta en formato json
        PlayerPrefsData savePrefs=new PlayerPrefsData(PlayerPrefs.GetFloat("allVolume"),PlayerPrefs.GetString("P1Deck"),PlayerPrefs.GetString("P2Deck"));
        string jsonPlayerPrefs=JsonUtility.ToJson(savePrefs);
        File.WriteAllText(Application.dataPath+"/MyAssets/Database/PlayerPreferences/PlayerPrefs.json",jsonPlayerPrefs);
    }
    private class PlayerPrefsData{//Clase para guardar las preferencias del jugador
        public float volume;
        public string deckP1;
        public string deckP2;
        public PlayerPrefsData(float volume,string deckP1,string deckP2){
            this.volume=volume;
            this.deckP1=deckP1;
            this.deckP2=deckP2;
        }
    }
}