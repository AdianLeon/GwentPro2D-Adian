using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour
{
    //SCRIPTABLE OBJECTS IN UNITY
    public Card card;
    public static int pow;
    public static bool played;
    public static Card.rank wRank;
    public static Card.fields wField;
    public static Card.quality wQuality;

    public GameObject thisCard;
    void Start()
    {
        thisCard.GetComponent<Image>().sprite=card.artwork;
        pow=card.power;
        played=card.isPlayed;
        Card.rank wRank=card.cRank;
        Card.fields wField=card.cField;
        Card.quality wQuality=card.cQuality;
    }
}
