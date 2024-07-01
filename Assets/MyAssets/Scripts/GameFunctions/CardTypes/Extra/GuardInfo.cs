using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para ensenar la info de los guardias
public class GuardInfo : Card
{
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
    }
}
