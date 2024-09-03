using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
public enum VarType { Number, Boolean, String, Card, Player, Container, CardList }
public class EffectAction : INode
{
    public List<IActionStatement> ActionStatements = new List<IActionStatement>();
}
public interface IActionStatement { }
public interface IReference { public VarType Type { get; } }
public class PlayerReference : IReference
{
    public VarType Type => VarType.Player;
    public enum PlayerToGet { None, Self, Other }
    public PlayerToGet Player;
    public PlayerReference(string player = "None") { Player = (PlayerToGet)Enum.Parse(typeof(PlayerToGet), player); }
}
public class ContainerReference : IReference
{
    public VarType Type => VarType.Container;
    public enum ContainerToGet { Board, Hand, Field, Graveyard, Deck }
    public ContainerToGet Name;
    public IReference Owner;
    public ContainerReference(string containerName, IReference owner = null)
    {
        if (owner.Type != VarType.Player) { throw new Exception("El tipo de variable debe ser Player"); }
        Name = (ContainerToGet)Enum.Parse(typeof(ContainerToGet), containerName); Owner = owner;
    }
}
public class PrintAction : IActionStatement
{
    public string Message;
    public PrintAction(string message) { Message = message; }
}
public abstract class ContextMethod : IActionStatement
{
    public ContainerReference Container;
}
public class ContextCardParameterMethod : ContextMethod
{
    public enum ActionType { Push, SendBottom/*, Remove*/ }
    public ActionType Type;
    public IReference Card;
    public ContextCardParameterMethod(ContainerReference container, string typeName, IReference card)
    {
        Container = container;
        Type = (ActionType)Enum.Parse(typeof(ActionType), typeName);
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
public class VariableScopes
{
    private Stack<Dictionary<string, IReference>> scopes = new Stack<Dictionary<string, IReference>>();
    public bool ContainsVar(string varName) => scopes.Any(dict => dict.ContainsKey(varName));
    public void AddNewScope() => scopes.Push(new Dictionary<string, IReference>());
    public void PopLastScope() => scopes.Pop();
    public void AddNewVar(VariableDeclaration declaration) => AddNewVar(declaration.VarName, declaration.VarValue);
    public void AddNewVar(string varName, IReference varValue)
    {
        if (ContainsVar(varName)) { scopes.Peek().Remove(varName); }
        scopes.Peek().Add(varName, varValue);
    }
    public IReference GetValue(string varName)
    {
        foreach (Dictionary<string, IReference> scope in scopes) { if (scope.ContainsKey(varName)) { return scope[varName]; } }
        throw new Exception("La variable llamada: '" + varName + "' no ha sido declarada en este contexto");
    }
}
public class VariableDeclaration : IActionStatement
{
    public string VarName;
    public IReference VarValue;
    public VariableDeclaration(string varName, IReference varValue) { VarName = varName; VarValue = varValue; }
}
public class VariableReference : IReference
{
    public VarType Type { get; }
    public string VarName;
    public VariableReference(string varName, VarType type) { VarName = varName; Type = type; }
}
public class FutureCardReferenceList : IReference { public VarType Type => VarType.CardList; }
public class FutureCardReference : IReference { public VarType Type => VarType.Card; }
public class CardReferenceList : FutureCardReferenceList
{
    public List<DraggableCard> Cards;
    public CardReferenceList(List<DraggableCard> cards) { Cards = cards; }
}
public class CardReference : FutureCardReference
{
    public DraggableCard Card;
    public string CardType { get { return ""; } }
    public string Name => Card.CardName;
    public string Faction => Card.Faction;
    public int Power
    {
        get => Card.GetComponent<PowerCard>() ? Card.GetComponent<PowerCard>().Power : Card.GetComponent<BoostCard>() ? Card.GetComponent<BoostCard>().Boost : Card.GetComponent<WeatherCard>() ? Card.GetComponent<WeatherCard>().Damage : 0;
        set { if (Card.GetComponent<PowerCard>()) { Card.GetComponent<PowerCard>().Power = value; } else if (Card.GetComponent<BoostCard>()) { Card.GetComponent<BoostCard>().Boost = value; } else if (Card.GetComponent<WeatherCard>()) { Card.GetComponent<WeatherCard>().Damage = value; } }
    }
    public PlayerReference Owner => Card.Owner == Judge.GetPlayer ? new PlayerReference("Self") : new PlayerReference("Other");
    public CardReference(DraggableCard card) { Card = card; }
}
public class ForEachCycle : IActionStatement
{
    public string IteratorVarName;
    public IReference CardReferences;
    public List<IActionStatement> ActionStatements = new List<IActionStatement>();
    public ForEachCycle(string iteratorVarName, IReference cardReferences, List<IActionStatement> actionStatements) { IteratorVarName = iteratorVarName; CardReferences = cardReferences; ActionStatements = actionStatements; }
}