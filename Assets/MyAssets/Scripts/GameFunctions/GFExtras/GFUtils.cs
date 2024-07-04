using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GFUtils : MonoBehaviour
{
    public static List<GameObject> GetCardsIn(List<GameObject> places){//Devuelve una lista de los hijos de una lista de GameObjects
        List<GameObject> childrens=new List<GameObject>();//Empezamos a contar sus hijos
        foreach(GameObject place in places){//Por cada lugar que esta en la lista
            childrens.AddRange(GetCardsIn(place));//Obtenemos las cartas de ese lugar y lo anadimos a la lista
        }
        return childrens;//Devolvemos la lista
    }
    public static List<GameObject> GetCardsIn(GameObject place){//Devuelve los hijos de un GameObject
        if(place.GetComponent<Card>()!=null){//Si el lugar es una carta se devuelve esa carta pues no puede contener otra carta
            return new List<GameObject>(){place};
        }
        if(place.transform.childCount==0){//Si no es una carta y no tiene hijos pues la lista se devuelve vacia
            return new List<GameObject>();
        }
        List<GameObject> childrens=new List<GameObject>();//Considerando que no es una carta  y que no esta vacio comenzamos a anadir sus hijos
        for(int i=0;i<place.transform.childCount;i++){
            childrens.Add(place.transform.GetChild(i).gameObject);
        }
        return GetCardsIn(childrens);//Analizamos todos sus hijos exactamente como antes
    }
    public static Fields GetField(string name){//Devuelve el campo de un objeto teniendo su nombre
        return (Fields)Enum.Parse(typeof(Fields),"P"+name[name.Length-1]);
    }//Funcionara siempre que siga la convencion de nombrar los objetos con P+(identificador)
}
