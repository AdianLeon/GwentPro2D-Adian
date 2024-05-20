using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas que tienen poder (Cartas de unidad y senuelos)
abstract public class CardWithPower : Card
{
    public int power;//Poder propio de la carta
    public int addedPower;//Poder anadido por efectos durante el juego
    public bool[] affected=new bool[4];//Un array que describe si la carta esta siendo afectada por un clima, la posicion del true es el id de la carta clima que la afecta
    
    public override void LoadInfo(){
        base.LoadInfo();
        //Power
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=power.ToString();
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
        
        //AddedPower
        if(addedPower>0){//Se actualiza el poder anadido, en caso de ser positivo se pone verde y si es negativo se pone rojo
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="+"+addedPower.ToString();
            GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().color=new Color(0,1,0,1);
        }else if(addedPower<0){
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text=addedPower.ToString();
            GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().color=new Color(1,0,0,1);
        }else{
            GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
            GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        }
    }
}
