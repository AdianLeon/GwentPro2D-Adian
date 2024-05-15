using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderCard : Card
{
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[L]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);

        if(this.GetComponent<LeaderEffect>().used){
            this.GetComponent<Image>().color=new Color(0.3f,0.3f,0.3f,1);
        }
    }
}