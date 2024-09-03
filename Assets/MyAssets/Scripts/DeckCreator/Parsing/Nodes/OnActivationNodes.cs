using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnActivation : INode
{
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
    public EffectSelector EffectSelector;
    public EffectPostAction EffectPostAction;
    public CreatedEffectCall(string effectName, EffectSelector effectSelector) { EffectName = effectName; EffectSelector = effectSelector; }
}
public class EffectSelector
{
    public string Source;
    public EffectSelector(string source) { Source = source; }
}
public class EffectPostAction
{
}