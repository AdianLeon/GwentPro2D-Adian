using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de la carta lider
public class GruEffect : MonoBehaviour, ILeaderEffect, IToJson
{
    private static GameObject GetHand{get=>GameObject.Find("Hand"+Board.GetPlayerTurn);}
    private static GameObject GetEnemyHand{get=>GameObject.Find("Hand"+Board.GetEnemyTurn);}
    public void TriggerEffect(){
        if(GetEnemyHand.transform.childCount<2){//Si el enemigo tiene una o ninguna carta
            RoundPoints.LongWriteUserRead("Los minions intentaron robar dos cartas de la mano enemiga, pero tenia menos de dos y les dio lastima, intentalo mas tarde");
            return;
        }
        LeaderSkill();//Activa la habilidad de lider
    }
    private void LeaderSkill(){
        int r=UnityEngine.Random.Range(0,4);
        GameObject cardToSteal=null;
        GameObject cardToSteal2=null;
        if(r==0){//Se roba 2 cartas al enemigo
            cardToSteal=GetRandomCardFromHand(GetEnemyHand);
            StealCardTo(GetHand,cardToSteal);
            cardToSteal2=GetRandomCardFromHand(GetEnemyHand);
            StealCardTo(GetHand,cardToSteal2);
            RoundPoints.LongWriteUserRead("Los minions han robado dos cartas de la mano enemiga exitosamente");
        }else if(r==1 || r==2){//Se roba una carta de la mano enemiga
            cardToSteal=GetRandomCardFromHand(GetEnemyHand);
            StealCardTo(GetHand,cardToSteal);
            RoundPoints.LongWriteUserRead("Los minions han intentado robar dos cartas al enemigo, pero ha logrado recuperar una");
        }else{//No pasa nada
            RoundPoints.LongWriteUserRead("Los minions intentaron robar dos cartas al enemigo, pero salio mal y recupero ambas");
        }
        if(GetHand.transform.childCount==11){//Se sobrepasa por una carta la capacidad de la mano
            Graveyard.SendToGraveyard(cardToSteal);//Se envia al cementerio
            if(r==0){//Si se robaron dos cartas
                RoundPoints.LongWriteUserRead("Los minions robaron dos cartas de la mano enemiga, pero quedaba un solo espacio en la mano (una de ellas se envio al cementerio)");
            }else{//Si se robo solo una carta
                RoundPoints.LongWriteUserRead("Los minions robaron una carta de la mano enemiga, pero no habia espacio en la mano (se envio al cementerio)");
            }
        }else if(GetHand.transform.childCount==12){//Si se sobrepasa la capacidad de la mano por dos cartas
            Graveyard.SendToGraveyard(cardToSteal);//Como unico esto puede pasar es robando dos cartas
            Graveyard.SendToGraveyard(cardToSteal2);//Asi que ambas son descartadas al cementerio
            RoundPoints.LongWriteUserRead("Los minions robaron dos cartas de la mano enemiga, pero no habia espacio en la mano (ambas se enviaron al cementerio)");
        }
    }
    private void StealCardTo(GameObject hand, GameObject cardToSteal){//Robar de un jugador
        Fields stealerField=(Fields)Enum.Parse(typeof(Fields),Board.GetPlayerTurn);//El campo del ladron es el turno actual
        cardToSteal.transform.SetParent(hand.transform);//Pone la carta robada en la mano del ladron
        cardToSteal.GetComponent<Card>().WhichField=stealerField;//Cambia el campo de la carta
    }
    private GameObject GetRandomCardFromHand(GameObject targetHand){//Devuelve una carta aleatoria de la mano objetivo
        return targetHand.transform.GetChild(UnityEngine.Random.Range(0,targetHand.transform.childCount)).gameObject;
    }
}