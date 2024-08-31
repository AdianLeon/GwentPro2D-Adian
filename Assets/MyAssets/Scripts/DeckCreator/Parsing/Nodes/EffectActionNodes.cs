using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EffectAction : INode
{
    public List<IActionStatement> ActionStatements = new List<IActionStatement>();
}
public interface IActionStatement { }
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
public class VariableScopes
{
    private Stack<Dictionary<string, IReference>> scopes = new Stack<Dictionary<string, IReference>>();
    public bool ContainsVar(string varName) => scopes.Any(dict => dict.ContainsKey(varName));
    public void AddNewScope() => scopes.Push(new Dictionary<string, IReference>());
    public void PopLastScope() => scopes.Pop();
    public bool AddNewVar(VariableDeclaration declaration) => AddNewVar(declaration.VarName, declaration.VarValue);
    public bool AddNewVar(string varName, IReference varValue)
    {
        if (ContainsVar(varName)) { return false; }
        scopes.Peek().Add(varName, varValue);
        return true;
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
    public string VarRefencedName;
    // public Type ExpectedType;
    public VariableReference(string varRefencedName) { VarRefencedName = varRefencedName; }
}