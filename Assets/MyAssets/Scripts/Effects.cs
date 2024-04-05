using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Script que contiene los efectos de las cartas y algunos efectos visuales
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
        }else if(card.GetComponent<Card>().id==0 || card.GetComponent<Card>().id==1 || card.GetComponent<Card>().id==2 || card.GetComponent<Card>().id==3){//Carta clima
            ClimaEffect(card);
        }else if(card.GetComponent<Card>().id==4 || card.GetComponent<Card>().id==5){//Carta despeje
            DespejeEffect(card);
        }else if(card.GetComponent<Card>().id==6){//El macho
            MostPowerEffect(card);
        }else if(card.GetComponent<Card>().id==7){//Vector
            LessPowerEffect(card);
        }else if(card.GetComponent<Card>().id==8){//Scarlet Overkill
            DrawCardEffect(card);
        }else{
            Debug.Log(card+" tiene hasEffect activado pero no tiene efecto");
        }
    }
    public static void ZonesGlow(GameObject thisCard){//Encuentra las zonas del mismo tipo y campo que la carta y las ilumina
        DropZone[] zones=GameObject.FindObjectsOfType<DropZone>();
        for(int i=0;i<zones.Length;i++){
            bool generalCase=(zones[i].GetComponent<DropZone>().cardType==thisCard.GetComponent<Dragging>().cardType) && (zones[i].GetComponent<DropZone>().whichField==thisCard.GetComponent<Dragging>().whichField);//Caso general es cualquier carta que no sea de clima, debe coincidir en tipo y campo
            bool climaCase=(Dragging.rank.Clima==thisCard.GetComponent<Dragging>().cardType) && (Dragging.rank.Clima==zones[i].GetComponent<DropZone>().cardType);//Caso clima es que tanto la carta como la zona sean tipo clima
            bool usualCase=(generalCase || climaCase) && TurnManager.CardsPlayed==0;//El caso usual es cuando solo se puede jugar una carta y esta carta puede ser de caso general o clima
            bool afterPassCase=(generalCase || climaCase) && TurnManager.lastTurn;//El caso afterPass es cuando un jugador pasa y ahora el otro puede jugar tantas cartas como quiera de caso general o clima
            if(afterPassCase || usualCase){//Para cualquiera de los dos casos usual o afterPass iluminaremos la(s) zona(s) donde el jugador puede poner la carta
                zones[i].GetComponent<Image>().color=new Color (1,1,1,0.1f);
            }
        }

        ExtraDrawCard.UpdateRedraw();//Se actualiza si se puede intercambiar cartas con el deck
        if(ExtraDrawCard.redrawable){//Si se puede
            if(thisCard.GetComponent<Dragging>().whichField==Dragging.fields.P1){//La carta es de P1
                GameObject.Find("UntouchableMyDeck").GetComponent<Image>().color=new Color (0,1,0,0.1f);//El deck de P1 "brilla"
            }else if(thisCard.GetComponent<Dragging>().whichField==Dragging.fields.P2){//La carta es de P2
                GameObject.Find("UntouchableEnemyDeck").GetComponent<Image>().color=new Color (0,1,0,0.1f);//El deck de P2 "brilla"
            }
        }
    }
    public static void OffZonesGlow(){
        DropZone[] zones=GameObject.FindObjectsOfType<DropZone>();
        for(int i=0;i<zones.Length;i++){
            zones[i].GetComponent<Image>().color=new Color (1,1,1,0);
        }
        GameObject.Find("UntouchableMyDeck").GetComponent<Image>().color=new Color (1,1,1,0);
        GameObject.Find("UntouchableEnemyDeck").GetComponent<Image>().color=new Color (1,1,1,0);
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
    public static void ClimaEffect(GameObject card){
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
                //La que coincida en nombre con la ultima carta que se le paso el mouse por encima y que no sea de oro
                if(CardView.cardName==TotalFieldForce.P1PlayedCards[i].name && TotalFieldForce.P1PlayedCards[i].GetComponent<Card>().wQuality!=Card.quality.Gold){
                    if(TotalFieldForce.P1PlayedCards[i].GetComponent<Dragging>().cardType!=Dragging.rank.Clima){ //Si ademas no es de clima
                        choosed=TotalFieldForce.P1PlayedCards[i];//La carta es valida para cambiar por el senuelo
                        Effects.SwapObjects(thisCard,choosed);//Se cambian de posicion
                        choosed.GetComponent<Dragging>().isDraggable=true;//La escogida ahora es arrastrable como cualquier otra de la mano
                        TotalFieldForce.P1PlayedCards.Add(thisCard);//Se anade el senuelo a las cartas jugadas de P1
                        TotalFieldForce.P1PlayedCards.Remove(choosed);//Se quita choosed de las cartas jugadas de P1
                        TurnManager.PlayedCards.Remove(choosed);
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
                }else{
                    RoundPoints.URWrite(TotalFieldForce.P1PlayedCards[i].GetComponent<Card>().cardRealName+" es una carta heroe, no es afectada por ningun efecto de carta especial");
                }
            }
        }else if(whichField==Dragging.fields.P2){//Si el senuelo es de P2
            for(int i=0;i<TotalFieldForce.P2PlayedCards.Count;i++){//Se busca en las cartas jugadas por P2
                //La que coincida en nombre con la ultima carta que se le paso el mouse por encima y que no sea de oro
                if(CardView.cardName==TotalFieldForce.P2PlayedCards[i].name && TotalFieldForce.P2PlayedCards[i].GetComponent<Card>().wQuality!=Card.quality.Gold){
                    if(TotalFieldForce.P2PlayedCards[i].GetComponent<Dragging>().cardType!=Dragging.rank.Clima){ //Si ademas no es de clima
                        choosed=TotalFieldForce.P2PlayedCards[i];//La carta es valida para cambiar por el senuelo
                        Effects.SwapObjects(thisCard,choosed);//Se cambian de posicion
                        choosed.GetComponent<Dragging>().isDraggable=true;//La escogida ahora es arrastrable como cualquier otra de la mano
                        TotalFieldForce.P2PlayedCards.Add(thisCard);//Se anade el senuelo a las cartas jugadas de P2
                        TotalFieldForce.P2PlayedCards.Remove(choosed);//Se quita choosed de las cartas jugadas de P2
                        TurnManager.PlayedCards.Remove(choosed);
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
    public static void MostPowerEffect(GameObject card){//Elimina la carta con mas poder del campo
        TurnManager.PlayedCards.Remove(card);
        if(TurnManager.PlayedCards.Count!=0){
            GameObject Card=TurnManager.PlayedCards[TurnManager.PlayedCards.Count-1];
            int maxTotalPower=Card.GetComponent<Card>().power+Card.GetComponent<Card>().addedPower;
            for(int i=0;i<TurnManager.PlayedCards.Count-1;i++){
                if(TurnManager.PlayedCards[i].GetComponent<Card>().power+TurnManager.PlayedCards[i].GetComponent<Card>().addedPower>maxTotalPower){
                    Card=TurnManager.PlayedCards[i];
                    maxTotalPower=Card.GetComponent<Card>().power+Card.GetComponent<Card>().addedPower;
                }
            }
            Graveyard.ToGraveyard(Card);
            TotalFieldForce.UpdateForce();
        }
        TurnManager.PlayedCards.Add(card);
    }
    public static void LessPowerEffect(GameObject card){//Elimina la carta con menos poder del campo enemigo
        //Determinando el campo a afectar
        List <GameObject> targetField=new List <GameObject>();//Una lista del campo enemigo
        if(card.GetComponent<Dragging>().whichField==Dragging.fields.P1){//Si el Vector jugado es de P1
            targetField=TotalFieldForce.P2PlayedCards;//El campo P2 es el enemigo
        }else if(card.GetComponent<Dragging>().whichField==Dragging.fields.P2){//Si el Vector jugado es de P2
            targetField=TotalFieldForce.P1PlayedCards;//El campo P1 es el enemigo
        }

        //Hallando la carta de menor poder y eliminandola
        if(targetField.Count!=0){//Si la lista del campo enemigo tiene elementos
            GameObject Card=targetField[targetField.Count-1];//Se empieza a comparar por la ultima carta
            //Si todas las cartas tienen el mismo poder la carta eliminada es la ultima jugada
            int minTotalPower=Card.GetComponent<Card>().power+Card.GetComponent<Card>().addedPower;//Poder total minimo
            for(int i=0;i<targetField.Count-1;i++){//Se recorre la lista excepto el ultimo elemento pues ya lo consideramos
                if(targetField[i].GetComponent<Card>().power+targetField[i].GetComponent<Card>().addedPower<minTotalPower){//Si el poder total de la carta es menor que el que tenemos guardada
                    Card=targetField[i];//Esta sera nuestra nueva carta de menor poder
                    minTotalPower=Card.GetComponent<Card>().power+Card.GetComponent<Card>().addedPower;//Este sera nuestro nuevo menor poder
                }
            }
            Graveyard.ToGraveyard(Card);//Se envia al cementerio la carta resultante(la de menor poder)
            TotalFieldForce.UpdateForce();
        }
    }
    public static void DrawCardEffect(GameObject card){
        GameObject PlayerArea=null;
        GameObject PlayerDeck=null;
        if(card.GetComponent<Dragging>().whichField==Dragging.fields.P1){
            PlayerArea=GameObject.Find("Hand");
            PlayerDeck=GameObject.Find("Deck");
        }else if(card.GetComponent<Dragging>().whichField==Dragging.fields.P2){
            PlayerArea=GameObject.Find("EnemyHand");
            PlayerDeck=GameObject.Find("EnemyDeck");
        }
        if(PlayerArea!=null && PlayerDeck!=null){
            GameObject picked=PlayerDeck.GetComponent<DrawCards>().cards[Random.Range(0,PlayerDeck.GetComponent<DrawCards>().cards.Count)];//La escogida es aleatoria
            GameObject Card = Instantiate(picked,new Vector3(0,0,0),Quaternion.identity);//Se instancia un objeto de esa escogida
            Card.transform.SetParent(PlayerArea.transform,false);//Se pone en la mano
            PlayerDeck.GetComponent<DrawCards>().cards.Remove(picked);//Se quita de la lista
        }
    }
}
