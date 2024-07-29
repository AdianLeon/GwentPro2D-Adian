//Declaracion de los enums a utilizar en las funciones del juego
public enum Player { None, P1, P2 }//Jugadores
public enum State { LoadingCards, SettingUpGame, PlayingCard, EndingTurn, EndingRound, EndingGame }//Estados del juego
public enum UnitDropZoneType { M, R, S };//Tipos de dropzones de cartas de unidad
public enum UnitCardZone { M, R, S, MR, MS, RS, MRS };//Zona(s) donde la carta unidad se puede jugar