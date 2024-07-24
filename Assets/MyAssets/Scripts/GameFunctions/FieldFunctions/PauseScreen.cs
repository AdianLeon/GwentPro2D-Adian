//Script para marcar las pantallas de pausa
using UnityEngine;

public class PauseScreen : MonoBehaviour, IStateListener
{
    public int GetPriority => 0;
    public void CheckState()
    {
        switch (Judge.CurrentState)
        {
            case State.SettingUpGame://Cuando inicia el juego se desactiva
                gameObject.SetActive(false);
                return;
            case State.EndingGame://Cuando el juego termina se activa
                gameObject.SetActive(true);
                return;
        }
    }
}
