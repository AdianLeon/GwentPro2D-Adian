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
        if(!used && TurnManager.CardsPlayed==0){//Si la habilidad no ha sido usada aun en la partida y no se ha jugado en el turno
            if(GameObject.Find(enemyHand).transform.childCount<2){//Si el enemigo tiene una o ninguna carta
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
        }else if(used && TurnManager.CardsPlayed==0){//Si la habilidad ha sido usada
            RoundPoints.URWrite("Gru solo puede ordenarle a los minions que roben dos cartas enemigas una vez por partida");
        }
    }
    public void LeaderSkill(string hand,string enemyHand,string thisPlayer){//Se pasa un string que identifica que jugador esta llamando la funcion
        if(!used){//Si no se ha usado la habilidad en la partida
            int r=Random.Range(0,4);
            GameObject cardToSteal=null;
            GameObject cardToSteal1=null;
            GameObject cardToSteal2=null;
            if(r==0){//Se roba 2 cartas al enemigo
                cardToSteal1=GetRandomCard(enemyHand);
                StealFrom(hand,enemyHand,thisPlayer,cardToSteal1);
                cardToSteal2=GetRandomCard(enemyHand);
                StealFrom(hand,enemyHand,thisPlayer,cardToSteal2);
                RoundPoints.URWrite("Los minions han robado dos cartas de la mano enemiga");
            }else if(r==1 || r==2){//Se roba una carta de la mano enemiga
                cardToSteal=GetRandomCard(enemyHand);
                StealFrom(hand,enemyHand,thisPlayer,cardToSteal);
                RoundPoints.URWrite("Los minions han intentado robar dos cartas al enemigo, pero ha logrado recuperar una");
            }else{//No pasa nada
                RoundPoints.URWrite("Los minions intentaron robar dos cartas al enemigo, pero salio mal y recupero ambas");
            }
            if(GameObject.Find(hand).transform.childCount==11){//Se sobrepasa por una carta la capacidad de la mano
                if(r==0){//Si se robaron dos cartas
                    int r2=Random.Range(0,1);//Se decide aleatoriamente cual de las dos se descarta al cementerio
                    if(r2==0){
                        Graveyard.ToGraveyard(cardToSteal1);
                    }else{
                        Graveyard.ToGraveyard(cardToSteal2);
                    }
                    RoundPoints.URWrite("Los minions robaron dos cartas de la mano enemiga, pero quedaba un solo espacio en la mano (una de ellas se envio al cementerio)");
                }else{//Si se robo solo una carta
                    Graveyard.ToGraveyard(cardToSteal);//Se envia al cementerio
                    RoundPoints.URWrite("Los minions robaron una carta de la mano enemiga, pero no habia espacio en la mano (se envio al cementerio)");
                }
            }else if(GameObject.Find(hand).transform.childCount==12){//Si se sobrepasa la capacidad de la mano por dos cartas
                Graveyard.ToGraveyard(cardToSteal1);//Como unico esto puede pasar es robando dos cartas
                Graveyard.ToGraveyard(cardToSteal2);//Asi que ambas son descartadas al cementerio
                RoundPoints.URWrite("Los minions robaron dos cartas de la mano enemiga, pero no habia espacio en la mano (ambas se enviaron al cementerio)");
            }
        }
        used=true;//Usamos la habilidad asi que no la usaremos mas
    }
    public void StealFrom(string hand,string enemyHand,string thisPlayer,GameObject cardToSteal){//Robar de un jugador
        Card.fields stealerField;
        if(thisPlayer=="P1"){//En dependencia de que argumento se le pasa a la funcion se roba de un jugador u otro
            stealerField=Card.fields.P1;
        }else{
            stealerField=Card.fields.P2;
        }
            cardToSteal.transform.SetParent(GameObject.Find(hand).transform);//Pone la carta robada en la mano del ladron
            cardToSteal.GetComponent<Card>().whichField=stealerField;//Cambia el campo de la carta
    }
    public GameObject GetRandomCard(string enemyHand){
        GameObject cardToSteal=GameObject.Find(enemyHand).transform.GetChild(Random.Range(0,GameObject.Find(enemyHand).transform.childCount)).gameObject;//Escoge una carta aleatoria de la mano enemiga
        return cardToSteal;
    }
}
