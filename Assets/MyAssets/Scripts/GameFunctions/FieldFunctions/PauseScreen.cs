//Script para marcar las pantallas de pausa
public class PauseScreen : StateListener
{
    public override int GetPriority=>0;
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.SettingUpGame://Cuando inicia el juego se desactiva
                gameObject.SetActive(false);
                return;
            case State.EndingGame://Cuando el juego termina se activa
                gameObject.SetActive(true);
                return;
        }
    }
}
