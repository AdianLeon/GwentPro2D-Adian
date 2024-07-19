using System.Collections.Generic;
using UnityEngine;
//Script para el funcionamiento del deck
public class Deck : StateListener, IContainer
{
    public override int GetPriority => 0;
    private int tradedCardsCount;
    public int GetTradeCount => tradedCardsCount;
    public void OnCardTrade() { tradedCardsCount++; }
    public List<DraggableCard> GetCards => DeckCards;
    public static List<DraggableCard> PlayerDeckCards => GameObject.Find("Deck" + Judge.GetPlayer).GetComponent<Deck>().GetCards;
    public static List<DraggableCard> EnemyDeckCards => GameObject.Find("Deck" + Judge.GetEnemy).GetComponent<Deck>().GetCards;
    private List<DraggableCard> DeckCards = new List<DraggableCard>();
    public override void CheckState()
    {
        switch (Judge.CurrentState)
        {
            case State.SettingUpGame://Cuando inicia el juego se reinicia el contador de las cartas intercambiadas con el deck
                tradedCardsCount = 0;//y se reparten 10 cartas a cada jugador de las cartas seleccionadas por ese jugador
                ReceiveAndDealCards();
                break;
            case State.EndingRound://Cuando se empieza una ronda se reparten dos cartas a cada jugador
                DrawTopCard();
                DrawTopCard();
                break;
            case State.EndingGame://Cuando se acaba el juego se limpia la lista
                DeckCards.Clear();
                break;
        }
    }
    private void ReceiveAndDealCards()
    {
        //Anadiendo las cartas del contenedor del jugador al deck
        GameObject.Find("Cards" + gameObject.Field()).CardsInside<DraggableCard>().ForEach(card => DeckCards.Add(card));
        ShuffleDeck(DeckCards);//Barajeando el deck
        for (int i = 0; i < 10; i++) { DrawTopCard(); }//Repartiendo 10 cartas
    }
    public DraggableCard DrawTopCard()
    {//Roba una carta del deck sin importar el espacio en la mano
        if (DeckCards.Count > 0)
        {//Si quedan cartas en el deck, creamos la carta y la ponemos en la mano
            DraggableCard newCard = Instantiate(DeckCards[DeckCards.Count - 1].gameObject, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<DraggableCard>();//Se instancia una carta de esa escogida
            newCard.MoveCardTo(GameObject.Find("Hand" + gameObject.Field()));//Se pone en la mano

            DeckCards.RemoveAt(DeckCards.Count - 1);//Se quita de la lista
            return newCard;
        }
        UserRead.Write("Se ha intentado robar una carta del deck, pero ya no quedan cartas!");
        return null;
    }
    public void AddCardRandomly(DraggableCard newCard)
    {//Anade una carta en una posicion random del deck
        DeckCards.Add(newCard);
        SwapCardToRandomPosition(DeckCards.Count - 1);
    }
    public void ShuffleDeck(List<DraggableCard> DeckCards)
    {//Barajea el deck
        for (int i = 0; i < DeckCards.Count; i++)
        {//Cada carta se inserta en una posicion aleatoria
            SwapCardToRandomPosition(i);
        }
    }
    private void SwapCardToRandomPosition(int posOfCard)
    {//Cambia la carta en esa posicion con otra random
        if (posOfCard >= DeckCards.Count || posOfCard < 0) { throw new System.Exception("Indice de posOfCard no valido"); }
        int randomPos = UnityEngine.Random.Range(0, DeckCards.Count);
        DraggableCard aux = DeckCards[posOfCard];
        DeckCards[posOfCard] = DeckCards[randomPos];
        DeckCards[randomPos] = aux;
    }
}