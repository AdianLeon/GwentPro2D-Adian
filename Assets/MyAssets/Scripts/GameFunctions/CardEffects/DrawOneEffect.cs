using UnityEngine;
//Script para el efecto de robar una carta del deck
public class DrawOneEffect : MonoBehaviour, ICardEffect
{
    public string GetEffectDescription=>"Cuando esta carta es jugada se roba una carta del deck propio";
    public void TriggerEffect(){//Roba una carta del deck propio
        GameObject newCard=GameObject.Find("Deck"+gameObject.GetComponent<Card>().WhichPlayer).GetComponent<Deck>().DrawTopCard();
        if(newCard!=null){
            UserRead.Write("Se ha robado una carta del deck. Es "+newCard.GetComponent<Card>().CardName);
        }
    }
}
