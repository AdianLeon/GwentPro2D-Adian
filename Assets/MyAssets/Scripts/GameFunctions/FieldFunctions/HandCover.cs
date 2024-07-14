//Script para activar el cover de las manos cuando sea turno del enemigo
public class HandCover : StateListener
{
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame://Si es el turno de su enemigo, se activa, si es el turno de su jugador, se desactiva
            case State.EndingTurn:
            case State.EndingRound:
                this.gameObject.SetActive(GFUtils.GetField(this.name)==Judge.GetEnemy);
                break;
            case State.EndingGame://Si es el fin del juego se activa
                this.gameObject.SetActive(true);
                break;
        }
    }
}
