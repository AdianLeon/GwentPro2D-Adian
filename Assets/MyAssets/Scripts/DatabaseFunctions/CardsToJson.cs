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
        //Sprites
        string sourceImage=card.GetComponent<Card>().faction+"/"+card.GetComponent<Image>().sprite.name;
        string artwork=card.GetComponent<Card>().faction+"/"+card.GetComponent<Card>().artwork.name;
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
        string quality="";
        if(card.GetComponent<UnitCard>()!=null){
            zones=card.GetComponent<UnitCard>().whichZone.ToString();
            quality=card.GetComponent<UnitCard>().whichQuality.ToString();
        }
        CustomClasses.CardSave saveCard=new CustomClasses.CardSave(
            card.GetComponent<Card>().faction,card.GetComponent<Card>().cardRealName,//Faccion y nombre
            card.GetComponent<Card>().description, card.GetComponent<Card>().effectDescription,//Descripcion y descripcion de efecto
            sourceImage, artwork,//Sprites
            card.GetComponent<Card>().cardColor.r, card.GetComponent<Card>().cardColor.g, card.GetComponent<Card>().cardColor.b,//Color
            powerPoints,//Power||Damage||Boost
            GetCardScriptName(card), GetEffectScriptsNames(card),//Nombres de scripts
            zones, quality,//Enums zones y quality
            card.GetComponent<Card>().onActivationCode
        );
        string filePath=Application.dataPath+"/MyAssets/Database/Decks/"+card.GetComponent<Card>().faction;
        string cardJsonName="/"+card.name+".json";
        WriteJsonOfCard(saveCard,filePath,cardJsonName);
    }
    public static void WriteJsonOfCard(CustomClasses.CardSave saveCard,string address,string cardJsonName){//Crea un json de la carta guardada en la direccion
        string jsonStringCard=JsonUtility.ToJson(saveCard,true);
        if(!Directory.Exists(address)){
            Debug.Log("Directorio no encontrado");
            Directory.CreateDirectory(address);
        }
        File.WriteAllText(address+cardJsonName,jsonStringCard);
    }
    private static string GetCardScriptName(GameObject card){//Devuelve el nombre del script Card
        return card.GetComponent<Card>().GetType().Name;
    }
    private static string[] GetEffectScriptsNames(GameObject card){//Devuelve una lista de nombres de los efectos de la carta
        Component[] effectScripts=card.GetComponents(typeof(CardEffect));
        string[] effectScriptsNames=new string[effectScripts.Length];
        for(int i=0;i<effectScripts.Length;i++){
            effectScriptsNames[i]=effectScripts[i].GetType().Name;
        }
        return effectScriptsNames;
    }
}