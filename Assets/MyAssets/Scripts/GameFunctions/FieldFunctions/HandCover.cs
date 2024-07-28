//Script para activar el cover de las manos cuando sea turno del enemigo
using System.Collections.Generic;
using UnityEngine;

public class HandCover : MonoBehaviour, IStateSubscriber
{
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (new List<State>{State.SettingUpGame,State.EndingTurn,State.EndingRound}, new Execution(stateInfo => UpdateCover(), 1)),
        new (State.EndingGame , new Execution(stateInfo => gameObject.SetActive(true), 0))
    };
    private void UpdateCover() => gameObject.SetActive(gameObject.Field() == Judge.GetEnemy);
}
