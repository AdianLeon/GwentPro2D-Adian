using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//Script para reaccionar activar el objeto cuando sea turno del enemigo
public class HandCover : CustomBehaviour
{
    public override void Finish(){
        this.gameObject.SetActive(true);
    }
    public override void Initialize(){
        NextUpdate();
    }
    public override void NextUpdate(){//Si es el turno de su enemigo, se activa, si es el turno de su jugador, se desactiva
        this.gameObject.SetActive(GFUtils.GetField(this.name)==Judge.GetEnemy);
    }
}
