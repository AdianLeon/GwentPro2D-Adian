using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script para el efecto de la carta lider
public class LeaderEffect : MonoBehaviour
{
    public bool used;//Array de booleanos para determinar si se ha usado o no la habilidad de lider
        void Start(){
        used=false;
    }
    public void LeaderP1(){//Habilidad de lider, intento de robo
        if(TurnManager.PlayerTurn==1){
            LeaderSkillConditions("Hand","EnemyHand","P1");
        }else{
            RoundPoints.URWrite("Ese no es el tuyo, tu Gru es el de arriba");
        }
    }
    public void LeaderP2(){//Habilidad de lider, intento de robo
        if(TurnManager.PlayerTurn==2){
            LeaderSkillConditions("EnemyHand","Hand","P2");
        }else{
            RoundPoints.URWrite("Ese no es el tuyo, tu Gru es el de abajo");
        }
    }
    public void LeaderSkillConditions(string hand,string enemyHand,string thisPlayer){
        if(!used && TurnManager.CardsPlayed==0){//Si la habilidad no ha sido usada aun en la partida
            if(GameObject.Find(hand).transform.childCount<9 && TurnManager.CardsPlayed==0){//Solo si las cartas de la mano son 8 o menos y es la primera carta que se juega 
                if(GameObject.Find(enemyHand).transform.childCount<2){//Si el enemigo tiene dos cartas o menos
                    if(GameObject.Find(enemyHand).transform.childCount==1)//Si tiene una
                        RoundPoints.URWrite("Los minions intentaron robar dos cartas de la mano enemiga, pero solo tenia una y les dio lastima, se la regresaron y le pidieron disculpas");
                    if(GameObject.Find(enemyHand).transform.childCount==0)//Si no tiene
                        RoundPoints.URWrite("Los minions intentaron robar dos cartas de la mano enemiga, pero no tenia ni una y se rieron de el, pero volvieron con las manos vacias");
                    
                    TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                    used=true;
                    DeckTrade.firstAction=false;//Ya no se puede intercambiar cartas con el deck propio si es el primer turno de la partida
                }else{
                    LeaderSkill(hand,enemyHand,thisPlayer);//Activa la habilidad de lider para el jugador correspondiente
                    TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                    DeckTrade.firstAction=false;//Ya no se puede intercambiar cartas con el deck propio si es el primer turno de la partida
                }
            }else if(GameObject.Find(hand).transform.childCount>8){//En el caso en que es la primera carta que se juega pero no hay espacio en la mano
                RoundPoints.URWrite("Los minions robaron dos cartas pero no tienes suficiente espacio en la mano asi que las devolvieron y le pidieron disculpas al enemigo");
                TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                used=true;
                DeckTrade.firstAction=false;//Ya no se puede intercambiar cartas si es el primer turno de la partida
            }
        }else if(used && TurnManager.CardsPlayed==0){//Si la habilidad ha sido usada
            RoundPoints.URWrite("Gru solo puede ordenarle a los minions que roben dos cartas enemigas una vez por partida");
        }
    }
    public void LeaderSkill(string hand,string enemyHand,string thisPlayer){//Se pasa un string que identifica que jugador esta llamando la funcion
        if(!used){//Si no se ha usado la habilidad en la partida
            int r=Random.Range(0,4);
            if(r==0){//Se roba 2 cartas al enemigo
                StealFrom(hand,enemyHand,thisPlayer);
                StealFrom(hand,enemyHand,thisPlayer);
                RoundPoints.URWrite("Los minions han robado dos cartas de la mano enemiga");
            }else if(r==1 || r==2){//Se roba una carta de la mano enemiga
                StealFrom(hand,enemyHand,thisPlayer);
                RoundPoints.URWrite("Los minions han intentado robar dos cartas al enemigo, pero ha logrado recuperar una");
            }else{//No pasa nada
                RoundPoints.URWrite("Los minions intentaron robar dos cartas al enemigo, pero salio mal y recupero ambas");
            }
        }
        used=true;//Usamos la habilidad asi que no la usaremos mas
    }
    public void StealFrom(string hand,string enemyHand,string thisPlayer){//Robar de un jugador
        Card.fields stealerField;
        if(thisPlayer=="P1"){//En dependencia de que argumento se le pasa a la funcion se roba de un jugador u otro
            stealerField=Card.fields.P1;
        }else{
            stealerField=Card.fields.P2;
        }
            GameObject stolenCard=GameObject.Find(enemyHand).transform.GetChild(Random.Range(0,GameObject.Find(enemyHand).transform.childCount)).gameObject;//Escoge una carta aleatoria de la mano
            stolenCard.transform.SetParent(GameObject.Find(hand).transform);//Pone la carta robada en la mano del ladron
            stolenCard.GetComponent<Card>().whichField=stealerField;//Cambia el campo de la carta
    }
}
