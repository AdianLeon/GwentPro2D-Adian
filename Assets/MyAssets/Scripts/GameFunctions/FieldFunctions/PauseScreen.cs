//Script para marcar las pantallas de pausa
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour, IStateSubscriber
{
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (State.SettingUpGame, new Execution (stateInfo=>gameObject.SetActive(false), 0)),
        new (State.EndingGame, new Execution (stateInfo=>gameObject.SetActive(true), 0))
    };
}
