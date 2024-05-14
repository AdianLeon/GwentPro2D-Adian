using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//Script para el audio
public class AudioManager : MonoBehaviour
{
    static bool firstExecuted=true;//Controla la primera ejecucion
    [SerializeField] AudioSource musicSource;//Declara un campo en el objeto que lo contiene
    public AudioClip backgroundMusic;//Clip de audio

    void Start(){//Cuando se inicialice la escena
        if(firstExecuted){//Si es la primera vez que este script se ejecuta
            PlayerPrefs.SetFloat("allVolume",1);//Se impone como preferencia el volumen maximo
            firstExecuted=false;//Ya no se ejecutara este condicional de nuevo
        }
        if(SceneManager.GetActiveScene().buildIndex==0){//Si estamos en el menu inicial
            GameObject.Find("SoundSlider").GetComponent<Slider>().value=PlayerPrefs.GetFloat("allVolume")*100;//Inicializa el valor del slider del sonido con el valor preferido del jugador
            GameObject.Find("Percentage").GetComponent<TextMeshProUGUI>().text=PlayerPrefs.GetFloat("allVolume")*100+"%";//Actualiza el porcentaje
        }
        this.gameObject.GetComponent<AudioSource>().volume=PlayerPrefs.GetFloat("allVolume");//Se accede al volumen preferido del jugador y se actualiza
        musicSource.clip=backgroundMusic;//Se asigna el clip de audio al campo musicSource
        musicSource.Play();//Se llama a la funcion Play(Se pone la musica)
    }
    public void SetVolume(float volume){//Cambia el volumen, esta funcion solo es llamada por el slider creado en el menu inicial
        PlayerPrefs.SetFloat("allVolume",volume/100);//Se establece este volumen como preferencia del jugador
        this.gameObject.GetComponent<AudioSource>().volume=volume/100;//Se actualiza el volumen
        GameObject.Find("Percentage").GetComponent<TextMeshProUGUI>().text=volume+"%";//Se muestra el porcentaje del volumen total
    }
}
