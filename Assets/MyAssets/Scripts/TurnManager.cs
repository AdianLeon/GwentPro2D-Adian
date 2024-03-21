using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static int PlayerTurn;//Turno de jugador
    public static int CardsPlayed;//Cant de cartas jugadas en el turno
    static int countPass;//Conteo de pases sin cartas jugadas
    public static List<GameObject> PlayedCards=new List<GameObject>();//Lista de las cartas jugadas

    void Start()
    {
        countPass=0;
        PlayerTurn=1;
        CardsPlayed=0;
    }

    public static void EndTurn(){//Se llama con cada pass
        if(CardsPlayed==0){//Cuenta cada vez que se acaba el turno sin jugar cartas
            countPass++;
        }
        if(countPass>1){//A las dos veces que se acaba el turno sin jugar cartas se avanza de ronda
            NextRound();
            Debug.Log("Se acaba de empezar otra ronda");
        }else{
            if(PlayerTurn==1){
               PlayerTurn=2;
                CardsPlayed=0;
            }else{
                PlayerTurn=1;
                CardsPlayed=0;
            }
        }
    }
    public static void PlayCard(GameObject card){//Juega la carta y anade la carta a la lista de cartas jugadas
            CardsPlayed++;
            PlayedCards.Add(card);
            card.GetComponent<Card>().isPlayed=true;
    }

    public static void NextRound(){//Proxima ronda
    Debug.Log("Turno de: P"+PlayerTurn);
        Graveyard.AllToGraveyard();//Manda todas las cartas al cementerio
        
        if(TotalFieldForce.P1ForceValue>TotalFieldForce.P2ForceValue){//Si P1 tiene mas poder que P2
        Debug.Log("P1>P2");
            if(PlayerTurn==2){//P1 comienza el proximo turno
                PlayerTurn=1;
                ClickPassB();
            }else{
                ClickEnemyPassB();
            }

            PlayerCondition.Wins("P1");//P1 gana la ronda y obtiene un punto de ronda
        }
        else if(TotalFieldForce.P2ForceValue>TotalFieldForce.P1ForceValue){//Si P2 tiene mas poder que P1
        Debug.Log("P2>P1");
            if(PlayerTurn==1){//P2 comienza el proximo turno
                PlayerTurn=2;
                ClickEnemyPassB();
            }else{
                ClickPassB();
            }

            PlayerCondition.Wins("P2");//P2 gana la ronda y obtiene un punto de ronda
        }
        else{//Si ambos tienen igual poder ambos ganan 1 punto y la ronda continua sin afectarse
        Debug.Log("P1==P2");
            PlayerCondition.rPointsP1++;
            PlayerCondition.rPointsP2++;
        }
        CardsPlayed=0;
        countPass=0;
        TotalFieldForce.P1ForceValue=0;
        TotalFieldForce.P2ForceValue=0;
    }
    public static void ClickPassB(){//Clickea el PassButton
        if(GameObject.Find("PassButton")!=null)
            GameObject.Find("PassButton").GetComponent<Button>().onClick.Invoke();

        Debug.Log("Presiona el boton: PassButton");
    }
    public static void ClickEnemyPassB(){//Clickea el PassButton
        if(GameObject.Find("EnemyPassButton")!=null)
            GameObject.Find("EnemyPassButton").GetComponent<Button>().onClick.Invoke();

        Debug.Log("Presiona el boton: EnemyPassButton");
    }
}
