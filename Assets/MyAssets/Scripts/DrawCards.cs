using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script para el deck
public class DrawCards : MonoBehaviour
{
    //Creando objetos
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;
    public GameObject PlayerArea;
    //Lista de cartas en el deck
    List <GameObject> cards = new List <GameObject>();
    void Start()
    {
        //Anadiendo cartas al deck
        cards.Add(Card1);
        cards.Add(Card2);
        cards.Add(Card3);
        //Reparte 10 cartas de las anadidas al deck
        //for(int i=0;i<10;i++)
        //{
        //    GameObject picked=cards[Random.Range(0,cards.Count)];//La escogida es aleatoria
        //    GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
        //    Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
        //    cards.Remove(picked);//Se quita de la lista
        //}
    }
    //Toma una carta aleatoria y la coloca en la mano del jugador
    public void OnClick()
    {
        if(PlayerArea.transform.childCount<10 && cards.Count!=0)//Solo si el deck no esta vacio y si hay menos de 10 cartas en la mano
        {
            GameObject picked=cards[Random.Range(0,cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            cards.Remove(picked);//Se quita de la lista
        }
    }
}
