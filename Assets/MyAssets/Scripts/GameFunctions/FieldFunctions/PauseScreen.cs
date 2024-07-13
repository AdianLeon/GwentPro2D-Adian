using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para marcar las pantallas de pausa
public class PauseScreen : CustomBehaviour
{
    public override void Finish(){
        this.gameObject.SetActive(true);
    }
    public override void Initialize(){
        this.gameObject.SetActive(false);
    }
    public override void NextUpdate(){}
}
