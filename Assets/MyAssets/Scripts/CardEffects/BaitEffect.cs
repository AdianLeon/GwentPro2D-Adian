using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitEffect : CardEffect
{
    override public void TriggerEffect(){
        GameObject choosed=null;//Guarda el objeto escogido para ser intercambiado con el senuelo
        List<GameObject> FieldList=new List<GameObject>();
        if(this.GetComponent<Dragging>().whichField==Dragging.fields.P1){//Si el senuelo es de P1
            FieldList=TotalFieldForce.P1PlayedCards;
        }else if(this.GetComponent<Dragging>().whichField==Dragging.fields.P2){//Si el senuelo es de P2
            FieldList=TotalFieldForce.P2PlayedCards;
        }
        for(int i=0;i<FieldList.Count;i++){//Se busca en las cartas jugadas
            //La que coincida en nombre con la ultima carta que se le paso el mouse por encima y que no sea de oro
            if(CardView.cardName==FieldList[i].name && FieldList[i].GetComponent<Card>().wQuality!=Card.quality.Gold){
                if(FieldList[i].GetComponent<Dragging>().cardType!=Dragging.rank.Clima){ //Si ademas no es de clima
                    choosed=FieldList[i];//La carta es valida para cambiar por el senuelo
                    SwapWith(choosed);//Se cambian de posicion
                    choosed.GetComponent<Dragging>().isDraggable=true;//La escogida ahora es arrastrable como cualquier otra de la mano
                    FieldList.Add(this.gameObject);//Se anade el senuelo a las cartas jugadas
                    FieldList.Remove(choosed);//Se quita choosed de las cartas jugadas
                    TurnManager.PlayedCards.Remove(choosed);
                    for(int j=0;j<choosed.GetComponent<Card>().affected.Length;j++){//Deshace el efecto de clima cuando la carta vuelve a la mano, el senuelo recibira el clima como consecuencia de la llamada de UpdateClima
                        if(choosed.GetComponent<Card>().affected[j]){//Si esta afectado, se deshace
                            choosed.GetComponent<Card>().affected[j]=false;
                            choosed.GetComponent<Card>().addedPower++;
                        }
                    }
                    TurnManager.PlayCard(this.gameObject);//Se juega el senuelo como cualquier otra carta
                    break;//Se sale del bucle pues ya cambiamos el senuelo por la carta indicada
                }
            }
        }
    }

    private void SwapWith(GameObject choosed){
        GameObject placehold=new GameObject();//Creamos un objeto auxiliar
        placehold.transform.SetParent(this.transform.parent);
        LayoutElement le=placehold.AddComponent<LayoutElement>();//Crea un espacio para saber donde esta el placeholder
        le.preferredWidth=this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight=this.GetComponent<LayoutElement>().preferredHeight;
        placehold.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        this.transform.SetParent(choosed.transform.parent);
        this.transform.SetSiblingIndex(choosed.transform.GetSiblingIndex());

        choosed.transform.SetParent(placehold.transform.parent);
        choosed.transform.SetSiblingIndex(placehold.transform.GetSiblingIndex());

        Destroy(placehold);
    }
}
