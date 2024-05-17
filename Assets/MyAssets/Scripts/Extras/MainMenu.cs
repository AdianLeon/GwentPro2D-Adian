using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Script para la funcionalidad de los botones en el menu principal
public class MainMenu : MonoBehaviour
{
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
