using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para mostrar mensajes en un objeto llamado UserRead
public class UserRead : MonoBehaviour, IStateListener, IKeyboardListener
{
    public int GetPriority => 1;
    private static List<string> messages;//Lista de mensajes
    private static int currentMessage;//Indexador que escoge el mensaje a mostrar en la lista
    public void CheckState()
    {
        switch (Judge.CurrentState)
        {
            case State.SettingUpGame://Cuando inicia el juego limpia la lista y escribe un mensaje inicial
                messages = new List<string>();
                Write("Ha comenzado una nueva partida, es el turno de P1");
                break;
            case State.EndingTurn://Al final de turnos y rondas escribe informacion sobre la ronda
            case State.EndingRound:
                WriteRoundInfo();
                break;
            case State.EndingGame://Al final del juego escribe el mensaje de victoria con el turno del jugador (Cuando se acaba el juego el turno del jugador se usa para determinar el ganador)
                Write("Felicidades " + Judge.GetPlayer + ". Has ganado la partida!!");
                break;
        }
    }
    public void ListenToKeyboardPress()
    {//Si se presiona la flecha izquierda o derecha se navega por los mensajes
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { PreviousMessage(); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { NextMessage(); }
    }
    private static void WriteRoundInfo()
    {//Se llama cuando se desea escribir la informacion de ronda
        if (PlayerPrefs.GetInt("SinglePlayerMode") == 1 && Judge.GetPlayer == Player.P2) { Write("Es el turno del enemigo..."); return; }//Mensaje para mostrar cuando sea el turno de la computadora

        if (Judge.HasPlayed && Judge.IsLastTurnOfRound) { Write("Turno de " + Judge.GetPlayer + ", es el ultimo turno antes de que se acabe la ronda"); }//Si no se han jugado cartas y es el ultimo turno
        else if (Judge.HasPlayed) { Write("Turno de " + Judge.GetPlayer); }//Si no es el ultimo turno pero no se han jugado cartas
        else if (Judge.IsLastTurnOfRound) { Write("Puedes seguir jugando mas cartas. Presiona espacio cuando desees acabar la ronda"); }//Si se han jugado cartas y es el ultimo turno
        else { Write("Presiona espacio para pasar de turno"); }//Si se han jugado cartas y no es el ultimo turno
    }
    private static void UpdateMessage()
    {//Actualiza el mensaje del UserRead con el mensaje seleccionado
        Show(messages[currentMessage]);
        GameObject.Find("MessageNumber").GetComponent<TextMeshProUGUI>().text = (currentMessage + 1).ToString() + "/" + messages.Count;
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
    public static void Show(string passedMessage) { GameObject.Find("UserReadZone").GetComponent<TextMeshProUGUI>().text = passedMessage; }//Muestra el mensaje pasado como parametro en el UserRead directamente
    public static void Write(string passedMessage)
    {//Se llama cuando se desea poner un mensaje en el UserRead, pero si se ha escrito un mensaje Long en los ultimos 2s entonces no puede mostrarse
        messages.Add(passedMessage);
        currentMessage = messages.Count - 1;
        UpdateMessage();
    }
}
