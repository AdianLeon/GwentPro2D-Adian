using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public interface IActionStatement : INode { public void PerformAction(); }
public class PrintAction : IActionStatement
{
    public IExpression<string> Message;
    public PrintAction(IExpression<string> message) { Message = message; }
    public void PerformAction() => UserRead.Write(Message.Evaluate());
}
public abstract class ContextMethod : IActionStatement
{
    public ContainerReference Container;

    public abstract void PerformAction();
}
public class ContextCardParameterMethod : ContextMethod
{
    public string ActionType;
    public IReference Card;
    public ContextCardParameterMethod(ContainerReference container, string actionType, IReference card)
    {
        Container = container;
        ActionType = actionType;
        if (card.Type != VarType.Card) { throw new Exception("El tipo de parametro de un metodo de contexto con parametro carta no es carta"); }
        Card = card;
    }
    public override void PerformAction() => ContextUtils.DoActionForCardParameterMethod(this);
}
public class ContextPopMethod : ContextMethod, IReference
{
    public VarType Type => VarType.Card;
    public ContextPopMethod(ContainerReference container) { Container = container; }
    public override void PerformAction() => ContextUtils.PopContainer(this);
}
public class ContextShuffleMethod : ContextMethod
{
    public ContextShuffleMethod(ContainerReference container) { Container = container; }
    public override void PerformAction() => ContextUtils.ShuffleContainer(this);
}
public class CardPowerSetting : IActionStatement
{
    public IReference CardReference;
    public IExpression<int> NewPower;
    public CardPowerSetting(IReference cardReference, IExpression<int> newPower) { if (cardReference.Type != VarType.Card) { throw new Exception("El tipo de parametro de un metodo de contexto con parametro carta no es carta"); } CardReference = cardReference; NewPower = newPower; }

    public void PerformAction()
    {
        IReference reference = CardReference;
        while (reference is VariableReference) { reference = ((VariableReference)reference).VarName.ScopeValue(); }
        if (reference is CardReference) { ((CardReference)reference).Power = NewPower.Evaluate(); }
    }
}
public class ForEachCycle : IActionStatement
{
    public string IteratorVarName;
    public IReference CardReferences;
    public List<IActionStatement> ActionStatements = new List<IActionStatement>();
    public ForEachCycle(string iteratorVarName, IReference cardReferences, List<IActionStatement> actionStatements) { IteratorVarName = iteratorVarName; CardReferences = cardReferences; ActionStatements = actionStatements; }
    public void PerformAction()
    {
        List<DraggableCard> cards;
        if (CardReferences is VariableReference)
        {
            IReference reference = CardReferences;
            while (reference is VariableReference) { reference = ((VariableReference)reference).VarName.ScopeValue(); }
            if (reference is not CardReferenceList) { throw new Exception("La variable llamada: '" + ((VariableReference)CardReferences).VarName + "' no contiene una CardReferenceList"); }
            cards = ((CardReferenceList)reference).Cards;
        }
        else { throw new NotImplementedException("No se ha anadido la forma de evaluar demandada"); }
        VariableScopes.AddNewScope();
        foreach (DraggableCard card in cards)
        {
            VariableScopes.AddNewVar(IteratorVarName, new CardReference(card));
            ActionStatements.ForEach(action => action.PerformAction());
        }
        VariableScopes.PopLastScope();
    }
}