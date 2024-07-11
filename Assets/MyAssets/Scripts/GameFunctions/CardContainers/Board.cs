using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para la logica de los turnos
public class Board : MonoBehaviour, IContainer
{
    public List<GameObject> GetCards{get=>GFUtils.GetCardsIn(this.gameObject);}//Lista de cartas en el campo
    
}