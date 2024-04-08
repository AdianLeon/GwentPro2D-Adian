using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el audio
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;//Declara un campo en el objeto que lo contiene
    public AudioClip backgroundMusic;//Clip de audio

    void Start(){//Cuando se inicialice la escena
        musicSource.clip=backgroundMusic;//Se asigna el clip de audio al campo musicSource
        musicSource.Play();//Se llama a la funcion Play(Se pone la musica)
    }
}
