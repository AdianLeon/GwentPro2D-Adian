using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//Script para la funcionalidad de los botones en el menu principal
public class MainMenu : MonoBehaviour
{
    static bool firstExecuted=true;//Controla la primera ejecucion
    void Start(){//Cuando se inicialice la escena
        if(firstExecuted){//Si es la primera vez que este script se ejecuta
            PlayerPrefs.SetFloat("allVolume",1);//Se impone como preferencia el volumen maximo
            PlayerPrefs.SetString("P1Deck","Minions");
            PlayerPrefs.SetString("P2Deck","Minions");
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
        Application.Quit();
    }
}
