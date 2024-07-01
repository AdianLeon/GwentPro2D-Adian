using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProcessCard : MonoBehaviour
{
    public class SaveInstruction{
        public string key;
        public string value;
    }
    public static void CompileAndCreate(List<Token> tokenList,int start,int end){
        //CardsToJson.CardSave cardToJson=new CardsToJson.CardSave();
        Dictionary<string,string> propertiesDict=new();

        Debug.Log("Tokens in list:");
        for(int i=start;i<end;i++){
            Debug.Log(tokenList[i].text+"  --  "+tokenList[i].type.ToString()+"  depth: "+tokenList[i].depth);
        }
        for(int i=start;i<end;i++){
            if(tokenList[i].type==tokenTypes.cardAssignment && tokenList[i+1].text==":"){
                propertiesDict.Add(tokenList[i].text,GetInstructionValue(tokenList,i+2,tokenList[i].text));
                if(propertiesDict[tokenList[i].text]==""){
                    CheckErrors.ErrorWrite("Valor no asignado a "+tokenList[i].text+" en linea: "+tokenList[i].line+" columna: "+tokenList[i].col,"CompileAndCreate");
                }
            }
        }
        Debug.Log("PropertiesDict list:");
        foreach(string key in propertiesDict.Keys){
            Debug.Log("Key: "+key+" Value: "+propertiesDict[key]);
        }
        int power=int.Parse(propertiesDict["Power"]);

        CardSave codeCard = new CardSave
        {
            faction = propertiesDict["Faction"],
            cardName = propertiesDict["Name"],
            description="Esta es una carta creada",
            effectDescription="Esta es una carta creada",
            powerPoints=power,
            scriptComponents=new string[]{GetCardComponentFromCode(propertiesDict["Type"])},
            zones=GetZonesFromCode(propertiesDict["Range"]),
            onActivationCodeName=propertiesDict["OnActivation"],
        };
        
        string filePath=Application.dataPath+"/MyAssets/Database/Decks/"+codeCard.faction;
        string cardJsonName="/"+codeCard.cardName+".json";
        CardsToJson.WriteJsonOfCard(codeCard,filePath,cardJsonName);
    }
    private static string GetInstructionValue(List<Token> tokenList,int index,string nameOfKey){
        if(tokenList[index].type==tokenTypes.literal){
            return tokenList[index].text;
        }else if(tokenList[index].type==tokenTypes.number){
            return tokenList[index].text;
        }else{
            if(nameOfKey=="Range" || nameOfKey=="OnActivation"){
                if(tokenList[index].text=="["){
                    string ans="";
                    int matchPos=Utils.FindMatchingParenthesis(tokenList,index);
                    for(int i=index;i<=matchPos;i++){//Copiamos todos los elementos hasta el parentesis (incluyendo el parentesis)
                        ans+=tokenList[i].text;
                    }
                    return ans;
                }else{
                    CheckErrors.ErrorWrite("Propiedad "+nameOfKey+" no definida correctamente (falta iniciar con '[') linea: "+tokenList[index].line+" columna: "+tokenList[index].col,"GetInstructionValue");
                    return "";
                }
            }else{
                return "";
            }
        }
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
            CheckErrors.ErrorWrite("El valor correspondiente a Type no es correcto","ProcessCard");
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