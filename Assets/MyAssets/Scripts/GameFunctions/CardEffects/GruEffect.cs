using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de la carta lider
public class GruEffect : MonoBehaviour, ICardEffect
{
    private static GameObject GetHand{get=>GameObject.Find("Hand"+Judge.GetPlayer);}
    private static GameObject GetEnemyHand{get=>GameObject.Find("Hand"+Judge.GetEnemy);}
    public void TriggerEffect(){
        if(GetEnemyHand.transform.childCount<2){//Si el enemigo tiene una o ninguna carta
            GFUtils.UserRead.LongWrite("Los minions intentaron robar dos cartas de la mano enemiga, pero tenia menos de dos y les dio lastima, intentalo mas tarde");
            return;
        }
        LeaderSkill();//Activa la habilidad de lider
    }
    private static void LeaderSkill(){
        int r=UnityEngine.Random.Range(0,4);
        switch (r){
            case 0://Se roba 2 cartas al enemigo
                StealCardTo(GetHand,GetRandomCardFromHand(GetEnemyHand));
                StealCardTo(GetHand,GetRandomCardFromHand(GetEnemyHand));
                GFUtils.UserRead.LongWrite("Los minions han robado dos cartas de la mano enemiga exitosamente");
                break;
            case 1://Se roba 1 carta al enemigo
            case 2:
                StealCardTo(GetHand,GetRandomCardFromHand(GetEnemyHand));
                GFUtils.UserRead.LongWrite("Los minions han intentado robar dos cartas al enemigo, pero ha logrado recuperar una");
                break;
            case 3://Fallo
                GFUtils.UserRead.LongWrite("Los minions intentaron robar dos cartas al enemigo, pero salio mal y recupero ambas");
                break;
        }
    }
    private static void StealCardTo(GameObject hand, GameObject cardToSteal){//Robar de un jugador
        Player stealerField=(Player)Enum.Parse(typeof(Player),Judge.GetPlayer.ToString());//El campo del ladron es el turno actual
        cardToSteal.transform.SetParent(hand.transform);//Pone la carta robada en la mano del ladron
        cardToSteal.GetComponent<Card>().WhichPlayer=stealerField;//Cambia el campo de la carta
    }
    private static GameObject GetRandomCardFromHand(GameObject targetHand){//Devuelve una carta aleatoria de la mano objetivo
        return targetHand.transform.GetChild(UnityEngine.Random.Range(0,targetHand.transform.childCount)).gameObject;
    }
}