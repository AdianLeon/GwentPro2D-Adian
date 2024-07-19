using UnityEngine;
using TMPro;
//Script para el audio
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;//Declara un campo en el objeto que lo contiene
    public AudioClip backgroundMusic;//Clip de audio
    void Start()
    {
        gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("allVolume");//Se accede al volumen preferido del jugador y se actualiza
        musicSource.clip = backgroundMusic;//Se asigna el clip de audio al campo musicSource
        musicSource.Play();//Se llama a la funcion Play (Se pone la musica)
    }
    public void SetVolume(float volume)
    {//Cambia el volumen, esta funcion solo es llamada por el slider creado en el menu inicial
        PlayerPrefs.SetFloat("allVolume", volume / 100);//Se establece este volumen como preferencia del jugador
        gameObject.GetComponent<AudioSource>().volume = volume / 100;//Se actualiza el volumen
        GameObject.Find("Percentage").GetComponent<TextMeshProUGUI>().text = volume + "%";//Se muestra el porcentaje del volumen total
    }
}
