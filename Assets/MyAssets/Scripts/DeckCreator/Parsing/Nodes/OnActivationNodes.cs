using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnActivation : INode
{
    public void AddScriptEffects(GameObject cardOwner) { foreach (EffectCall effectCall in effectCalls) { if (effectCall is ScriptEffectCall) { cardOwner.AddComponent(Type.GetType(effectCall.EffectName)); } } }
    public List<EffectCall> effectCalls = new List<EffectCall>();
}
public abstract class EffectCall
{
    public string EffectName;
}
public class ScriptEffectCall : EffectCall
{
    public ScriptEffectCall(string effectName) { EffectName = effectName; }
}
public class CreatedEffectCall : EffectCall
{
    public CreatedEffectCall(string effectName) { EffectName = effectName; }
}