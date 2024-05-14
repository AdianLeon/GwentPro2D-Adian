using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiplyEffect : CardEffect
{
    public int originalPower;
    void Start(){
        originalPower=this.GetComponent<UnitCard>().power;
    }
    override public void TriggerEffect(){//Multiplica por n su ataque, siendo n la cantidad de cartas iguales a ella en el campo.
        int n=1;//Contador de cuantas cartas del mismo tipo hay, contando la propia carta
        n+=CountSelfIn(TotalFieldForce.P1PlayedCards);//Se cuenta en el campo de P1 
        n+=CountSelfIn(TotalFieldForce.P2PlayedCards);//Se cuenta en el campo de P2
        this.GetComponent<UnitCard>().power=originalPower*n;//Se iguala el poder de la carta jugada a n veces su propio poder
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text=this.GetComponent<UnitCard>().power.ToString();//Actualizamos el poder en el CardView
    }
    private int CountSelfIn(List<GameObject> PlayedCardsList){//Devuelve el conteo de cuantas veces se encuentra la carta en la lista
        int n=0;
        for(int i=0;i<PlayedCardsList.Count;i++){
            if(PlayedCardsList[i].GetComponent<Card>().cardRealName==this.GetComponent<Card>().cardRealName){
                //Si se encuentra una carta igual, se cuenta
                n++;
            }
        }
        return n;
    }
}
