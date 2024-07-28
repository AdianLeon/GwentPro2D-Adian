using System;
using UnityEngine;
using System.Collections.Generic;
//Script centro del manejo de estados y poseedor del unico Update()
public class StateManager : MonoBehaviour
{
    private static Dictionary<State, List<Execution>> assigner;//Diccionario que asigna estados a una lista de ejecuciones ordenada
    void Start()
    {////Inicializa el diccionario y busca todos las subscripciones de los IStateSubscriber de estados y las anade al diccionario
        assigner = new Dictionary<State, List<Execution>>();
        foreach (State state in Enum.GetValues(typeof(State))) { assigner.Add(state, new List<Execution>()); }
        GFUtils.FindGameObjectsOfType<IStateSubscriber>().ForEach(stateSubscriber => AddToAssigner(stateSubscriber));
        Publish(State.LoadingCards);
        ResetGame();
    }
    private static void AddToAssigner(IStateSubscriber stateSubscriber)
    {//Recibe un suscriptor y anade sus subscripciones a el diccionario asignador insertandolo segun su prioridad
        foreach (StateSubscription stateSubscription in stateSubscriber.GetStateSubscriptions)
        {//Por cada subscripcion
            foreach (State key in stateSubscription.Keys)
            {//Se itera por sus estados (que son las llaves del diccionario)
                InsertByPriority(stateSubscription, key);
            }
        }
    }
    private static void InsertByPriority(StateSubscription stateSubscription, State key)
    {//Inserta el Execution de stateSubscription en la lista del diccionario correspondiente al estado segun su prioridad
        for (int i = 0; i < assigner[key].Count; i++)
        {//Itera por cada uno de los execution de la lista correspondiente a ese estado, cuando encuentra uno de menor (thatPriority>myPriority) prioridad se inserta en su lugar
            if (assigner[key][i].Priority > stateSubscription.Value.Priority) { assigner[key].Insert(i, stateSubscription.Value); return; }
        }//Si no se encuentra que exista algun Execution en esa lista de menor prioridad se anade al final
        assigner[key].Add(stateSubscription.Value);
    }
    void Update() => GFUtils.FindGameObjectsOfType<IKeyboardListener>().ForEach(listener => listener.ListenToKeyboardPress());//Constantemente llama a los implementadores de la interfaz
    public static void ResetGame() => Publish(State.SettingUpGame);//Reinicia el juego, este metodo es llamado por un boton que aparece cuando acaba el juego llamado ResetGameButton
    public static void Publish(State state, StateInfo stateInfo = null) => assigner[state].ForEach(execution => execution.TriggerCode(stateInfo));
}
