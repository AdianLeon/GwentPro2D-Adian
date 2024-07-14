using System;
using UnityEngine;
//Script para el efecto de la carta lider
public class GruEffect : MonoBehaviour, ICardEffect
{
    private static GameObject GetHand{get=>GameObject.Find("Hand"+Judge.GetPlayer);}
    private static GameObject GetEnemyHand{get=>GameObject.Find("Hand"+Judge.GetEnemy);}
    private static bool StealRandomEnemyCard(){//Robar del enemigo una carta random
        if(GetEnemyHand.transform.childCount==0){return false;}//Si no tiene cartas devuelve false
        GameObject cardToSteal=GetEnemyHand.transform.GetChild(UnityEngine.Random.Range(0,GetEnemyHand.transform.childCount)).gameObject;//Carta random de la mano objetivo
        cardToSteal.transform.SetParent(GetHand.transform);//Pone la carta robada en la mano del ladron
        cardToSteal.GetComponent<Card>().WhichPlayer=Judge.GetPlayer;//Cambia el campo de la carta al del jugador que desencadeno el efecto
        return true;
    }
    public void TriggerEffect(){
        int r=UnityEngine.Random.Range(0,4);
        switch (r){
            case 0://Se roba 2 cartas al enemigo
                if(!StealRandomEnemyCard()){
                    GFUtils.UserRead.Write("Los minions han intentado robar dos cartas al enemigo, pero no tenia ninguna");return;
                }
                if(!StealRandomEnemyCard()){
                    GFUtils.UserRead.Write("Los minions han intentado robar dos cartas al enemigo, pero solo tenia una");return;
                }
                GFUtils.UserRead.Write("Los minions han robado dos cartas de la mano enemiga exitosamente");return;
            case 1://Se roba 1 carta al enemigo
            case 2:
                if(!StealRandomEnemyCard()){
                    GFUtils.UserRead.Write("Los minions han intentado robar dos cartas al enemigo, pero no tenia ninguna");return;
                }
                GFUtils.UserRead.Write("Los minions han intentado robar dos cartas al enemigo, pero solo consiguieron robar una");return;
            case 3://Fallo
                GFUtils.UserRead.Write("Los minions intentaron robar dos cartas al enemigo, pero no robaron ninguna");return;
        }
    }
}