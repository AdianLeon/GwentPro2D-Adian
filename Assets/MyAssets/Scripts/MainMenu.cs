using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Script para la funcionalidad del menu principal
public class MainMenu : MonoBehaviour
{
    //Cambia la escena a la siguiente (Accede a la escena Game)
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    //Cambia la escena a la anterior (Regresa al Menu)
    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
    //Para salir del juego
    public void Quit()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}
