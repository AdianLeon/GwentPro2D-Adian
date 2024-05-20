using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para ensenar la info de los guardias
public class GuardInfo : Card
{
    public override void LoadInfo(){//Esta funcion es especifica para cada tipo de carta, pero todas comparten lo siguiente
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGType").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);
    }
}
