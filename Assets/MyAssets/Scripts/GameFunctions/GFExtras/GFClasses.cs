using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script que declara las clases a utilizar de todo el juego
public abstract class CustomBehaviour: MonoBehaviour{//Comportamiento custom para determinados scripts
    public abstract void Initialize();//Inicializa sus variables y realiza acciones al principio del juego
    public abstract void Finish();//Termina el juego y realiza las ultimas acciones
    public abstract void NextUpdate();//Actualiza algo con los valores actuales
}
[System.Serializable]
public class PlayerPrefsData{//Clase para guardar las preferencias del jugador
        public float volume;
        public string deckPrefP1;
        public string deckPrefP2;
        public PlayerPrefsData(float volume,string deckPrefP1,string deckPrefP2){
            this.volume=volume;
            this.deckPrefP1=deckPrefP1;
            this.deckPrefP2=deckPrefP2;
        }
}