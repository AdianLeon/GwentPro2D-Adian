using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EffectAction : INode
{
    public List<IActionStatement> ActionStatements = new List<IActionStatement>();
}
public interface IActionStatement { }
public class PrintAction : IActionStatement
{
    public string Message;
    public PrintAction(string message) { Message = message; }
}
public interface IReference { }
public class PlayerReference : IReference
{
    public enum PlayerToGet { None, Self, Other }
    public PlayerToGet Player;
    public PlayerReference(string player = "None") { Player = (PlayerToGet)Enum.Parse(typeof(PlayerToGet), player); }
}
public class ContainerReference : IReference
{
    public enum ContainerToGet { Board, Hand, Field, Graveyard, Deck }
    public ContainerToGet Name;
    public PlayerReference Owner;
    public ContainerReference(string containerName, PlayerReference owner = null)
    {
        Name = (ContainerToGet)Enum.Parse(typeof(ContainerToGet), containerName); Owner = owner;
    }
}
public abstract class ContextMethod : IActionStatement
{
    public ContainerReference Container;
}
// public class ContextFindMethod : ContextMethod
// {
//     public CardPredicate Predicate;
//     public ContextFindMethod(ContainerReference container, CardPredicate predicate) { Container = container; Predicate = predicate; }
// }
// public class CardPredicate { }
public class ContextCardParameterMethod : ContextMethod
{
    public enum ActionType { Push, SendBottom/*, Remove*/ }
    public ActionType Type;
    public ICardReference Card;
    public ContextCardParameterMethod(ContainerReference container, string typeName, ICardReference card)
    {
        Container = container;
        Type = (ActionType)Enum.Parse(typeof(ActionType), typeName);
        Card = card;
    }
}
public interface ICardReference : IReference { }
public class ContextPopMethod : ContextMethod, ICardReference
{
    public ContextPopMethod(ContainerReference container) { Container = container; }
}
public class ContextShuffleMethod : ContextMethod
{
    public ContextShuffleMethod(ContainerReference container) { Container = container; }
}