using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para activar cuando esta desactivado y desactivar cuando esta activado un objeto determinado
public class PassButtonScript : MonoBehaviour
{
    public GameObject objectToToggle;//Objeto a togglear

    public void Toggle(){
        objectToToggle.SetActive(!objectToToggle.activeSelf);//Setea al estado en el que no esta
    }
}