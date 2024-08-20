using UnityEngine;
//Script para el efecto de robar una carta del deck
public class DrawOneEffect : MonoBehaviour, ICardEffect
{
    public void TriggerEffect()
    {//Roba una carta del deck propio
        DraggableCard newCard = GameObject.Find("Deck" + gameObject.GetComponent<DraggableCard>().Owner).GetComponent<Deck>().DrawTopCard();
        if (newCard != null) { UserRead.Write("Se ha robado una carta del deck. Es " + newCard.GetComponent<Card>().CardName); }
    }
}
