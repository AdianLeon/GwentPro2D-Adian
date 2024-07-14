using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.PackageManager;

public static class ProcessCard
{
    private static string cardName=null;
    private static string cardFaction=null;
    public static void CompileAndCreate(List<Token> tokenList,int start,int end){
        //CardsToJson.CardSave cardToJson=new CardsToJson.CardSave();
        Dictionary<string,string> cardProperties=new Dictionary<string, string>();

        Debug.Log("Tokens in list:");
        for(int i=start;i<end;i++){
            Debug.Log(tokenList[i].text+"  --  "+tokenList[i].type.ToString()+"  depth: "+tokenList[i].depth);
        }
        for(int i=start;i<end;i++){
            if((tokenList[i].text=="Name" || tokenList[i].text=="Type") && tokenList[i].depth==1){
                tokenList[i].type=TokenTypes.cardAssignment;
            }
            if(tokenList[i].type!=TokenTypes.cardAssignment){
                continue;
            }
            if(tokenList[i+1].text!=":"){
                Errors.Write("Token ':' no encontrado luego de '"+tokenList[i].text+"'",tokenList[i+1]);
                return;
            }
            string value=GetPropertyValue(tokenList,i+2,tokenList[i].text);
            if(value==null){
                Errors.Write("No se pudo procesar la propiedad de carta: '"+tokenList[i].text+"'");
                return;
            }
            cardProperties.Add(tokenList[i].text,value);
            if(tokenList[i].text=="Name"){
                cardName=value;
            }
            if(tokenList[i].text=="Faction"){
                cardFaction=value;
            }
        }
        cardName=null;
        cardFaction=null;
        // Debug.Log("PropertiesDict list:");
        // foreach(string key in propertiesDict.Keys){
        //     Debug.Log("Key: "+key+" Value: "+propertiesDict[key]);
        // }
        int power=0;
        if(cardProperties.ContainsKey("Power")){
            power=int.Parse(cardProperties["Power"]);
        }
        
        CardSave codeCard = new CardSave
        {
            faction = cardProperties["Faction"],
            cardName = cardProperties["Name"],
            effectDescription="Esta es una carta creada",
            powerPoints=power,
            scriptComponent=GetCardComponentFromCode(cardProperties["Type"]),
            zones=GetZonesFromCode(cardProperties["Range"]),
            onActivationName=cardProperties["OnActivation"],
        };
        
        string filePath=Application.dataPath+"/MyAssets/Database/Decks/"+codeCard.faction;
        string cardJsonName="/"+codeCard.cardName+".json";
        CardsToJson.WriteJsonOfCard(codeCard,filePath,cardJsonName);
    }
    private static string GetPropertyValue(List<Token> tokenList,int index,string nameOfKey){
        if(tokenList[index].type==TokenTypes.literal || tokenList[index].type==TokenTypes.number){
            if(tokenList[index+1].text!=","){
                Errors.Write("Esperado token: ','",tokenList[index+1]);
                return null;
            }
            return tokenList[index].text;
        }
        if(nameOfKey=="Range" || nameOfKey=="OnActivation"){
            if(tokenList[index].text!="["){
                Errors.Write("Propiedad "+nameOfKey+" no definida correctamente (falta iniciar con '[')", tokenList[index].line, tokenList[index].col);
                return "";
            }
            int matchPos=DeckCreatorUtils.FindMatchingParenthesis(tokenList,index);
            if(nameOfKey=="Range"){
                string ans="";
                for(int i=index+1;i<matchPos;i++){//Copiamos todos los elementos hasta el parentesis (no incluye los parentesis)
                    ans+=tokenList[i].text;
                }
                return ans;
            }
            if(nameOfKey=="OnActivation"){
                List<Token> onActivationTokens=new List<Token>();
                for(int i=index+1;i<matchPos;i++){
                    onActivationTokens.Add(tokenList[i]);
                }
                onActivationTokens.Add(new Token("$",0,0,TokenTypes.end,0));
                OnActivation onActivation=OnActivationUtils.CreateOnActivation(onActivationTokens);
                //Guardar ese OnActivation creado
                if(cardName==null || cardFaction==null){
                    Errors.Write("Propiedades Name y Faction de la carta debe ser declarada antes que OnActivation",tokenList[index]);
                    return null;
                }
                string address=Application.dataPath+"/MyAssets/Database/CardsOnActivations/";
                string onActivationFileName=cardFaction+"/"+cardName+"OnActivation.json";
                OnActivationUtils.WriteJsonOfOnActivation(onActivation,address,onActivationFileName);
                if(onActivation!=null){
                    return onActivationFileName;
                    //Devolver el nombre del OnActivation.json

                }
                return null;
            }
        }
        return null;
    }
    private static string GetCardComponentFromCode(string w){
        if(w=="Oro"){return "GoldCard";
        }else if(w=="Plata"){return "SilverCard";
        }else if(w=="Clima"){return "WeatherCard";
        }else if(w=="Aumento"){return "BoostCard";
        }else if(w=="Lider"){return "LeaderCard";
        }else if(w=="Senuelo"){return "BaitCard";
        }else if(w=="Despeje"){return "ClearWeatherCard";
        }else{
            Errors.Write("El valor correspondiente a Type no es correcto");
            return "";
        }
    }
    private static string GetZonesFromCode(string w){
        string ans="";
        if(w.Contains("M")){ans+="M";}
        if(w.Contains("R")){ans+="R";}
        if(w.Contains("S")){ans+="S";}
        return ans;
    }
}