using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class P1TotalFieldForce : MonoBehaviour
{
    public static int P1ForceValue=0;
    public TextMeshProUGUI totalForce;
    void Start()
    {
        totalForce=GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        totalForce.text=P1ForceValue.ToString();
    }
}
