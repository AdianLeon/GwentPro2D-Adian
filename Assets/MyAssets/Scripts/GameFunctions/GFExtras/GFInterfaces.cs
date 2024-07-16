using System.Collections.Generic;
using UnityEngine;
//Script que declara las interfaces a utilizar
interface IEffect{//Para los efectos
    string GetEffectDescription{get;}
}
interface ISpecialCard : IEffect{//Para las cartas con efecto especial
    void TriggerSpecialEffect();//Este metodo se llama cuando se desee activar el efecto especial
}
interface ICardEffect : IEffect{//Para los scripts que describen un efecto de carta
    void TriggerEffect();//Este metodo se llama cuando se desee activar el efecto de carta
}
interface IAffectable{//Para las cartas que sean afectables por efectos de cartas especiales
    List<WeatherCard> AffectedByWeathers{get; set;}//Lista de las cartas clima que estan afectando la carta
}
interface IShowZone{//Para las cartas que deben iluminar alguna zona
    void ShowZone();//Se llama este metodo cuando se desee iluminar esas zonas
}
interface IGlow{//Para los objetos que se iluminan
    void OnGlow();//Activa su iluminacion
    void OffGlow();//Desactiva su iluminacion
}
interface IContainer{//Para aquellos objetos que en el juego contendran cartas
    List<GameObject> GetCards{get;}//Devuelve una lista con las cartas contenidas en el objeto
}