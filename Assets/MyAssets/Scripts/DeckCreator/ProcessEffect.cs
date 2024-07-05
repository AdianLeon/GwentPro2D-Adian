using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProcessEffect : MonoBehaviour
{
    public static void SaveEffectOnJson(List<Token> tokenList,int start,int end){

    }
    public static void ExecuteEffect(GameObject caller,string onActivationName){
        Type script=Type.GetType(onActivationName);
        if(script==null){
            Debug.Log("No se encontro: '"+onActivationName+"', buscando json");
            ExecuteOnActivation(onActivationName);
            return;
        }
        if(!typeof(ICardEffect).IsAssignableFrom(script)){
            Debug.Log("ERROR: El script: '"+onActivationName+"' no es un efecto de carta");
            return;
        }
        if(caller.GetComponent<ICardEffect>()!=null){
            Debug.Log("ERROR: La carta ya tiene efecto, solo se le activara el existente...");
        }else{
            caller.AddComponent(script);
        }
        caller.GetComponent<ICardEffect>().TriggerEffect();
    }
    private static void ExecuteOnActivation(string onActivationName){
        // if(/*NotFound*/){
        //     Debug.Log("Error: '"+onActivationName+"' no fue encontrado como script ni como OnActivation.json");
        //     return;
        // }

        Debug.Log("Ejecutando json de nombre: '"+onActivationName+"'");
    }
}
