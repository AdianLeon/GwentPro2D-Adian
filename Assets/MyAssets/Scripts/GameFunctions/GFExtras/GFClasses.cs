using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script que declara las clases a utilizar de todo el proyecto
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