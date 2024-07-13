using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
//Script de las luces del juego que indican si se puede jugar
public class PlayedLight : StateListener, IGlow
{
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame:
                OnGlow();
                break;
            case State.PlayingCard:
            case State.EndingTurn:
            case State.EndingRound:
                if(Judge.CanPlay){OnGlow();}else{OffGlow();}
                break;
            case State.EndingGame:
                OffGlow();
                break;
        }
    }
    public void OnGlow(){//Pone el objeto en verde
        this.gameObject.GetComponent<UnityEngine.UI.Image>().color=new Color(0,1,0,0.2f);
    }
    public void OffGlow(){//Pone el objeto en rojo
        this.gameObject.GetComponent<UnityEngine.UI.Image>().color=new Color(1,0,0,0.2f);
    }
}
