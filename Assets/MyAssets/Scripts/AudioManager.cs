using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    public AudioClip backgroundMusic;

    void Start(){
        musicSource.clip=backgroundMusic;
        musicSource.Play();
    }
}
