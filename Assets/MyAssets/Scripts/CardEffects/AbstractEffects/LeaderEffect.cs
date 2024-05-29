using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script padre de todos los efectos de lider
abstract public class LeaderEffect : CardEffect
{
    public virtual void TriggerLeaderEffect(){}//Esta funcion es la que sobreescribiremos para describir el efecto del lider
}
