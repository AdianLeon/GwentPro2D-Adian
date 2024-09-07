using System.Collections.Generic;

public class OnActivation : INode
{
    public List<EffectCall> effectCalls = new List<EffectCall>();
}
public abstract class EffectCall
{
    public IExpression<string> EffectName;
}
public class ScriptEffectCall : EffectCall
{
    public ScriptEffectCall(IExpression<string> effectName) { EffectName = effectName; }
}
public class CreatedEffectCall : EffectCall
{
    public EffectSelector EffectSelector;
    public EffectPostAction EffectPostAction;
    public CreatedEffectCall(IExpression<string> effectName, EffectSelector effectSelector) { EffectName = effectName; EffectSelector = effectSelector; }
}
public class EffectSelector
{
    public IExpression<string> Source;
    public IExpression<bool> Single;
    public CardPredicate CardPredicate;
    public EffectSelector(IExpression<string> source, IExpression<bool> single, CardPredicate cardPredicate) { Source = source; Single = single; CardPredicate = cardPredicate; }
}
public class CardPredicate
{
}
public class EffectPostAction
{
}