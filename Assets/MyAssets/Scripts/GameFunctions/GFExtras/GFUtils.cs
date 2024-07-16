using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public static class GameObjectExtensionMethods{
    public static List<GameObject> CardsInside(this GameObject place){//Devuelve los descendientes carta de un GameObject
        if(place.GetComponent<Card>()!=null){//Si el lugar es una carta se devuelve esa carta
            return new List<GameObject>(){place};
        }
        List<GameObject> cards=new List<GameObject>();//Considerando que no es una carta comenzamos a anadir sus hijos
        foreach(Transform card in place.transform){//Si no tiene hijos este for no se ejecuta y se devolvera una lista vacia
            cards.AddRange(card.gameObject.CardsInside());//Anade a su lista lo que sus hijos devuelvan
        }
        return cards;//Devuelve todas las cartas que contiene
    }
    public static Player Field(this GameObject place){//Devuelve el campo de un objeto
        return (Player)Enum.Parse(typeof(Player),"P"+place.name[place.name.Length-1]);//Funcionara siempre que se siga la convencion de nombrar los objetos con P+(identificador)
    }
    public static void Disappear(this IEnumerable<GameObject> cards){//Se deshace una por una de las cartas en el IEnumerable
        foreach(GameObject card in cards){
            Disappear(card);
        }
    }
    public static void Disappear(this GameObject card){//Se deshace de la carta mandandola a la basura
        card.GetComponent<DraggableCard>().DropCardOnZone(GameObject.Find("Trash"));
    }
}
public static class GFUtils
{
    public static void GlowOff(){//Desactiva toda la iluminacion de las zonas y las cartas jugadas
        foreach(DropZone zone in GameObject.FindObjectsOfType<DropZone>()){//Hace las zonas invisibles nuevamente
            zone.OffGlow();
        }
        foreach(GameObject playedCard in Field.AllPlayedCards){//Las cartas se dessombrean
            playedCard.GetComponent<Card>().OnGlow();
        }
    }
}