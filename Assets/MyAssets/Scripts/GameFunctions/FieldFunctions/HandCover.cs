using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//Script para reaccionar activar el objeto cuando sea turno del enemigo
public class HandCover : StateListener
{
    public override void CheckState(){//Si es el turno de su enemigo, se activa, si es el turno de su jugador, se desactiva
        switch(Judge.CurrentState){
            case State.SettingUpGame:
            case State.EndingTurn:
            case State.EndingRound:
                this.gameObject.SetActive(GFUtils.GetField(this.name)==Judge.GetEnemy);
                break;
            case State.EndingGame:
                this.gameObject.SetActive(true);
                break;
        }
    }
}
