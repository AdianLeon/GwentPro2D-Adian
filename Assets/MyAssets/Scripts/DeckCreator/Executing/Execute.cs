using System;
using System.IO;
using UnityEngine;

public class Execute : MonoBehaviour
{
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
                //Buscar y compilar el efecto, luego activarlo de la manera que el sepa hacerlo
                string effectInCode = File.ReadAllText(Application.dataPath + "/MyAssets/Database/CreatedEffects/" + effectCall.EffectName + ".txt");
                EffectDeclaration effectDeclaration = EffectParser.ProcessCode(effectInCode);
                if (effectDeclaration == null) { return; }
                foreach (IActionStatement actionStatement in effectDeclaration.EffectAction.ActionStatements)
                {
                    actionStatement.DoAction();
                }
            }
        }
    }
}
