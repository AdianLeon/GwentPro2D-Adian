using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public static void CheckForEffect(GameObject card){
        if(card.GetComponent<Card>().hasEffect){
            Effect(card);
        }
    }
    public static void Effect(GameObject card){
        Debug.Log("ACTIVO MI CARTA TRAMPA");
        if(card.GetComponent<Card>().id==8 || card.GetComponent<Card>().id==9 || card.GetComponent<Card>().id==10){//Carta aumento
            AumEffect(card);
        }

        //id 11 y 12 son los senuelos

        if(card.GetComponent<Card>().id==13 || card.GetComponent<Card>().id==14){//Carta clima
            ClimaEffect(card);
        }

        if(card.GetComponent<Card>().id==15){
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
        if(target1!=null && target2!=null){
            for(int i=0;i<target1.transform.childCount;i++){//Disminuye en 1 el poder de la fila seleccionada
                        target1.transform.GetChild(i).GetComponent<Card>().addedPower--;
            }
            for(int i=0;i<target2.transform.childCount;i++){
                        target2.transform.GetChild(i).GetComponent<Card>().addedPower--;
            }
        }
    }
    public static void DespejeEffect(GameObject card){
        GameObject parent=null;
        GameObject target1=null;//Objetivos a los que quitarle 1 de poder a los hijos
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
                    target1.transform.GetChild(i).GetComponent<Card>().addedPower++;
                }
                for(int i=0;i<target2.transform.childCount;i++){
                    target2.transform.GetChild(i).GetComponent<Card>().addedPower++;
                }
            }
        }
        Card[] cardsInSlot=parent.GetComponentsInChildren<Card>();//Lista de las cartas en el slot
        Debug.Log("Before: "+cardsInSlot.Length+" y "+card.transform.parent.childCount);
        for(int i=card.transform.parent.childCount-1;i>=0;i--){
            Debug.Log("Iteracion#: "+i);
            Debug.Log(cardsInSlot[i]);
            Graveyard.ToGraveyard(cardsInSlot[i].gameObject);//Mandando las cartas del slot para el cementerio
        }
        Debug.Log("After: "+cardsInSlot.Length+" y "+card.transform.parent.childCount);
    }
}
