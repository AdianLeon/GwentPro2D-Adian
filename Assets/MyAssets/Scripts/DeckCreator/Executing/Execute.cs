using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Execute : MonoBehaviour, IStateSubscriber
{
    public GameObject errorScreen;
    public static bool LoadedAllEffects => loadedAllEffects;
    private static bool loadedAllEffects;
    private static Dictionary<string, EffectDeclaration> createdEffects;
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (State.Loading, new Execution (stateInfo => LoadEffects(), 0))
    };
    private void LoadEffects()
    {
        Debug.Log(0);
        createdEffects = new Dictionary<string, EffectDeclaration>();
        loadedAllEffects = true;
        string[] addressesOfEffects = Directory.GetFiles(Application.dataPath + "/MyAssets/Database/CreatedEffects", "*.txt");//Obtiene dentro del directorio del deck solo la direccion de los archivos con extension txt (ignora los meta)

        errorScreen.SetActive(true);
        foreach (string address in addressesOfEffects)
        {//Para cada uno de los archivos con extension json
            string codeEffect = File.ReadAllText(address);//Lee el archivo
            EffectDeclaration effectDeclaration = EffectParser.ProcessCode(codeEffect);//Convierte el string en json a un objeto 
            if (effectDeclaration != null) { createdEffects.Add(effectDeclaration.Name, effectDeclaration); }
            else { Errors.Write("No se pudo procesar el texto del efecto en: " + address); loadedAllEffects = false; }
        }
        Debug.Log("On Execute: " + LoadedAllEffects);
    }

    public static void DoEffect(Card card)
    {
        if (card.OnActivation == null) { return; }
        foreach (EffectCall effectCall in card.OnActivation.effectCalls)
        {
            if (effectCall is ScriptEffectCall)
            {
                Type effectType = Type.GetType(effectCall.EffectName);
                ICardEffect effectScript = (ICardEffect)card.GetComponent(effectType);
                effectScript.TriggerEffect();
            }
            else if (effectCall is CreatedEffectCall)
            {
                if (effectCall == null) { return; }
                if (!createdEffects.ContainsKey(effectCall.EffectName)) { return; }
                createdEffects[effectCall.EffectName].EffectAction.TriggerEffect();
            }
        }
    }
}
