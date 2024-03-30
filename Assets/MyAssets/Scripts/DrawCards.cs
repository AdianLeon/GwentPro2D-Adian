using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para el deck
public class DrawCards : MonoBehaviour
{
    public GameObject PlayerArea;
    public List <GameObject> cards = new List <GameObject>();//Lista de cartas
    static int timesStarted=0;
    static bool[] used=new bool[2];
    void Start(){
        timesStarted++;
        if(timesStarted==2){//Este Start es ejecutado por dos decks, por eso se reparten las cartas la segunda vez porque es entonces cuando ambos tienen todas sus cartas
            for(int i=0;i<10;i++){
                GameObject.Find("Deck").GetComponent<Button>().onClick.Invoke();
                GameObject.Find("EnemyDeck").GetComponent<Button>().onClick.Invoke();
            }
        }
    }
    public void OnClickP1()//Toma una carta aleatoria y la coloca en la mano de P1
    {
        DrawCard(GameObject.Find("Hand"),this.gameObject);
        /*if(PlayerArea.transform.childCount<10 && cards.Count!=0)//Solo si el deck no esta vacio y si hay menos de 10 cartas en la mano
        {
            GameObject picked=cards[Random.Range(0,cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            cards.Remove(picked);//Se quita de la lista

        }else if(PlayerArea.transform.childCount>=10 && cards.Count!=0){
            RoundPoints.URWrite("Has robado una carta, pero como tienes la mano llena se ha enviado al cementerio");
            GameObject picked=cards[Random.Range(0,cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano para que tenga el tamano establecido
            Graveyard.ToGraveyard(Card);//Se envia al cementerio
            cards.Remove(picked);//Se quita de la lista
        }*/
    }

    public void OnClickP2()//Toma una carta aleatoria y la coloca en la mano de P2
    {
        DrawCard(GameObject.Find("EnemyHand"),this.gameObject);
        /*if(PlayerArea.transform.childCount<10 && cards.Count!=0)//Solo si el deck no esta vacio y si hay menos de 10 cartas en la mano
        {
            GameObject picked=cards[Random.Range(0,cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            cards.Remove(picked);//Se quita de la lista
        }else if(PlayerArea.transform.childCount>=10 && cards.Count!=0){
            RoundPoints.URWrite("Has robado una carta, pero como tienes la mano llena se ha enviado al cementerio");
            GameObject picked=cards[Random.Range(0,cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano para que tenga el tamano establecido
            Graveyard.ToGraveyard(Card);//Se envia al cementerio
            cards.Remove(picked);//Se quita de la lista
        }*/
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
        if(!used[0] && TurnManager.CardsPlayed==0){//Si la habilidad no ha sido usada aun en la partida
            if(GameObject.Find("Hand").transform.childCount<9 && TurnManager.CardsPlayed==0){//Solo si las cartas de la mano son 8 o menos y es la primera carta que se juega 
                if(GameObject.Find("EnemyHand").transform.childCount<3){//Si el enemigo tiene dos cartas
                    RoundPoints.URWrite("Los minions intentaron robar dos cartas de la mano enemiga, pero como no tenia y les dio lastima las regresaron y le pidieron disculpas");
                    TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                    ExtraDrawCard.firstAction=false;//Ya no se puede intercambiar cartas con el deck propio si es el primer turno de la partida
                }else{
                    LeaderSkill("P1");//Activa la habilidad de lider para P1
                    TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                    ExtraDrawCard.firstAction=false;//Ya no se puede intercambiar cartas con el deck propio si es el primer turno de la partida
                }
            }else if(GameObject.Find("Hand").transform.childCount>8){//En el caso en que es la primera carta que se juega pero no hay espacio en la mano
                RoundPoints.URWrite("Los minions robaron dos cartas pero no tienes suficiente espacio en la mano asi que las devolvieron y le pidieron disculpas al enemigo");
                TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                ExtraDrawCard.firstAction=false;//Ya no se puede intercambiar cartas si es el primer turno de la partida
            }
        }else if(used[0] && TurnManager.CardsPlayed==0){//Si la habilidad ha sido usada
            RoundPoints.URWrite("Gru solo puede ordenarle a los minions que roben dos cartas enemigas una vez por partida");
        }
    }
    public static void LeaderP2(){//Habilidad de lider, intento de robo
        if(!used[1] && TurnManager.CardsPlayed==0){//Si la habilidad no ha sido usada aun en la partida
            if(GameObject.Find("EnemyHand").transform.childCount<9 && TurnManager.CardsPlayed==0){//Solo si las cartas de la mano son 8 o menos y es la primera carta que se juega
                if(GameObject.Find("Hand").transform.childCount<3){
                    RoundPoints.URWrite("Los minions intentaron robar dos cartas de la mano enemiga, pero como no tenia y les dio lastima las regresaron y le pidieron disculpas");
                    TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                    ExtraDrawCard.firstAction=false;//Ya no se puede intercambiar cartas con el deck propio si es el primer turno de la partida
                }else{
                    LeaderSkill("P2");//Activa la habilidad de lider para P2
                    TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                    ExtraDrawCard.firstAction=false;//Ya no se puede intercambiar cartas con el deck propio si es el primer turno de la partida
                }
            }else if(GameObject.Find("EnemyHand").transform.childCount>8){//En el caso en que es la primera carta que se juega pero no hay espacio en la mano
                RoundPoints.URWrite("Los minions robaron dos cartas pero no tienes suficiente espacio en la mano asi que las devolvieron y le pidieron disculpas al enemigo");
                TurnManager.CardsPlayed++;//Esta accion cuenta como carta jugada
                ExtraDrawCard.firstAction=false;//Ya no se puede intercambiar cartas si es el primer turno de la partida
            }
        }else if(used[1] && TurnManager.CardsPlayed==0){//Si la habilidad ha sido usada
            RoundPoints.URWrite("Gru solo puede ordenarle a los minions que roben dos cartas enemigas una vez por partida");
        }
    }
    public static void LeaderSkill(string whichPlayer){//Se pasa un string que identifica que jugador esta llamando la funcion
        string playerToStealFrom;
        int posInUsed;
        if(whichPlayer=="P1"){
            playerToStealFrom="P2";
            posInUsed=0;
        }else{
            playerToStealFrom="P1";
            posInUsed=1;
        }
        if(!used[posInUsed]){
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
        used[posInUsed]=true;
    }
    public static void StealFrom(string playerToStealFrom){
        GameObject stealArea=null;
        GameObject stealerArea=null;
        Dragging.fields stealerField;
        if(playerToStealFrom=="P1"){
            stealArea=GameObject.Find("Hand");
            stealerArea=GameObject.Find("EnemyHand");
            stealerField=Dragging.fields.P2;
        }else{
            stealArea=GameObject.Find("EnemyHand");
            stealerArea=GameObject.Find("Hand");
            stealerField=Dragging.fields.P1;
        }
            GameObject stolenCard=stealArea.transform.GetChild(Random.Range(0,stealArea.transform.childCount)).gameObject;//Escoge una carta aleatoria de la mano
            stolenCard.transform.SetParent(stealerArea.transform);//Pone la carta robada en la mano del ladron
            stolenCard.GetComponent<Dragging>().whichField=stealerField;//Cambia el campo de la carta
    }
}
