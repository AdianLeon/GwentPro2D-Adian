using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para mostrar mensajes en un objeto llamado UserRead
public class UserRead : MonoBehaviour, IStateSubscriber, IKeyboardListener
{
    private static List<string> messages = new List<string>();//Lista de mensajes
    private static int currentMessage;//Indexador que escoge el mensaje a mostrar en la lista
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (State.SettingUpGame, new Execution (stateInfo => { messages = new List<string>(); Write("Ha comenzado una nueva partida, es el turno de P1"); }, 0)),
        new (new List<State> { State.PlayingCard, State.EndingTurn }, new Execution (stateInfo => WriteRoundInfo(), 1) ),
        new StateSubscription(State.EndingRound, new Execution (stateInfo => Write( MakeEndRoundMessage(stateInfo.Player) ), 0)),
        new (State.EndingGame, new Execution (stateInfo => Write( MakeEndGameMessage(stateInfo.Player) ), 0))
    };
    private static void WriteRoundInfo()
    {//Se llama cuando se desea escribir la informacion de ronda
        if (!Judge.HasPlayed && !Judge.IsLastTurnOfRound) { Write("Es el turno de " + Judge.GetPlayer); return; }
        if (Computer.IsPlaying) { return; }
        if (Judge.IsLastTurnOfRound) { Write("Turno de " + Judge.GetPlayer + ", es el ultimo turno antes de que se acabe la ronda"); }//Si es el ultimo turno
        else if (Judge.HasPlayed) { Write("Presiona espacio para pasar de turno"); }//Si se han jugado cartas y no es el ultimo turno
    }
    private static string MakeEndRoundMessage(Player winner) => winner == Player.None ? "Ha ocurrido un empate" : winner + " gano la ronda";
    private static string MakeEndGameMessage(Player winner) => Computer.IsActive && winner == Player.P2 ? "Has perdido!!" : "Felicidades " + winner + ". Has ganado la partida!!";
    public void ListenToKeyboardPress()
    {//Si se presiona la flecha izquierda o derecha se navega por los mensajes
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { PreviousMessage(); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { NextMessage(); }
    }
    public static void PreviousMessage()
    {//Este metodo es llamado por el boton a la izquierda del UserRead o por presionar la flecha izquierda
        currentMessage--;
        if (currentMessage < 0) { currentMessage = 0; }
        UpdateMessage();
    }
    public static void NextMessage()
    {//Este metodo es llamado por el boton a la derecha del UserRead o por presionar la flecha derecha
        currentMessage++;
        if (currentMessage > messages.Count - 1) { currentMessage = messages.Count - 1; }
        UpdateMessage();
    }
    public static void Write(string passedMessage)
    {//Se llama cuando se desea poner un mensaje en el UserRead, pero si se ha escrito un mensaje Long en los ultimos 2s entonces no puede mostrarse
        messages.Add(passedMessage);
        currentMessage = messages.Count - 1;
        UpdateMessage();
    }
    private static void UpdateMessage()
    {//Actualiza el mensaje del UserRead con el mensaje seleccionado
        Show(messages[currentMessage]);
        GameObject.Find("MessageNumber").GetComponent<TextMeshProUGUI>().text = (currentMessage + 1).ToString() + "/" + messages.Count;
    }
    public static void Show(string passedMessage) => GameObject.Find("UserReadZone").GetComponent<TextMeshProUGUI>().text = passedMessage;//Muestra el mensaje pasado como parametro en el UserRead directamente
}
