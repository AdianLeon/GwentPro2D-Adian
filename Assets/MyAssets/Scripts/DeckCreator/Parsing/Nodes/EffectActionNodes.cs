using System;
using System.Collections.Generic;
using System.ComponentModel;

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
    public PlayerReference(PlayerToGet player) { Player = player; }
}
public class ContainerReference : IReference
{
    public enum ContainerToGet { Board, Hand, Field, Graveyard, Deck }
    public ContainerToGet Container;
    public PlayerReference Owner;
    public ContainerReference(ContainerToGet container, PlayerReference owner = null)
    {
        Container = container; Owner = owner;
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
// public class ContextCardParameterMethod : ContextMethod
// {
//     public enum ActionType { Push, SendBottom, Remove }
//     public ActionType Type;
//     public CardReference Card;
//     public ContextCardParameterMethod(ContainerReference container, ActionType type, CardReference card) { Container = container; Type = type; Card = card; }
// }
// public class CardReference : IReference { }
// public class ContextPopMethod : ContextMethod, IReference
// {
//     public ContextPopMethod(ContainerReference container) { Container = container; }
// }
public class ContextShuffleMethod : ContextMethod
{
    public ContextShuffleMethod(ContainerReference container) { Container = container; }
}