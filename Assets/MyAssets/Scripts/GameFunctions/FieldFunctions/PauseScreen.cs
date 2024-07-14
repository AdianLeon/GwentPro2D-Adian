//Script para marcar las pantallas de pausa
public class PauseScreen : StateListener
{
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame://Cuando inicia el juego se desactiva
                this.gameObject.SetActive(false);
                return;
            case State.EndingGame://Cuando el juego termina se activa
                this.gameObject.SetActive(true);
                return;
        }
    }
}
