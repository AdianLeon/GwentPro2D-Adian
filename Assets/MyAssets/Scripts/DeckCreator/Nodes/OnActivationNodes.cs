using System;
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
    public List<(string, IReference)> Parameters;
    public EffectSelector EffectSelector;
    public EffectPostAction EffectPostAction;
    public CreatedEffectCall(IExpression<string> effectName, List<(string, IReference)> parameters, EffectSelector effectSelector, EffectPostAction effectPostAction) { EffectName = effectName; Parameters = parameters; EffectSelector = effectSelector; EffectPostAction = effectPostAction; }
}
public class EffectPostAction : CreatedEffectCall
{
    public CreatedEffectCall Parent;
    public EffectPostAction(CreatedEffectCall postAction, CreatedEffectCall parent) : base(postAction.EffectName, postAction.Parameters, postAction.EffectSelector, postAction.EffectPostAction)
    { Parent = parent; }
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
    private string cardParameterName;
    private IExpression<bool> filter;
    public bool EvaluateCard(CardReference cardReference)
    {
        VariableScopes.Reset();
        VariableScopes.AddNewVar(cardParameterName, cardReference);
        bool fulfillsPredicate = filter.Evaluate();
        VariableScopes.PopLastScope();
        return fulfillsPredicate;
    }
    public CardPredicate(string parameterName, IExpression<bool> predicate) { cardParameterName = parameterName; filter = predicate; }
}