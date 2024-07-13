using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//Script para mostrar mensajes en un objeto llamado UserRead
public class UserRead : StateListener
{
    private int currentMessage;
    private List<string> messages;
    private float secCounter;//Contador de segundos
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame:
                messages=new List<string>();
                secCounter=int.MinValue;
                ForcedWrite("Ha comenzado una nueva partida, es el turno de P1");
                break;
            case State.PlayingCard:
            case State.EndingTurn:
            case State.EndingRound:
                UpdateMessage();
                break;
            case State.EndingGame:
                ForcedWrite("Felicidades "+Judge.GetPlayer+". Has ganado la partida!!");
                break;
        }
    }
    //Mensajes en el UserRead
    private void UpdateMessage(){//Actualiza el mensaje del UserRead
        GameObject.Find("MessageNumber").GetComponent<TextMeshProUGUI>().text=(currentMessage+1).ToString();
        GameObject.Find("UserRead").GetComponent<TextMeshProUGUI>().text=messages[currentMessage];
    }
    public void PreviousMessage(){//Este metodo es llamado por el boton a la izquierda del UserRead 
        currentMessage--;
        if(currentMessage<0){currentMessage=0;}
        UpdateMessage();
    }
    public void NextMessage(){//Este metodo es llamado por el boton a la derecha del UserRead
        currentMessage++;
        if(currentMessage>messages.Count-1){currentMessage=messages.Count-1;}
        UpdateMessage();
    }
    public void Write(string passedMessage){//Se llama cuando se desea poner un mensaje en el UserRead, pero si se ha escrito un mensaje Long entonces no puede efectuarse por 2s
        if(Time.time-secCounter>2){
            ForcedWrite(passedMessage);
        }
    }
    public void LongWrite(string passedMessage){//Se llama cuando se desea poner un mensaje importante en el UserRead (dura 2s)
        ForcedWrite(passedMessage);
        secCounter=Time.time;
    }
    private void ForcedWrite(string passedMessage){//Escribe el mensaje ignorando los 2s
        messages.Add(passedMessage);
        currentMessage=messages.Count-1;
        UpdateMessage();
    }
    public void WriteRoundInfo(){//Se llama cuando se desea escribir la informacion de ronda
        if(Judge.GetTurnActionsCount==0 && Judge.IsLastTurn){//Si se puede jugar y es el ultimo turno
            Write("Turno de "+Judge.GetPlayer+", es el ultimo turno antes de que se acabe la ronda");
        }else if(Judge.GetTurnActionsCount==0){//Si no es el ultimo turno pero se puede jugar
            Write("Turno de "+Judge.GetPlayer);
        }else if(Judge.IsLastTurn){//Si "no se puede jugar" pero es el ultimo turno
            Write("Puedes seguir jugando mas cartas. Presiona espacio cuando desees acabar la ronda");
        }else{//Si no se puede jugar y no es el ultimo turno
            Write("Presiona espacio para pasar de turno");
        }
    }
}
