using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
//Script centro del manejo de estados y poseedor del unico Update()
public class StateManager : MonoBehaviour
{
    void Update() => GFUtils.FindGameObjectsOfType<IKeyboardListener>().ForEach(listener => listener.ListenToKeyboardPress());//Constantemente llama a los implementadores de la interfaz
    private static Dictionary<State, List<Execution>> assigner;//Diccionario que asigna una lista de ejecuciones ordenada a estados
    void Start()
    {//Primero carga los efectos y cartas, luego inicializa el diccionario y busca todos las subscripciones de los IStateSubscriber y las anade al diccionario, entonces comienza el juego
        Executer.LoadEffectsAndCards();
        AddAllIStateSubscribersToAssigner();
        Publish(State.SettingUpGame);
    }
    private void AddAllIStateSubscribersToAssigner()
    {
        assigner = new Dictionary<State, List<Execution>>();
        foreach (State state in Enum.GetValues(typeof(State))) { assigner.Add(state, new List<Execution>()); }
        FindAllIStateSubscribers().ForEach(stateSubscriber => AddToAssigner(stateSubscriber));
    }
    private IEnumerable<IStateSubscriber> FindAllIStateSubscribers()
    {//Busca todos los GameObject que contengan IStateSubscribers y luego devuelve todos esos IStateSubscribers aplanados
        return Resources.FindObjectsOfTypeAll<GameObject>().Where(gameObject => gameObject.GetComponent<IStateSubscriber>() != null).SelectMany(gameObject => gameObject.GetComponents<IStateSubscriber>());
    }
    private static void AddToAssigner(IStateSubscriber stateSubscriber)
    {//Recibe un suscriptor y anade sus subscripciones a el diccionario insertando en todos los estados indicados el execution segun su prioridad
        stateSubscriber.GetStateSubscriptions.ForEach(stateSubscription => stateSubscription.States.ForEach(state => InsertByPriority(stateSubscription, state)));
    }
    private static void InsertByPriority(StateSubscription stateSubscription, State state)
    {//Inserta el Execution de stateSubscription en la lista del diccionario correspondiente al estado segun su prioridad
        List<Execution> executions = assigner[state];
        Execution newExecution = stateSubscription.Execution;
        for (int i = 0; i < executions.Count; i++)
        {//Itera por cada uno de los execution de la lista correspondiente a ese estado, cuando encuentra uno de menor (thatPriority>myPriority) prioridad se inserta en su lugar
            if (executions[i].Priority > newExecution.Priority) { executions.Insert(i, newExecution); return; }
        }//Si no se encuentra que exista algun Execution en esa lista de menor prioridad se anade al final
        executions.Add(newExecution);
    }
    public static void ResetGame() => Publish(State.SettingUpGame);//Reinicia el juego, este metodo es llamado por un boton que aparece cuando acaba el juego llamado ResetGameButton
    public static void Publish(State state, StateInfo stateInfo = null) => assigner[state].ForEach(execution => execution.TriggerCode(stateInfo));//Llama a todo el codigo asignado al estado
}
