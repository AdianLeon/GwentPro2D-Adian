using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitCard : Card
{
    public int power;//Poder propio de la carta
    public int addedPower;//Poder anadido por efectos durante el juego
    public bool[] affected=new bool[4];//Un array que describe si la carta esta siendo afectada por un clima, la posicion del true es el id de la carta clima que la afecta
    
    public enum quality{None,Silver,Gold}//Calidad de la carta, si es plata tendra hasta 3 copias, si es oro no sera afectada por ningun efecto durante el juego
    public quality wichQuality;
    public override void LoadInfo(){
        base.LoadInfo();
        if(whichZone==zones.Melee){
            GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[M]";
        }else if(whichZone==zones.Ranged){
            GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[R]";
        }else{//Para los otros dos tipos que quedan sin especificar (Asedio y Senuelo) corresponde una S en ambos casos
            GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[S]";
        }
        //Power
        if(power!=0){
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=power.ToString();
            GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
        }else{
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
            GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);
        }
        if(power==0){//Senuelo o Minion
            GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=power.ToString();
            GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0.8f);
        }
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
