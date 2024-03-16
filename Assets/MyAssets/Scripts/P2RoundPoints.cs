using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class P2RoundPoints : MonoBehaviour
{
    static int rpoints2;
    public TextMeshProUGUI P2RPoints;
    void Start()
    {
        P2RPoints=GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(rpoints2<PlayerCondition.rPointsP2){
            rpoints2=PlayerCondition.rPointsP2;
            P2RPoints.text=P2RPoints.text+"1";
        }
    }
}
