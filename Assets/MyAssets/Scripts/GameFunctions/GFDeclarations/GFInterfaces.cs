using System.Collections.Generic;
//Script que declara las interfaces a utilizar
interface IStateSubscriber
{//Para aquellos scripts que deban ejecutar codigo en alguno de los estados definidos
    public List<StateSubscription> GetStateSubscriptions { get; }
}
interface IKeyboardListener
{//Para aquellos scripts que deban reaccionar ante la presion de alguna tecla por parte del usuario
    void ListenToKeyboardPress();
}
interface IEffect
{//Para los efectos
    string GetEffectDescription { get; }
}
interface ISpecialCard : IEffect
{//Para las cartas con efecto especial
    void TriggerSpecialEffect();//Este metodo se llama cuando se desee activar el efecto especial
}
interface ICardEffect : IEffect
{//Para los scripts que describen un efecto de carta
    void TriggerEffect();//Este metodo se llama cuando se desee activar el efecto de carta
}
interface IAffectable
{//Para las cartas que sean afectables por efectos de cartas especiales
    List<WeatherCard> WeathersAffecting { get; }//Lista de las cartas clima que estan afectando la carta
}
interface IGlow
{//Para los objetos que se iluminan
    void TriggerGlow();//Activa su iluminacion
    void RestoreGlow();//Desactiva su iluminacion
}
interface IContainer
{//Para aquellos objetos que en el juego contendran cartas
    IEnumerable<DraggableCard> GetCards { get; }//Devuelve una lista con las cartas contenidas en el objeto
}