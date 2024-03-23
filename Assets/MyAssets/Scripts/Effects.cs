using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void CheckForEffect(GameObject card){
        if(card.GetComponent<Card>().hasEffect){
            Effect(card);
        }
    }
    public static void Effect(GameObject card){
        Debug.Log("ACTIVO MICARTATRAMPAA");
        if(card.GetComponent<Card>().id==9 || card.GetComponent<Card>().id==10){
            Debug.Log("Efecto de aumento");
        }

        //id 11 y 12 son los senuelos

        if(card.GetComponent<Card>().id==13 || card.GetComponent<Card>().id==14){
            Debug.Log("Efecto de clima");
        }

        //id 15 y 16 son los despejes
    }
}
