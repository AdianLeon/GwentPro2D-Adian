using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas que tienen poder, poder anadido y que se pueden afectar por efectos de cartas especiales (Cartas de unidad (oro y plata) y senuelos)
abstract public class PowerCard : DraggableCard
{
    public int Power;//Poder de la carta
    public int AddedPower;//Poder anadido por efectos durante el juego
    public int TotalPower=>Power+AddedPower;//Poder total
    public override void LoadInfo(){
        base.LoadInfo();
        //Power
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=Power.ToString();
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);

        //AddedPower
        if(AddedPower>0){//Se actualiza el poder anadido, en caso de ser positivo se pone verde y si es negativo se pone rojo
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="+"+AddedPower.ToString();
            GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().color=new Color(0,1,0,1);
        }else if(AddedPower<0){
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text=AddedPower.ToString();
            GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().color=new Color(1,0,0,1);
        }else{
            GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        }
    }
}