using System;
using System.Collections.Generic;
using UnityEngine;

public class Execute : MonoBehaviour
{
    public static void DoEffect(Card card)
    {
        if (card.OnActivation == null) { Debug.Log("OnActivation null"); return; }

        foreach (EffectCall effectCall in card.OnActivation.effectCalls)
        {
            if (effectCall is ScriptEffect)
            {
                Type effectType = Type.GetType(effectCall.effectName);
                ICardEffect effectScript = (ICardEffect)card.GetComponent(effectType);
                effectScript.TriggerEffect();
            }
        }
    }
}
