using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnActivation : INode
{
    public void AddScriptEffects(GameObject cardOwner)
    {
        foreach (EffectCall effectCall in effectCalls)
        {
            if (effectCall is ScriptEffect) { cardOwner.AddComponent(Type.GetType(effectCall.effectName)); }
        }
    }
    public List<EffectCall> effectCalls = new List<EffectCall>();
}
public abstract class EffectCall
{
    public string effectName;
}
public class ScriptEffect : EffectCall
{
    public ScriptEffect(string scriptName) { effectName = scriptName; }
}
public class CreatedEffect : EffectCall
{

}