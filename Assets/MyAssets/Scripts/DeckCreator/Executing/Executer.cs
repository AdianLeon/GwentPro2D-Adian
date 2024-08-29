using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Executer : MonoBehaviour, IStateSubscriber
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
    }

    public static void ExecuteOnActivation(Card card)
    {
        if (card.OnActivation == null) { return; }
        foreach (EffectCall effectCall in card.OnActivation.effectCalls)
        {
            if (effectCall == null) { return; }
            else if (effectCall is ScriptEffectCall)
            {
                Type effectType = Type.GetType(effectCall.EffectName);
                ICardEffect effectScript = (ICardEffect)card.GetComponent(effectType);
                effectScript.TriggerEffect();
            }
            else if (effectCall is CreatedEffectCall)
            {
                if (!createdEffects.ContainsKey(effectCall.EffectName)) { return; }
                List<DraggableCard> targets = SelectTargets(card.OnActivation);
                ExecuteEffect((CreatedEffectCall)effectCall, targets);
            }
        }
    }
    private static void ExecuteEffect(CreatedEffectCall effectCall, List<DraggableCard> targets)
    {
        foreach (IActionStatement actionStatement in createdEffects[effectCall.EffectName].EffectAction.ActionStatements)
        {
            if (actionStatement is PrintAction) { UserRead.Write((actionStatement as PrintAction).Message); }
            else if (actionStatement is ContextShuffleMethod) { ContextUtils.ShuffleContainer(actionStatement as ContextShuffleMethod); }
        }
    }
    private static List<DraggableCard> SelectTargets(OnActivation onActivation)
    {
        return null;
    }
}
