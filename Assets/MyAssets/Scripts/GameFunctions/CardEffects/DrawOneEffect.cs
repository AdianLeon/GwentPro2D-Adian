using UnityEngine;
//Script para el efecto de robar una carta del deck
public class DrawOneEffect : MonoBehaviour, ICardEffect
{
    public void TriggerEffect(){//Roba una carta del deck propio
        GameObject newCard=GameObject.Find("Deck"+this.gameObject.GetComponent<Card>().WhichPlayer).GetComponent<Deck>().DrawTopCard();
        if(newCard!=null){
            GFUtils.UserRead.Write("Se ha robado una carta del deck. Es "+newCard.GetComponent<Card>().CardName);
        }
    }
}
