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
    public static void CompileAndCreate(List<CustomClasses.Token> tokenList,int start,int end){
        //CardsToJson.CardSave cardToJson=new CardsToJson.CardSave();
        Dictionary<string,string> propertiesDict=new();
        List<CustomClasses.Token> onActivationTokens=new List<CustomClasses.Token>();

        Debug.Log("Tokens in list:");
        for(int i=start;i<end;i++){
            Debug.Log(tokenList[i].text+"  --  "+tokenList[i].type.ToString()+"  depth: "+tokenList[i].depth);
        }
        for(int i=start;i<end;i++){
            if(tokenList[i].type==CustomClasses.Token.tokenTypes.cardAssignment && tokenList[i+1].text==":"){
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
        
        CustomClasses.CardSave codeCard=new CustomClasses.CardSave(
            propertiesDict["Faction"],
            propertiesDict["Name"],
            "Esta es una carta creada",
            "Esta es una carta creada",
            propertiesDict["Faction"]+"/"+propertiesDict["Name"]+"Image",propertiesDict["Faction"]+"/"+propertiesDict["Name"],
            GetRandomColor(),GetRandomColor(),GetRandomColor(),
            power,
            GetCardComponentFromCode(propertiesDict["Type"]),
            null,
            GetZonesFromCode(propertiesDict["Range"]),
            GetQualityFromCode(propertiesDict["Type"]),
            propertiesDict["OnActivation"]
        );
        string filePath=Application.dataPath+"/MyAssets/Database/Decks/"+propertiesDict["Faction"];
        string cardJsonName="/"+propertiesDict["Name"]+".json";
        CardsToJson.WriteJsonOfCard(codeCard,filePath,cardJsonName);
    }
    private static string GetInstructionValue(List<CustomClasses.Token> tokenList,int index,string nameOfKey){
        if(tokenList[index].type==CustomClasses.Token.tokenTypes.literal){
            return tokenList[index].text;
        }else if(tokenList[index].type==CustomClasses.Token.tokenTypes.number){
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
        if(w=="Oro" || w=="Plata"){return "UnitCard";
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
    private static string GetQualityFromCode(string type){
        if(type=="Oro"){return "Gold";
        }else if(type=="Plata"){return "Silver";
        }else{return "";}
    }
    private static string GetZonesFromCode(string w){
        string ans="";
        if(w.Contains("M")){ans+="M";}
        if(w.Contains("R")){ans+="R";}
        if(w.Contains("S")){ans+="S";}
        return ans;
    }
    private static float GetRandomColor(){//Devuelve un float random entre 0.2f y 1
        System.Random random=new System.Random();//Instanciamos un objeto de la clase Random
        float rndmfloat=(float)random.NextDouble();//Obtenemos un float random entre 0 y 1
        return rndmfloat*0.8f+0.2f;//Multiplicamos por 0.8f (reduce el float entre 0 y 0.8) y luego sumamos 0.2f (aumenta esta escala y obtenemos entre 0.2f y 1)
    }
}