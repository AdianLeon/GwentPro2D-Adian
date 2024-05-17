using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script padre de todos los efectos de las cartas (especiales y de unidad)
public class CardEffect : MonoBehaviour
{
    //Este metodo se llama siempre que se coloque una carta, si no se modifica desde el script del efecto particular, entonces se ejecuta este (no hace nada)
    virtual public void TriggerEffect(){}
}
