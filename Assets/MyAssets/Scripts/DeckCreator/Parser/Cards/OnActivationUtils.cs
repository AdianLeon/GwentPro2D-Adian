using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
public static class OnActivationUtils{
    public static OnActivation CreateOnActivation(List<Token> t){//Procesa una lista de tokens y los convierte en un OnActivation
        OnActivation onActivation=new OnActivation();
        return ParseOnActivation(onActivation,t,0);
    }
    public static OnActivation ParseOnActivation(OnActivation onActivation,List<Token> t,int index){
        if(t[index].text=="$"){return onActivation;}
        if(t[index].text!="{"){
            Errors.Write("Esperado '{' en vez de '"+t[index].text+"'");
            return null;
        }
        if(t[index+1].text=="ScriptEffect" && t[index+2].text==":"){
            if(t[index+3].type!=TokenTypes.literal){
                Errors.Write("Esperado un string nombre de efecto de script en vez de '"+t[index+3].text+"'",t[index+3]);
                return null;
            }
            onActivation.effectCalls.Add(new ScriptEffect{effectName=t[index+3].text});
            if(t[index+4].text!="}"){
                Errors.Write("Esperado final de declaracion '}'",t[index+4]);
                return null;
            }
            return ParseOnActivation(onActivation,t,index+5);
        }
        return null;
    }
    public static void WriteJsonOfOnActivation(OnActivation onActivation,string address,string fileName){
        string jsonOnActivation=JsonConvert.SerializeObject(onActivation, Formatting.Indented);
        Debug.Log("Json of OnActivation:");
        Debug.Log(jsonOnActivation);
        if(!Directory.Exists(address)){
            Directory.CreateDirectory(address);
        }
        File.WriteAllText(address+fileName,jsonOnActivation);
    }
}
[System.Serializable]
public class OnActivation{
    public List<ScriptEffect> effectCalls=new List<ScriptEffect>();
}
[System.Serializable]
public abstract class EffectCall{
    public string effectName;
}
[System.Serializable]
public class ScriptEffect: EffectCall{
}
[System.Serializable]
public class OnActivationEffect: EffectCall{
}