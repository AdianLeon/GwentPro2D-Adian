using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Effects : MonoBehaviour
{
    public static void CheckForEffect(GameObject card){
        if(card.GetComponent<Card>().hasEffect){
            Effect(card);
        }
    }
    public static void Effect(GameObject card){
        if(card.GetComponent<Dragging>().cardType==Dragging.rank.Aumento){//Carta aumento
            AumEffect(card);
        }

        if(card.GetComponent<Card>().id==0 || card.GetComponent<Card>().id==1 || card.GetComponent<Card>().id==2 || card.GetComponent<Card>().id==3){//Carta clima
            ClimaEffect(card);
        }

        if(card.GetComponent<Card>().id==4 || card.GetComponent<Card>().id==5){//Carta despeje
            DespejeEffect(card);
        }
    }
    public static void AumEffect(GameObject card){
        GameObject target=null;//Objetivo al que anadirle 1 de poder a los hijos
        if(card.transform.parent==GameObject.Find("MyAumentoZoneM").transform){//Si la zona en la que esta la carta es x, afecta a tal zona
            target=GameObject.Find("MyMeleeDropZone");
        }else if(card.transform.parent==GameObject.Find("MyAumentoZoneR").transform){
            target=GameObject.Find("MyRangedDropZone");
        }else if(card.transform.parent==GameObject.Find("MyAumentoZoneS").transform){
            target=GameObject.Find("MySiegeDropZone");
        }else if(card.transform.parent==GameObject.Find("EnemyAumentoZoneM").transform){
            target=GameObject.Find("EnemyMeleeDropZone");
        }else if(card.transform.parent==GameObject.Find("EnemyAumentoZoneR").transform){
            target=GameObject.Find("EnemyRangedDropZone");
        }else if(card.transform.parent==GameObject.Find("EnemyAumentoZoneS").transform){
            target=GameObject.Find("EnemySiegeDropZone");
        }
        if(target!=null){
            for(int i=0;i<target.transform.childCount;i++){
                if(target.transform.GetChild(i).GetComponent<Card>().wQuality!=Card.quality.Gold)     
                    target.transform.GetChild(i).GetComponent<Card>().addedPower++;//Aumenta el poder en 1
            }
            Graveyard.ToGraveyard(card);//Envia la carta usada al cementerio
        }
    }
    public static void ClimaEffect(GameObject card){//Esto es una idea, hay que hacerlo de efecto constante
        GameObject target1=null;//Objetivos a los que quitarle 1 de poder a los hijos
        GameObject target2=null;
        if(card.transform.parent==GameObject.Find("ClimaZoneM").transform){//Si la zona en la que esta la carta es x, afecta a tales zonas
            target1=GameObject.Find("MyMeleeDropZone");
            target2=GameObject.Find("EnemyMeleeDropZone");
        }else if(card.transform.parent==GameObject.Find("ClimaZoneR").transform){
            target1=GameObject.Find("MyRangedDropZone");
            target2=GameObject.Find("EnemyRangedDropZone");
        }else if(card.transform.parent==GameObject.Find("ClimaZoneS").transform){
            target1=GameObject.Find("MySiegeDropZone");
            target2=GameObject.Find("EnemySiegeDropZone");
        }
        if(target1!=null && target2!=null ){
            for(int i=0;i<target1.transform.childCount;i++){//Disminuye en 1 el poder de la fila seleccionada y lo marca
                if(target1.transform.GetChild(i).GetComponent<Card>().affected[card.GetComponent<Card>().id]==false && target1.transform.GetChild(i).GetComponent<Card>().wQuality!=Card.quality.Gold){
                    target1.transform.GetChild(i).GetComponent<Card>().addedPower--;
                    target1.transform.GetChild(i).GetComponent<Card>().affected[card.GetComponent<Card>().id]=true;
                }
            }
            for(int i=0;i<target2.transform.childCount;i++){
                if(target2.transform.GetChild(i).GetComponent<Card>().affected[card.GetComponent<Card>().id]==false && target2.transform.GetChild(i).GetComponent<Card>().wQuality!=Card.quality.Gold){
                    target2.transform.GetChild(i).GetComponent<Card>().addedPower--;
                    target2.transform.GetChild(i).GetComponent<Card>().affected[card.GetComponent<Card>().id]=true;
                }
            }
        }
    }
    public static void UpdateClima(){//Esta funcion se llama cada vez que se juega una nueva carta
        Card[] cardsInSlot=GameObject.Find("ClimaZoneM").GetComponentsInChildren<Card>();//Hace un recorrido por todos los slots de clima
        for(int i=0;i<GameObject.Find("ClimaZoneM").transform.childCount;i++){//Itera por cada una de las cartas del slot
            ClimaEffect(cardsInSlot[i].gameObject);//Hace que pasen por el efecto de clima otra vez, si ya fueron afectadas no se repite
        }
        cardsInSlot=GameObject.Find("ClimaZoneR").GetComponentsInChildren<Card>();
        for(int i=0;i<GameObject.Find("ClimaZoneR").transform.childCount;i++){
            ClimaEffect(cardsInSlot[i].gameObject);
        }
        cardsInSlot=GameObject.Find("ClimaZoneS").GetComponentsInChildren<Card>();
        for(int i=0;i<GameObject.Find("ClimaZoneS").transform.childCount;i++){
            ClimaEffect(cardsInSlot[i].gameObject);
        }
    }
    public static void DespejeEffect(GameObject card){
        GameObject parent=null;
        GameObject target1=null;//Objetivos a los que afectan las cartas de clima
        GameObject target2=null;
        if(card.transform.parent==GameObject.Find("ClimaZoneM").transform){//Si la zona en la que esta la carta es x, afecta a tales zonas
            parent=GameObject.Find("ClimaZoneM");
            target1=GameObject.Find("MyMeleeDropZone");
            target2=GameObject.Find("EnemyMeleeDropZone");
        }else if(card.transform.parent==GameObject.Find("ClimaZoneR").transform){
            parent=GameObject.Find("ClimaZoneR");
            target1=GameObject.Find("MyRangedDropZone");
            target2=GameObject.Find("EnemyRangedDropZone");
        }else if(card.transform.parent==GameObject.Find("ClimaZoneS").transform){
            parent=GameObject.Find("ClimaZoneS");
            target1=GameObject.Find("MySiegeDropZone");
            target2=GameObject.Find("EnemySiegeDropZone");
        }
        if((target1!=null && target2!=null) && parent!=null){
            for(int j=0;j<card.transform.parent.childCount-1;j++){//Deshaciendo el efecto de clima
                for(int i=0;i<target1.transform.childCount;i++){
                    if(target1.transform.GetChild(i).GetComponent<Card>().wQuality!=Card.quality.Gold)
                        target1.transform.GetChild(i).GetComponent<Card>().addedPower++;
                        //Como la carta  de clima solo puede afectar una sola vez por partida no reiniciaremos el array affected a false
                }
                for(int i=0;i<target2.transform.childCount;i++){
                    if(target2.transform.GetChild(i).GetComponent<Card>().wQuality!=Card.quality.Gold)
                        target2.transform.GetChild(i).GetComponent<Card>().addedPower++;
                }
            }
        }
        Card[] cardsInSlot=parent.GetComponentsInChildren<Card>();//Lista de los componenetes Card en el slot que sea
        for(int i=card.transform.parent.childCount-1;i>=0;i--){//Se recorre esa lista de atras hacia adelante para que la ultima en
            Debug.Log(cardsInSlot[i]);//enviarse al cementerio sea la carta despeje
            Graveyard.ToGraveyard(cardsInSlot[i].gameObject);//Mandando las cartas del slot para el cementerio
        }
    }
    public static void SwapObjects(GameObject Card1,GameObject Card2){
        GameObject placehold=new GameObject();//Creamos un objeto auxiliar
        placehold.transform.SetParent(Card1.transform.parent);
        LayoutElement le=placehold.AddComponent<LayoutElement>();//Crea un espacio para saber donde esta el placeholder
        le.preferredWidth=Card1.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight=Card1.GetComponent<LayoutElement>().preferredHeight;
        placehold.transform.SetSiblingIndex(Card1.transform.GetSiblingIndex());

        Card1.transform.SetParent(Card2.transform.parent);
        Card1.transform.SetSiblingIndex(Card2.transform.GetSiblingIndex());

        Card2.transform.SetParent(placehold.transform.parent);
        Card2.transform.SetSiblingIndex(placehold.transform.GetSiblingIndex());

        Destroy(placehold);
    }
    public static void Senuelo(GameObject thisCard){
        GameObject choosed=null;//Guarda el objeto escogido para ser intercambiado con el senuelo
        Dragging.fields whichField=thisCard.GetComponent<Dragging>().whichField;//campo del senuelo
        if(whichField==Dragging.fields.P1){//Si el senuelo es de P1
            for(int i=0;i<TotalFieldForce.P1PlayedCards.Count;i++){//Se busca en las cartas jugadas por P1
                if(CardView.cardName==TotalFieldForce.P1PlayedCards[i].name){//La que coincida en nombre con la ultima carta que se le paso el mouse por encima
                    if(TotalFieldForce.P1PlayedCards[i].GetComponent<Dragging>().cardType!=Dragging.rank.Clima){ //Si ademas no es de clima
                        choosed=TotalFieldForce.P1PlayedCards[i];//La carta es valida para cambiar por el senuelo
                        Effects.SwapObjects(thisCard,choosed);//Se cambian de posicion
                        choosed.GetComponent<Dragging>().isDraggable=true;//La escogida ahora es arrastrable como cualquier otra de la mano
                        TotalFieldForce.P1PlayedCards.Add(thisCard);//Se anade el senuelo a las cartas jugadas de P1
                        TotalFieldForce.P1PlayedCards.Remove(choosed);//Se quita choosed de las cartas jugadas de P1
                        for(int j=0;j<choosed.GetComponent<Card>().affected.Length;j++){//Deshace el efecto de clima cuando la carta vuelve a 
                        //la mano, el senuelo recibira el clima como consecuencia de la llamada de UpdateClima
                            if(choosed.GetComponent<Card>().affected[j]){//Si esta afectado, se deshace
                                choosed.GetComponent<Card>().affected[j]=false;
                                choosed.GetComponent<Card>().addedPower++;
                            }
                        }
                        TurnManager.PlayCard(thisCard);//Se juega el senuelo como cualquier otra carta
                        break;//Se sale del bucle pues ya cambiamos el senuelo por la carta indicada
                    }
                }
            }
        }else if(whichField==Dragging.fields.P2){//Si el senuelo es de P2
            for(int i=0;i<TotalFieldForce.P2PlayedCards.Count;i++){//Se busca en las cartas jugadas por P2
                if(CardView.cardName==TotalFieldForce.P2PlayedCards[i].name){//La que coincida en nombre con la ultima carta que se le paso el mouse por encima
                    if(TotalFieldForce.P2PlayedCards[i].GetComponent<Dragging>().cardType!=Dragging.rank.Clima){ //Si ademas no es de clima
                        choosed=TotalFieldForce.P2PlayedCards[i];//La carta es valida para cambiar por el senuelo
                        Effects.SwapObjects(thisCard,choosed);//Se cambian de posicion
                        choosed.GetComponent<Dragging>().isDraggable=true;//La escogida ahora es arrastrable como cualquier otra de la mano
                        TotalFieldForce.P2PlayedCards.Add(thisCard);//Se anade el senuelo a las cartas jugadas de P2
                        TotalFieldForce.P2PlayedCards.Remove(choosed);//Se quita choosed de las cartas jugadas de P2
                        for(int j=0;j<choosed.GetComponent<Card>().affected.Length;j++){//Deshace el efecto de clima cuando la carta vuelve a 
                        //la mano, el senuelo recibira el clima como consecuencia de la llamada de UpdateClima
                            if(choosed.GetComponent<Card>().affected[j]){//Si esta afectado, se deshace
                                choosed.GetComponent<Card>().affected[j]=false;
                                choosed.GetComponent<Card>().addedPower++;
                            }
                        }                
                        TurnManager.PlayCard(thisCard);//Se juega el senuelo como cualquier otra carta
                        break;//Se sale del bucle pues ya cambiamos el senuelo por la carta indicada
                    }
                }
            }
        }
    }
}
