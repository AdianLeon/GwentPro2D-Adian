using UnityEngine;
using System.IO;
using Newtonsoft.Json;
//Script para convertir cartas a archivos json
public class CardsToJson : MonoBehaviour
{
    void Awake(){
        Debug.Log("Exporting Cards");
        foreach(Transform card in this.transform){//Toma todas las cartas y las exporta en formato json
            ExportCard(card.gameObject);
        }
    }
    private static void ExportCard(GameObject card){
        //Power
        int powerPoints=0;
        if(card.GetComponent<UnitCard>()!=null){            powerPoints=card.GetComponent<UnitCard>().Power;
        }else if(card.GetComponent<WeatherCard>()!=null){   powerPoints=card.GetComponent<WeatherCard>().Damage;
        }else if(card.GetComponent<BoostCard>()!=null){     powerPoints=card.GetComponent<BoostCard>().Boost;}
        //Zones
        string zones="";
        if(card.GetComponent<UnitCard>()!=null){zones=card.GetComponent<UnitCard>().WhichZone.ToString();}

        CardSave saveCard = new CardSave{
            //Faccion y nombre
            faction = card.GetComponent<Card>().Faction,
            cardName = card.GetComponent<Card>().CardName,
            //Power||Damage||Boost
            powerPoints = powerPoints,
            //Nombre del script carta
            scriptComponent = card.GetComponent<Card>().GetType().Name,
            //Enums zones y quality
            zones = zones,
            //Nombre del json con los efectos
            onActivationName = card.GetComponent<Card>().OnActivationName
        };
        string address=Application.dataPath+"/MyAssets/Database/Decks/"+saveCard.faction+"/";
        string cardJsonName=card.name+".json";
        WriteJsonOfCard(saveCard,address,cardJsonName);
    }
    public static void WriteJsonOfCard(CardSave saveCard,string address,string cardJsonName){//Crea un json de la carta guardada en la direccion
        string jsonStringCard=JsonConvert.SerializeObject(saveCard,Formatting.Indented);
        if(!Directory.Exists(address)){
            Directory.CreateDirectory(address);
        }
        File.WriteAllText(address+cardJsonName,jsonStringCard);
    }
}