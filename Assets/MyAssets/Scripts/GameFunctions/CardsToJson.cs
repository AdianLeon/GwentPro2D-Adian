using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using Newtonsoft.Json;
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
            faction = card.GetComponent<Card>().Faction,
            cardName = card.GetComponent<Card>().CardName,
            //Descripcion y descripcion de efecto
            description = card.GetComponent<Card>().Description,
            effectDescription = card.GetComponent<Card>().EffectDescription,
            //Power||Damage||Boost
            powerPoints = powerPoints,
            //Nombres de scripts
            scriptComponent = card.GetComponent<Card>().GetType().Name,
            //Enums zones y quality
            zones = zones,
            //Nombre del json con los efectos
            onActivationName = card.GetComponent<Card>().OnActivationName
        };
        string filePath=Application.dataPath+"/MyAssets/Database/Decks/"+card.GetComponent<Card>().Faction;
        string cardJsonName="/"+card.name+".json";
        WriteJsonOfCard(saveCard,filePath,cardJsonName);
    }
    public static void WriteJsonOfCard(CardSave saveCard,string address,string cardJsonName){//Crea un json de la carta guardada en la direccion
        string jsonStringCard=JsonConvert.SerializeObject(saveCard,Formatting.Indented);
        if(!Directory.Exists(address)){
            Directory.CreateDirectory(address);
        }
        File.WriteAllText(address+cardJsonName,jsonStringCard);
    }
}