using System.Collections.Generic;
//Script que declara las interfaces a utilizar
interface IStateListener
{//Para aquellos scripts que deban ejecutar codigo en alguno de los estados definidos
    public abstract int GetPriority { get; }//Para definir un orden de prioridad y ejecutar primero aquellos scripts de mayor prioridad
    public abstract void CheckState();//Realiza acciones en dependencia del estado actual
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
interface IShowZone
{//Para las cartas que deben iluminar alguna zona
    void ShowZone();//Se llama este metodo cuando se desee iluminar esas zonas
}
interface IGlow
{//Para los objetos que se iluminan
    void OnGlow();//Activa su iluminacion
    void OffGlow();//Desactiva su iluminacion
}
interface IContainer
{//Para aquellos objetos que en el juego contendran cartas
    IEnumerable<DraggableCard> GetCards { get; }//Devuelve una lista con las cartas contenidas en el objeto
}
interface IKeyboardListener
{
    void ListenToKeyboardPress();
}