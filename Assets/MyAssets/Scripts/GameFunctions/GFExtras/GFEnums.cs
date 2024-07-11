using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script que declara los enums a utilizar en las funciones del juego
public enum Player{P1,P2}//Jugadores
public enum States{Initializing,WaitPlayerAction,}//Estados del juego
public enum ZonesDZ{M,R,S};//Zona valida de las dropzones de cartas de unidad
public enum ZonesUC{M,R,S,MR,MS,RS,MRS};//Zona(s) donde la carta unidad se puede jugar