using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas clima y despeje
public class WeatherCard : Card
{
    public int id;//Identificador unico de las cartas de clima
    public override void LoadInfo(){
        base.LoadInfo();
        if(this.gameObject.GetComponent<WeatherCard>().id==4){//Si es despeje
            GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[D]";
        }else{
            GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[C]";
        }

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
    }
}
