using System;
using UnityEngine;
//Script que declara las clases a utilizar de todo el juego
public abstract class StateListener : MonoBehaviour
{//Para aquellos scripts que deban ejecutar codigo en alguno de los estados definidos
    public abstract int GetPriority { get; }//Para definir un orden de prioridad y ejecutar primero aquellos scripts de mayor prioridad
    public abstract void CheckState();//Realiza acciones en dependencia del estado actual, idealmente contiene uno o mas switchs
}
[System.Serializable]
public class PlayerPrefsData
{//Clase para guardar las preferencias del jugador
    public float volume;
    public string deckPrefP1;
    public string deckPrefP2;
    public PlayerPrefsData(float volume, string deckPrefP1, string deckPrefP2)
    {
        this.volume = volume;
        this.deckPrefP1 = deckPrefP1;
        this.deckPrefP2 = deckPrefP2;
    }
}