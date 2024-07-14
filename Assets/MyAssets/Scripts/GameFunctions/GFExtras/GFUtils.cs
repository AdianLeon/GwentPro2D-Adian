using System;
using System.Collections.Generic;
using UnityEngine;
public class GFUtils : MonoBehaviour
{
    public static UserRead UserRead{get=>GameObject.Find("UserRead").GetComponent<UserRead>();}//Comodidad para escribir en el UserRead
    public static List<GameObject> GetCardsIn(GameObject place){//Devuelve los descendientes carta de un GameObject
        if(place.GetComponent<Card>()!=null){//Si el lugar es una carta se devuelve esa carta
            return new List<GameObject>(){place};
        }
        List<GameObject> cards=new List<GameObject>();//Considerando que no es una carta comenzamos a anadir sus hijos
        foreach(Transform card in place.transform){//Si no tiene hijos este for no se ejecuta y se devolvera una lista vacia
            cards.AddRange(GetCardsIn(card.gameObject));//Anade a su lista lo que sus hijos devuelvan
        }
        return cards;//Devuelve todas las cartas que contiene
    }
    public static Player GetField(string name){//Devuelve el campo de un objeto teniendo su nombre
        return (Player)Enum.Parse(typeof(Player),"P"+name[name.Length-1]);
    }//Funcionara siempre que siga la convencion de nombrar los objetos con P+(identificador)
    public static void GetRidOf(List<GameObject> cards){//Se deshace una por una de las cartas pasadas en la lista
        foreach(GameObject card in cards){
            GetRidOf(card);
        }
    }
    public static void GetRidOf(GameObject card){
        card.GetComponent<Dragging>().DropCardOnZone(GameObject.Find("Trash"));
    }
    public static void GlowOff(){//Desactiva toda la iluminacion de las zonas y las cartas jugadas
        foreach(DropZone zone in GameObject.FindObjectsOfType<DropZone>()){//Hace las zonas invisibles nuevamente
            zone.OffGlow();
        }
        foreach(GameObject playedCard in Field.AllPlayedCards){//Las cartas se dessombrean
            playedCard.GetComponent<Card>().OnGlow();
        }
    }
}