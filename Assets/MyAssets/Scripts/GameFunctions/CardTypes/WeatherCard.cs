using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
//Script para las cartas clima
public class WeatherCard : Card, IShowZone, ICardEffect
{
    public int damage;//Cant de poder restado cuando una carta es afectada por el clima
    public override Color GetCardViewColor(){return new Color(0.7f,0.2f,0.2f);}
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[C]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="-"+damage;
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,1);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
    }
    public void ShowZone(){
        DZWeather[] zones=GameObject.FindObjectsOfType<DZWeather>();//Se crea un array de todas las zonas de clima
        foreach(DZWeather zone in zones){
            zone.OnGlow();
        }
    }
    public void TriggerEffect(){//Efecto de las cartas clima
        //Afecta a las zonas
        AffectZoneWithWeather(this.transform.parent.GetComponent<DZWeather>().Target1);//La de P1
        AffectZoneWithWeather(this.transform.parent.GetComponent<DZWeather>().Target2);//La de P2
    }
    private void AffectZoneWithWeather(GameObject zoneTarget){//Afecta la zona determinada con el efecto clima
        GameObject card;
        for(int i=0;i<zoneTarget.transform.childCount;i++){//Itera por todos los hijos
            card=zoneTarget.transform.GetChild(i).gameObject;
            //Si la carta en esa zona no es senuelo, o es unidad pero no de oro
            if(card.GetComponent<IAffectable>()!=null){
                if(!card.GetComponent<IAffectable>().AffectedByWeathers.Contains(this.gameObject.name)){//Si no han sido afectados
                    //Si la carta es senuelo o si la carta no es heroe
                    card.GetComponent<IAffectable>().AffectedByWeathers.Add(this.gameObject.name);
                    if(card.GetComponent<CardWithPower>()!=null){
                        card.GetComponent<CardWithPower>().AddedPower-=damage;
                    }
                }
            }
        }
    }
    public static void UpdateWeather(){//Esta funcion se llama cada vez que se juega una nueva carta
        RectivateWeathersInZone("WeatherZoneM");//Actualiza el clima por zonas
        RectivateWeathersInZone("WeatherZoneR");
        RectivateWeathersInZone("WeatherZoneS");
    }
    private static void RectivateWeathersInZone(string zoneToUpdate){//Reactiva los efectos de las cartas clima en una zona especifica
        WeatherCard[] cardsInZone=GameObject.Find(zoneToUpdate).GetComponentsInChildren<WeatherCard>();//Se acceden a todos los hijos de esa zona
        foreach(WeatherCard cardInZone in cardsInZone){//Itera por cada uno de esos hijos
            cardInZone.TriggerEffect();//Hace que activen el efecto de clima otra vez
        }
    }
}
