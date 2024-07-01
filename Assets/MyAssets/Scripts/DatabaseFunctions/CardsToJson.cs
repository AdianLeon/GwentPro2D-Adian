using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
//Script para convertir cartas a archivos json
public class CardsToJson : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Exporting Cards");
        this.ExportAllCards();//Para hacer jsons de todas las cartas en el objeto CardsToExport
    }
    private void ExportAllCards(){//Toma todas las cartas y las exporta en formato json
        for(int i=0;i<this.transform.childCount;i++){
            ExportCard(this.transform.GetChild(i).gameObject);
        }
    }
    private static void ExportCard(GameObject card){
        //Power
        int powerPoints=0;
        if(card.GetComponent<UnitCard>()!=null){
            powerPoints=card.GetComponent<UnitCard>().power;
        }else if(card.GetComponent<WeatherCard>()!=null){
            powerPoints=card.GetComponent<WeatherCard>().damage;
        }else if(card.GetComponent<BoostCard>()!=null){
            powerPoints=card.GetComponent<BoostCard>().boost;
        }
        //Zones && Quality
        string zones="";
        if(card.GetComponent<UnitCard>()!=null){
            zones=card.GetComponent<UnitCard>().whichZone.ToString();
        }
        CardSave saveCard = new CardSave
        {
            //Faccion y nombre
            faction = card.GetComponent<Card>().faction,
            cardRealName = card.GetComponent<Card>().cardRealName,
            //Descripcion y descripcion de efecto
            description = card.GetComponent<Card>().description,
            effectDescription = card.GetComponent<Card>().effectDescription,
            //Power||Damage||Boost
            powerPoints = powerPoints,
            //Nombres de scripts
            typeComponent = card.GetComponent<Card>().GetType().Name,//Componente carta
            effectComponents = GetEffectScriptsNames(card),//Componentes efecto
            //Enums zones y quality
            zones = zones,
            //Nombre del json con los efectos
            onActivationCodeName = card.GetComponent<Card>().OnActivationCode
        };
        string filePath=Application.dataPath+"/MyAssets/Database/Decks/"+card.GetComponent<Card>().faction;
        string cardJsonName="/"+card.name+".json";
        WriteJsonOfCard(saveCard,filePath,cardJsonName);
    }
    public static void WriteJsonOfCard(CardSave saveCard,string address,string cardJsonName){//Crea un json de la carta guardada en la direccion
        string jsonStringCard=JsonUtility.ToJson(saveCard,true);
        if(!Directory.Exists(address)){
            Directory.CreateDirectory(address);
        }
        File.WriteAllText(address+cardJsonName,jsonStringCard);
    }
    private static string[] GetEffectScriptsNames(GameObject card){//Devuelve una lista de nombres de los efectos de la carta
        Component[] effectScripts=card.GetComponents(typeof(IEffect));
        string[] effectScriptsNames=new string[effectScripts.Length];
        for(int i=0;i<effectScripts.Length;i++){
            effectScriptsNames[i]=effectScripts[i].GetType().Name;
        }
        return effectScriptsNames;
    }
}