using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para el funcionamiento del deck y la habilidad de lider
public class DrawCards : MonoBehaviour
{
    public GameObject PlayerArea;
    public List <GameObject> cards = new List <GameObject>();//Lista de cartas
    static int timesStarted=0;
    static bool[] used=new bool[2];//Array de booleanos para determinar si se ha usado o no la habilidad de lider
    void Start(){
        timesStarted++;
        if(timesStarted%2==0){//Este Start es ejecutado por dos decks, por eso se reparten las cartas la segunda vez
            for(int i=0;i<10;i++){
                GameObject.Find("Deck").GetComponent<Button>().onClick.Invoke();
                GameObject.Find("EnemyDeck").GetComponent<Button>().onClick.Invoke();
            }
            used[0]=false;
            used[1]=false;
        }
    }
    public void OnClickP1()//Toma una carta aleatoria y la coloca en la mano de P1
    {
        DrawCard(GameObject.Find("Hand"),this.gameObject);
    }

    public void OnClickP2()//Toma una carta aleatoria y la coloca en la mano de P2
    {
        DrawCard(GameObject.Find("EnemyHand"),this.gameObject);
    }
    public static void DrawCard(GameObject PlayerArea,GameObject PlayerDeck){
        if(PlayerArea.transform.childCount<10 && PlayerDeck.GetComponent<DrawCards>().cards.Count!=0){//Solo si el deck no esta vacio y si hay menos de 10 cartas en la mano
            GameObject picked=PlayerDeck.GetComponent<DrawCards>().cards[Random.Range(0,PlayerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            PlayerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista

        }else if(PlayerArea.transform.childCount>=10 && PlayerDeck.GetComponent<DrawCards>().cards.Count!=0){
            RoundPoints.URWrite("Has robado una carta, pero como tienes la mano llena se ha enviado al cementerio");
            GameObject picked=PlayerDeck.GetComponent<DrawCards>().cards[Random.Range(0,PlayerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano para que tenga el tamano establecido
            Graveyard.ToGraveyard(Card);//Se envia al cementerio
            PlayerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista
        }
    }
    public static void LeaderP1(){//Habilidad de lider, intento de robo
        if(TurnManager.PlayerTurn==1){
            if(!used[0] && TurnManager.CardsPlayed==0){//Si la habilidad no ha sido usada aun en la partida
                if(GameObject.Find("Hand").transform.childCount<9 && TurnManager.CardsPlayed==0){//Solo si las cartas de la mano son 8 o menos y es la primera carta que se juega 
                    if(GameObject.Find("EnemyHand").transform.childCount<2){//Si el enemigo tiene dos cartas o menos
                        if(GameObject.Find("EnemyHand").transform.childCount==1)//Si tiene una
                            RoundPoints.URWrite("Los minions intentaron robar dos cartas de la mano enemiga, pero solo tenia una y les dio lastima, se la regresaron y le pidieron disculpas");
                        if(GameObject.Find("EnemyHand").transform.childCount==0)//Si no tiene
                            RoundPoints.URWrite("Los minions intentaron robar dos cartas de la mano enemiga, pero no tenia ni una y se rieron de el, pero volvieron con las manos vacias");
                        
                        TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                        used[0]=true;
                        DeckTrade.firstAction=false;//Ya no se puede intercambiar cartas con el deck propio si es el primer turno de la partida
                    }else{
                        LeaderSkill("P1");//Activa la habilidad de lider para P1
                        TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                        DeckTrade.firstAction=false;//Ya no se puede intercambiar cartas con el deck propio si es el primer turno de la partida
                    }
                }else if(GameObject.Find("Hand").transform.childCount>8){//En el caso en que es la primera carta que se juega pero no hay espacio en la mano
                    RoundPoints.URWrite("Los minions robaron dos cartas pero no tienes suficiente espacio en la mano asi que las devolvieron y le pidieron disculpas al enemigo");
                    TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                    used[0]=true;
                    DeckTrade.firstAction=false;//Ya no se puede intercambiar cartas si es el primer turno de la partida
                }
            }else if(used[0] && TurnManager.CardsPlayed==0){//Si la habilidad ha sido usada
                RoundPoints.URWrite("Gru solo puede ordenarle a los minions que roben dos cartas enemigas una vez por partida");
            }
        }else{
            RoundPoints.URWrite("Ese no es el tuyo, tu Gru es el de arriba");
        }
    }
    public static void LeaderP2(){//Habilidad de lider, intento de robo
        if(TurnManager.PlayerTurn==2){
            if(!used[1] && TurnManager.CardsPlayed==0){//Si la habilidad no ha sido usada aun en la partida
                if(GameObject.Find("EnemyHand").transform.childCount<9 && TurnManager.CardsPlayed==0){//Solo si las cartas de la mano son 8 o menos y es la primera carta que se juega
                    if(GameObject.Find("Hand").transform.childCount<2){//Si el enemigo tiene dos cartas o menos
                        if(GameObject.Find("Hand").transform.childCount==1)//Si tiene una
                            RoundPoints.URWrite("Los minions intentaron robar dos cartas de la mano enemiga, pero solo tenia una y les dio lastima, se la regresaron y le pidieron disculpas");
                        if(GameObject.Find("Hand").transform.childCount==0)//Si no tiene
                            RoundPoints.URWrite("Los minions intentaron robar dos cartas de la mano enemiga, pero no tenia ni una y se rieron de el, pero volvieron con las manos vacias");
                        
                        TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                        used[1]=true;
                        DeckTrade.firstAction=false;//Ya no se puede intercambiar cartas con el deck propio si es el primer turno de la partida
                    }else{
                        LeaderSkill("P2");//Activa la habilidad de lider para P2
                        TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                        DeckTrade.firstAction=false;//Ya no se puede intercambiar cartas con el deck propio si es el primer turno de la partida
                    }
                }else if(GameObject.Find("EnemyHand").transform.childCount>8){//En el caso en que es la primera carta que se juega pero no hay espacio en la mano
                    RoundPoints.URWrite("Los minions robaron dos cartas pero no tienes suficiente espacio en la mano asi que las devolvieron y le pidieron disculpas al enemigo");
                    TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                    used[1]=true;
                    DeckTrade.firstAction=false;//Ya no se puede intercambiar cartas si es el primer turno de la partida
                }
            }else if(used[1] && TurnManager.CardsPlayed==0){//Si la habilidad ha sido usada
                RoundPoints.URWrite("Gru solo puede ordenarle a los minions que roben dos cartas enemigas una vez por partida");
            }
        }else{
            RoundPoints.URWrite("Ese no es el tuyo, tu Gru es el de abajo");
        }
    }
    public static void LeaderSkill(string whichPlayer){//Se pasa un string que identifica que jugador esta llamando la funcion
        string playerToStealFrom;
        int posInUsed;
        if(whichPlayer=="P1"){//Determinando a quien robar
            playerToStealFrom="P2";
            posInUsed=0;
        }else{
            playerToStealFrom="P1";
            posInUsed=1;
        }
        if(!used[posInUsed]){//Si no hemos usado la habilidad en la partida
            int r=Random.Range(0,4);
            if(r==0){//Se roba 2 cartas al enemigo
                StealFrom(playerToStealFrom);
                StealFrom(playerToStealFrom);
                RoundPoints.URWrite("Los minions han robado dos cartas de la mano enemiga");
            }else if(r==1 || r==2){//Se roba una carta de la mano enemiga
                StealFrom(playerToStealFrom);
                RoundPoints.URWrite("Los minions han intentado robar dos cartas al enemigo, pero ha logrado recuperar una");
            }else{//No pasa nada
                RoundPoints.URWrite("Los minions intentaron robar dos cartas al enemigo, pero salio mal y recupero ambas");
            }
        }
        used[posInUsed]=true;//Usamos la habilidad asi que no la usaremos mas
    }
    public static void StealFrom(string playerToStealFrom){//Robar de Px
        GameObject stealArea=null;
        GameObject stealerArea=null;
        Card.fields stealerField;
        if(playerToStealFrom=="P1"){//En dependencia de que argumento se le pasa a la funcion se roba de un jugador u otro
            stealArea=GameObject.Find("Hand");
            stealerArea=GameObject.Find("EnemyHand");
            stealerField=Card.fields.P2;
        }else{
            stealArea=GameObject.Find("EnemyHand");
            stealerArea=GameObject.Find("Hand");
            stealerField=Card.fields.P1;
        }
            GameObject stolenCard=stealArea.transform.GetChild(Random.Range(0,stealArea.transform.childCount)).gameObject;//Escoge una carta aleatoria de la mano
            stolenCard.transform.SetParent(stealerArea.transform);//Pone la carta robada en la mano del ladron
            stolenCard.GetComponent<Card>().whichField=stealerField;//Cambia el campo de la carta
    }
}
