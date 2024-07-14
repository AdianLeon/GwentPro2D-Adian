using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para mostrar mensajes en un objeto llamado UserRead
public class UserRead : StateListener
{
    private List<string> messages;//Lista de mensajes
    private int currentMessage;//Indexador que escoge el mensaje a mostrar en la lista
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame://Cuando inicia el juego limpia la lista y escribe un mensaje inicial
                messages=new List<string>();
                Write("Ha comenzado una nueva partida, es el turno de P1");
                break;
            case State.PlayingCard://Cuando se juega una carta o al final de turnos y rondas escribe informacion sobre la ronda
            case State.EndingTurn:
            case State.EndingRound:
                WriteRoundInfo();
                break;
            case State.EndingGame://Al final del juego escribe el mensaje de victoria con el turno del jugador (Cuando se acaba el juego el turno del jugador se usa para determinar el ganador)
                Write("Felicidades "+Judge.GetPlayer+". Has ganado la partida!!");
                break;
        }
    }
    void Update(){//Si se presiona la flecha izquierda o derecha se navega por los mensajes
        if(Input.GetKeyDown(KeyCode.LeftArrow)){PreviousMessage();}
        if(Input.GetKeyDown(KeyCode.RightArrow)){NextMessage();}
    }
    private void UpdateMessage(){//Actualiza el mensaje del UserRead con el mensaje seleccionado
        Show(messages[currentMessage]);
        GameObject.Find("MessageNumber").GetComponent<TextMeshProUGUI>().text=(currentMessage+1).ToString()+"/"+messages.Count;
    }
    public void PreviousMessage(){//Este metodo es llamado por el boton a la izquierda del UserRead o por presionar la flecha izquierda
        currentMessage--;
        if(currentMessage<0){currentMessage=0;}
        UpdateMessage();
    }
    public void NextMessage(){//Este metodo es llamado por el boton a la derecha del UserRead o por presionar la flecha derecha
        currentMessage++;
        if(currentMessage>messages.Count-1){currentMessage=messages.Count-1;}
        UpdateMessage();
    }
    public void Show(string passedMessage){//Muestra el mensaje pasado como parametro en el UserRead directamente
        GameObject.Find("UserRead").GetComponent<TextMeshProUGUI>().text=passedMessage;
    }
    public void Write(string passedMessage){//Se llama cuando se desea poner un mensaje en el UserRead, pero si se ha escrito un mensaje Long en los ultimos 2s entonces no puede mostrarse
        messages.Add(passedMessage);
        currentMessage=messages.Count-1;
        UpdateMessage();
    }
    public void WriteRoundInfo(){//Se llama cuando se desea escribir la informacion de ronda
        if(Judge.GetTurnActionsCount==0 && Judge.IsLastTurn){//Si no se han jugado cartas y es el ultimo turno
            Write("Turno de "+Judge.GetPlayer+", es el ultimo turno antes de que se acabe la ronda");
        }else if(Judge.GetTurnActionsCount==0){//Si no es el ultimo turno pero no se han jugado cartas
            Write("Turno de "+Judge.GetPlayer);
        }else if(Judge.IsLastTurn){//Si se han jugado cartas y es el ultimo turno
            Write("Puedes seguir jugando mas cartas. Presiona espacio cuando desees acabar la ronda");
        }else{//Si se han jugado cartas y no es el ultimo turno
            Write("Presiona espacio para pasar de turno");
        }
    }
}
