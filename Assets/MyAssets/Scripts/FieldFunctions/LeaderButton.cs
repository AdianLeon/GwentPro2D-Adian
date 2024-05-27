using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para hacer que la habilidad de lider funcione
public class LeaderButton : MonoBehaviour
{
    public Button thisLeaderButton;//El boton del objeto
    void Start(){
        thisLeaderButton.onClick.AddListener(OnButtonClick);//Ejecuta el metodo OnButtonClick cuando el boton se presione
    }
    private void OnButtonClick(){
        if(this.gameObject.GetComponent<LeaderEffect>()!=null){//Si hay efecto lider
            this.gameObject.GetComponent<LeaderEffect>().TriggerLeaderEffect();//Activa el efecto del lider
        }
    }
}
