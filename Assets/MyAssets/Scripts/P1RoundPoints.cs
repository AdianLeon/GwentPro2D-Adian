using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class P1RoundPoints : MonoBehaviour
{
    static int rpoints;
    public TextMeshProUGUI P1RPoints;
    void Start()
    {
        P1RPoints=GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(rpoints<PlayerCondition.rPointsP1){
            rpoints=PlayerCondition.rPointsP1;
            P1RPoints.text=P1RPoints.text+"1";
        }
    }
}
