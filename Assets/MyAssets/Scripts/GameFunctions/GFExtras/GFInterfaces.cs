using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Script que declara las interfaces a utilizar
interface ISpecialCard{//Para las cartas con efecto especial
    void TriggerSpecialEffect();//Este metodo se llama cuando se desee activar el efecto especial
}
interface ICardEffect{//Para los scripts que describen un efecto carta
    void TriggerEffect();//Este metodo se llama cuando se desee activar el efecto descrito
}
interface IAffectable{//Para las cartas que sean afectables por efectos de cartas especiales
    List<string> AffectedByWeathers{get; set;}//Lista de los nombres de las cartas clima que le afecten
}
interface IShowZone{//Para las cartas que deben iluminar alguna zona
    void ShowZone();//Se llama este metodo cuando se desean iluminar esas zonas
}
interface IGlow{//Para las zonas y cartas que se iluminan
    void OnGlow();//Activa su iluminacion
    void OffGlow();//Desactiva su iluminacion
}
interface IContainer{//Para aquellos objetos que en el juego contendran cartas
    List<GameObject> GetCards{get;}//Devuelve una lista con las cartas contenidas en el objeto
}