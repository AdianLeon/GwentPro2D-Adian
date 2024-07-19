using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Script para la funcionalidad de los cementerios
public class Graveyard : StateListener, IContainer
{
    public override int GetPriority => 2;
    public List<DraggableCard> GetCards => gameObject.CardsInside<DraggableCard>();
    public static List<DraggableCard> PlayerGraveyardCards => GameObject.Find("Graveyard" + Judge.GetPlayer).CardsInside<DraggableCard>();//Lista de las cartas en el cementerio del jugador en turno
    public static List<DraggableCard> EnemyGraveyardCards => GameObject.Find("Graveyard" + Judge.GetEnemy).CardsInside<DraggableCard>();//Lista de las cartas en el cementerio del enemigo del jugador en turno
    private int deadCount { get => this.transform.childCount; }//Contador de carta en este cementerio
    public override void CheckState()
    {
        switch (Judge.CurrentState)
        {
            case State.EndingGame://Al final del juego se deshace de todas las cartas en el cementerio enviandolas a la basura
                GetCards.Disappear();
                break;
        }
        switch (Judge.CurrentState)
        {//Luego vuelve a chequear el estado
            case State.SettingUpGame:
            case State.PlayingCard:
            case State.EndingTurn:
            case State.EndingRound:
            case State.EndingGame:
                GameObject.Find("GText" + gameObject.Field()).GetComponent<TextMeshProUGUI>().text = deadCount.ToString();//Cuenta y actualiza cuantas cartas hay en el cementerio
                break;
        }
    }
    public static void SendToGraveyard(List<DraggableCard> cards)
    {//Envia a todas las cartas de la lista al cementerio
        cards.ForEach(card => SendToGraveyard(card));
    }
    public static void SendToGraveyard(DraggableCard card)
    {//Envia la carta al cementerio correspondiente
        GameObject.Find("Graveyard" + card.WhichPlayer).GetComponent<Graveyard>().ToGraveyard(card);
    }
    private void ToGraveyard(DraggableCard card)
    {//Envia la carta a este cementerio
        //Se limpia la lista de cartas de clima si es afectable, se resetea el poder anadido si es de poder y se mueve para el cementerio
        card.GetComponent<IAffectable>()?.WeathersAffecting.Clear();
        if (card.GetComponent<PowerCard>() != null) { card.GetComponent<PowerCard>().AddedPower = 0; }
        card.GetComponent<DraggableCard>().MoveCardTo(GameObject.Find("Graveyard" + card.GetComponent<Card>().WhichPlayer));
    }
}
