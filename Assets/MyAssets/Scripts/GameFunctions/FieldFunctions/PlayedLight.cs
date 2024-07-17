using UnityEngine;
//Script de las luces del juego que indican si se puede jugar
public class PlayedLight : StateListener, IGlow
{
    public override int GetPriority=>1;
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame://Si se pude jugar se pone verde, si no rojo
            case State.PlayingCard:
            case State.EndingTurn:
            case State.EndingRound:
                if(Judge.CanPlay){OnGlow();}else{OffGlow();}
                break;
            case State.EndingGame://Cuando es el final del juego se deja en rojo
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
