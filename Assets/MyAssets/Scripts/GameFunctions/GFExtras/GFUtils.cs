using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class GFUtils : MonoBehaviour
{
    public static UserRead UserRead{get=>GameObject.Find("UserRead").GetComponent<UserRead>();}
    public static List<GameObject> GetCardsIn(GameObject place){//Devuelve los descendientes carta de un GameObject
        if(place.GetComponent<Card>()!=null){//Si el lugar es una carta se devuelve esa carta
            return new List<GameObject>(){place};
        }
        List<GameObject> cards=new List<GameObject>();//Considerando que no es una carta comenzamos a anadir sus hijos
        for(int i=0;i<place.transform.childCount;i++){//Si no tiene hijos este for no se ejecuta y se devolvera una lista vacia
            cards.AddRange(GetCardsIn(place.transform.GetChild(i).gameObject));//Anade a su lista lo que sus hijos devuelvan
        }
        return cards;//Devuelve todas las cartas que contiene
    }
    public static Player GetField(string name){//Devuelve el campo de un objeto teniendo su nombre
        return (Player)Enum.Parse(typeof(Player),"P"+name[name.Length-1]);
    }//Funcionara siempre que siga la convencion de nombrar los objetos con P+(identificador)
    public static void GetRidOf(List<GameObject> cards){
        foreach(GameObject card in cards){
            Dragging.GetRidOf(card);
        }
    }
    public static void OnStateChange(){
        Debug.Log("State changed to "+Judge.CurrentState);
        StateListener[] scripts=Resources.FindObjectsOfTypeAll<StateListener>();
        foreach(StateListener script in scripts){script.CheckState();}
    }
}