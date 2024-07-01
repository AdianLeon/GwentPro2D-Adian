using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//Script para hacer que la habilidad de lider funcione
public class LeaderButton : MonoBehaviour
{
    public Button thisLeaderButton;//El boton del objeto
    void Start(){
        thisLeaderButton.onClick.AddListener(OnButtonClick);//Ejecuta el metodo OnButtonClick cuando el boton se presione
    }
    private void OnButtonClick(){//Se llama cuando se presiona en el lider
        if(TurnManager.cardsPlayed>0 && !TurnManager.lastTurn){RoundPoints.URWriteRoundInfo();return;}//Si se han jugado cartas y no es el ultimo turno
        if(this.gameObject.GetComponent<LeaderCard>()==null){return;}//Si no es una carta lider
        LeaderCard leaderComponent=this.gameObject.GetComponent<LeaderCard>();
        if(leaderComponent.usedSkill){
            RoundPoints.URLongWrite("La habilidad del lider: "+leaderComponent.cardRealName+" ya ha sido usada. La habilidad de lider solo puede ser usada una vez por partida");
            return;}//Si la habilidad de este lider ya ha sido usada previamente
        if(leaderComponent.WhichField.ToString()!="P"+TurnManager.playerTurn.ToString()){//Si no coincide en campo con el jugador que lo presiona
            RoundPoints.URWrite("Ese no es el lider de tu deck");return;}

        //Juega la carta lider
        TurnManager.PlayLeaderCard(this.gameObject);
    }
}
