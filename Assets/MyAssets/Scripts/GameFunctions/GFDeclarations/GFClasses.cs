using System;
using System.Collections.Generic;
//Script que declara las clases a utilizar de todo el juego
[System.Serializable]
public class PlayerPrefsData
{//Clase para guardar las preferencias del jugador
    public float volume;
    public string deckPrefP1;
    public string deckPrefP2;
    public int singlePlayerMode;
    public PlayerPrefsData(float volume, string deckPrefP1, string deckPrefP2, int singlePlayerMode)
    {
        this.volume = volume;
        this.deckPrefP1 = deckPrefP1;
        this.deckPrefP2 = deckPrefP2;
        this.singlePlayerMode = singlePlayerMode;
    }
}
public class StateSubscription
{
    public List<State> States;
    public Execution Execution;
    public StateSubscription(Execution execution)
    {
        States = new List<State> { State.SettingUpGame, State.PlayingCard, State.EndingTurn, State.EndingRound, State.EndingGame };
        Execution = execution;
    }
    public StateSubscription(State state, Execution execution)
    {
        States = new List<State> { state };
        Execution = execution;
    }
    public StateSubscription(List<State> states, Execution execution)
    {
        States = states;
        Execution = execution;
    }

}
public class Execution
{
    public Action<StateInfo> TriggerCode;
    public int Priority;
    public string Owner;

    public Execution(Action<StateInfo> triggerCode, int priority)
    {
        TriggerCode = triggerCode;
        Priority = priority;
    }
}
public class StateInfo
{
    public Card CardPlayed;
    public Player Player;
}