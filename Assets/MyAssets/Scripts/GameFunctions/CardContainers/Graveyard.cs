using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para la funcionalidad de los cementerios
public class Graveyard : StateListener, IContainer
{
    public override int GetPriority=>2;
    public List<GameObject> GetCards=>gameObject.CardsInside();
    public static List<GameObject> PlayerGraveyardCards=>GameObject.Find("Graveyard"+Judge.GetPlayer).GetComponent<Graveyard>().GetCards;//Lista de las cartas en el cementerio del jugador en turno
    public static List<GameObject> EnemyGraveyardCards=>GameObject.Find("Graveyard"+Judge.GetEnemy).GetComponent<Graveyard>().GetCards;//Lista de las cartas en el cementerio del enemigo del jugador en turno
    private int deadCount{get=>this.transform.childCount;}//Contador de carta en este cementerio
    public override void CheckState(){
        switch(Judge.CurrentState){
            case State.EndingGame://Al final del juego se deshace de todas las cartas en el cementerio enviandolas a la basura
                GetCards.Disappear();
                break;
        }
        switch(Judge.CurrentState){//Luego vuelve a chequear el estado
            case State.SettingUpGame:
            case State.PlayingCard:
            case State.EndingTurn:
            case State.EndingRound:
            case State.EndingGame:
                GameObject.Find("GText"+gameObject.Field()).GetComponent<TextMeshProUGUI>().text=deadCount.ToString();//Cuenta y actualiza cuantas cartas hay en el cementerio
                break;
        }
    }
    public static void SendToGraveyard(List<GameObject> souls){//Envia a todas las cartas de la lista al cementerio
        foreach(GameObject soul in souls){
            SendToGraveyard(soul);
        }
    }
    public static void SendToGraveyard(GameObject soul){//Envia la carta al cementerio correspondiente
        GameObject.Find("Graveyard"+soul.GetComponent<Card>().WhichPlayer).GetComponent<Graveyard>().ToGraveyard(soul);
    }
    private void ToGraveyard(GameObject card){//Envia la carta a este cementerio
        card.GetComponent<IAffectable>()?.AffectedByWeathers.Clear();
        if(card.GetComponent<PowerCard>()!=null){
            card.GetComponent<PowerCard>().AddedPower=0;
        }
        card.GetComponent<DraggableCard>().DropCardOnZone(GameObject.Find("Graveyard"+card.GetComponent<Card>().WhichPlayer));
    }
}
