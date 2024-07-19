using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Execute : MonoBehaviour
{
    public static void DoEffect(GameObject caller, string onActivationName)
    {
        if (onActivationName == "") { return; }
        if (Type.GetType(onActivationName) == null)
        {
            // Debug.Log("No se encontro: '" + onActivationName + "', buscando OnActivation");
            DoOnActivation(caller, onActivationName);
            return;
        }
        Type script = Type.GetType(onActivationName);
        AddICardEffect(caller, onActivationName, script);
    }
    private static void AddICardEffect(GameObject caller, string onActivationName, Type effectScript)
    {
        if (!typeof(ICardEffect).IsAssignableFrom(effectScript))
        {
            // Debug.Log("ERROR: El script: '"+onActivationName+"' no es un efecto de carta");
            return;
        }
        if (caller.GetComponent<ICardEffect>() != null)
        {
            // Debug.Log("ERROR: La carta ya tiene efecto, solo se le activara el existente...");
        }
        else
        {
            // Debug.Log("Anadiendo script...");
            caller.AddComponent(effectScript);
            // Debug.Log("Script anadido");
        }
        // Debug.Log("Realizando efecto");
        caller.GetComponent<ICardEffect>().TriggerEffect();
    }
    private static void DoOnActivation(GameObject caller, string onActivationName)
    {
        string address = Application.dataPath + "/MyAssets/Database/CardsOnActivations/";
        string onActivationJson = File.ReadAllText(address + onActivationName);
        OnActivation savedOnActivation = JsonConvert.DeserializeObject<OnActivation>(onActivationJson);
        if (savedOnActivation == null)
        {
            // Debug.Log("Error: '" + onActivationName + "' no fue encontrado como script ni como OnActivation.json");
            return;
        }

        for (int i = 0; i < savedOnActivation.effectCalls.Count; i++)
        {
            DoEffect(caller, savedOnActivation.effectCalls[i].effectName);
        }
    }
}
