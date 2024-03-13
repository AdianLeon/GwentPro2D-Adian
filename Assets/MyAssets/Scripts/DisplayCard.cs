using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour
{
    //SCRIPTABLE OBJECTS IN UNITY
    public Card card;
    public GameObject thisCard;
    void Start()
    {
        thisCard.GetComponent<Image>().sprite=card.artwork;
    }
}
