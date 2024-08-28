using System.Collections.Generic;
using UnityEngine;
//Script para el funcionamiento del deck
public class Deck : MonoBehaviour, IStateSubscriber, IContainer
{
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (State.SettingUpGame, new Execution(stateInfo => ReceiveAndDealCards(), 0)),
        new (State.EndingRound, new Execution(stateInfo => { DrawTopCard(); DrawTopCard(); }, 0) ),
        new (State.EndingGame, new Execution(stateInfo => DeckCards.Clear(), 0))
    };
    public IEnumerable<DraggableCard> GetCards => DeckCards;
    public static IEnumerable<DraggableCard> PlayerCards => GameObject.Find("Deck" + Judge.GetPlayer).GetComponent<Deck>().GetCards;
    public static IEnumerable<DraggableCard> EnemyCards => GameObject.Find("Deck" + Judge.GetEnemy).GetComponent<Deck>().GetCards;
    private List<DraggableCard> DeckCards = new List<DraggableCard>();
    private void ReceiveAndDealCards()
    {
        //Anadiendo las cartas del contenedor del jugador al deck
        GameObject.Find("Cards" + gameObject.Field()).CardsInside<DraggableCard>().ForEach(card => DeckCards.Add(card));
        ShuffleDeck();//Barajeando el deck
        for (int i = 0; i < 10; i++) { DrawTopCard(); }//Repartiendo 10 cartas
    }
    public DraggableCard DrawTopCard()
    {//Roba una carta del deck sin importar el espacio en la mano
        if (DeckCards.Count == 0) { UserRead.Write("Se ha intentado robar una carta del deck, pero ya no quedan cartas!"); return null; }
        //Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
        DraggableCard newCard = Instantiate(DeckCards[DeckCards.Count - 1].gameObject, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<DraggableCard>();
        newCard.OnActivation = DeckCards[DeckCards.Count - 1].OnActivation;//Apartado para actualizar el OnActivation porque por alguna razon cuando se instancia la carta la referencia a su OnActivation se pierde
        newCard.GetComponent<CanvasGroup>().blocksRaycasts = true;//Aseguramos que la carta siempre bloquee los raycasts para que podamos interactuar con ella
        newCard.MoveCardTo(GameObject.Find("Hand" + gameObject.Field()));//Se pone en la mano
        DeckCards.RemoveAt(DeckCards.Count - 1);//Se quita de la lista
        return newCard;
    }
    public void AddCardRandomly(DraggableCard newCard)
    {//Anade una carta en una posicion random del deck
        DeckCards.Add(newCard);
        SwapCardToRandomPosition(DeckCards.Count - 1);
    }
    public void ShuffleDeck()
    {//Barajea el deck insertando cada carta en una posicion aleatoria
        for (int i = 0; i < DeckCards.Count; i++) { SwapCardToRandomPosition(i); }
    }
    private void SwapCardToRandomPosition(int posOfCard)
    {//Cambia la carta en esa posicion con otra random cambiando las referencias en la lista
        int randomPos = UnityEngine.Random.Range(0, DeckCards.Count);
        (DeckCards[posOfCard], DeckCards[randomPos]) = (DeckCards[randomPos], DeckCards[posOfCard]);
    }
}