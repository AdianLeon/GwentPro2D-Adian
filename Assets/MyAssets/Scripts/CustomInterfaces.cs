using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Script que declara las interfaces a utilizar
interface IEffect{//Para las cartas con efecto
    void TriggerEffect();//Este metodo se llama cuando se desee activar el efecto
}
interface ICardEffect: IEffect{}//Para las cartas cuyo efecto se activa cuando se juega
interface ILeaderEffect: IEffect{}//Para las cartas cuyo efecto se activa cuando se presiona
interface IAffectable{//Para las cartas que sean afectables por efectos de cartas especiales
    List<string> AffectedByWeathers{get; set;}//Devuelve una lista de las cartas clima que le afectan
}
interface IShowZone{//Para las cartas que deben iluminar alguna zona
    void ShowZone();//Se llama este metodo cuando se desean iluminar esas zonas
}
interface IIdle{}//Para las cartas que no son arrastrables