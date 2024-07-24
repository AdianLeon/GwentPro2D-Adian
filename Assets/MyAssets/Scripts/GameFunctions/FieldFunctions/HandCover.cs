//Script para activar el cover de las manos cuando sea turno del enemigo
using UnityEngine;

public class HandCover : MonoBehaviour, IStateListener
{
    public int GetPriority => 1;
    public void CheckState()
    {
        switch (Judge.CurrentState)
        {
            case State.SettingUpGame://Si es el turno de su enemigo, se activa, si es el turno de su jugador, se desactiva
            case State.EndingTurn:
            case State.EndingRound:
                gameObject.SetActive(gameObject.Field() == Judge.GetEnemy);
                break;
            case State.EndingGame://Si es el fin del juego se activa
                gameObject.SetActive(true);
                break;
        }
    }
}
