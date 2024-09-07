using System;
using System.Collections.Generic;

public interface IActionStatement : INode {/*public void PerformAction();*/ }
public class PrintAction : IActionStatement
{
    public IExpression<string> Message;
    public PrintAction(IExpression<string> message) { Message = message; }
}
public abstract class ContextMethod : IActionStatement
{
    public ContainerReference Container;
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
}
public class ContextPopMethod : ContextMethod, IReference
{
    public VarType Type => VarType.Card;
    public ContextPopMethod(ContainerReference container) { Container = container; }
}
public class ContextShuffleMethod : ContextMethod
{
    public ContextShuffleMethod(ContainerReference container) { Container = container; }
}
public class CardPowerSetting : IActionStatement
{
    public IReference CardReference;
    public IExpression<int> NewPower;
    public CardPowerSetting(IReference cardReference, IExpression<int> newPower) { if (cardReference.Type != VarType.Card) { throw new Exception("El tipo de parametro de un metodo de contexto con parametro carta no es carta"); } CardReference = cardReference; NewPower = newPower; }
}
public class ForEachCycle : IActionStatement
{
    public string IteratorVarName;
    public IReference CardReferences;
    public List<IActionStatement> ActionStatements = new List<IActionStatement>();
    public ForEachCycle(string iteratorVarName, IReference cardReferences, List<IActionStatement> actionStatements) { IteratorVarName = iteratorVarName; CardReferences = cardReferences; ActionStatements = actionStatements; }
}