using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para la carta Lider
public class LeaderCard : Card
{
    public override Color GetCardViewColor(){return new Color(0.7f,0.1f,0.5f);}
    private bool usedSkill;//Si la habilidad ha sido usada
    public bool UsedSkill{get=>usedSkill; set=>usedSkill=value;}
    private Button thisLeaderButton;//El boton del objeto
    void Start(){
        thisLeaderButton=GetComponent<Button>();
        thisLeaderButton.onClick.AddListener(OnButtonClick);//Ejecuta el metodo OnButtonClick cuando el boton se presione
    }
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[L]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,0);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
    }
    private void OnButtonClick(){//Se llama cuando se presiona en el lider
        if(!Board.CanPlay){//Si no se puede jugar
            RoundPoints.WriteRoundInfoUserRead();
            return;
        }
        if(WhichField.ToString()!=Board.GetPlayer){//Si no coincide en campo con el jugador que lo presiona
            RoundPoints.WriteUserRead("Ese no es el lider de tu deck");
            return;
        }
        if(usedSkill){//Si la habilidad de este lider ya ha sido usada previamente
            RoundPoints.LongWriteUserRead("La habilidad del lider: "+cardName+" ya ha sido usada. La habilidad de lider solo puede ser usada una vez por partida");
            return;
        }

        //Juega la carta lider
        Board.PlayLeaderCard(this.gameObject);
    }
}
