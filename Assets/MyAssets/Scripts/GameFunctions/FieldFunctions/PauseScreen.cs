using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para marcar las pantallas de pausa
public class PauseScreen : StateListener
{
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame:
                this.gameObject.SetActive(false);
                return;
            case State.EndingGame:
                this.gameObject.SetActive(true);
                return;
        }
    }
}
