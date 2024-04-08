using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script que contiene las propiedades de todas las cartas
public class Card : MonoBehaviour
{
    public int id;//Identificador unico de las cartas de clima y las de efecto
    public int power;//Poder propio de la carta
    public int addedPower;//Poder anadido por efectos durante el juego

    public string cardRealName;//Nombre a mostrar en el objeto gigante a la izquierda del campo
    public string description;//Descripcion de la carta a mostrar en el objeto gigante a la izquierda del campo

    public bool hasEffect;//Si tiene efecto o no
    public string effectDescription;//Descripcion del efecto

    public Sprite artwork;//Imagen relacionada con la carta para mostrar en grande en el objeto gigante a la izquierda del campo
    public Sprite qualitySprite;//Otra imagen que representa al enum quality
    public Color cardColor;//Color determinado de la carta

    public bool[] affected=new bool[4];//Un array que describe si la carta esta siendo afectada por un clima, la posicion del true es el id de la carta clima que la afecta
    
    public enum quality{None,Silver,Gold}//Calidad de la carta, si es plata tendra hasta 3 copias, si es oro no sera afectada por ningun efecto durante el juego
    public quality wQuality;
}
