using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas clima
public class WeatherCard : Card
{
    public int id;//Identificador unico de las cartas de clima
    public int damage;//Cant de poder restado cuando una carta es afectada por el clima
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[C]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="-"+damage;
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,1);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
    }
    public static GameObject GetWeatherWithID(int passedID){//Devuelve la carta clima con el id que se pasa como parametro
        WeatherCard[] allWeatherCards=GameObject.FindObjectsOfType<WeatherCard>();
        for(int i=0;i<allWeatherCards.Length;i++){
            if(allWeatherCards[i].id==passedID){
                return allWeatherCards[i].gameObject;
            }
        }
        return null;
    }
}
