using System.Collections.Generic;
using System.Data;
public interface INode { }
public class FullDeclaration : INode
{
    public Queue<int> PositionsInCode = new Queue<int>();
    public List<BlockDeclaration> BlockDeclarations = new List<BlockDeclaration>();
}
public class BlockDeclaration : INode
{
    public string Name;
}
public class CardDeclaration : BlockDeclaration
{
    public string Type;
    public string Faction;
    public string Description;
    public int Power;
    public UnitCardZone Range;
    public OnActivation OnActivation;

    public CardDeclaration(string name, string type, string description, string faction, int power, UnitCardZone range, OnActivation onActivation)
    {
        Name = name;
        Type = type;
        Description = description;
        Faction = faction;
        Power = power;
        Range = range;
        OnActivation = onActivation;
    }
}
public class EffectDeclaration : BlockDeclaration
{
    public EffectAction EffectAction;
    public EffectDeclaration(string name, EffectAction effectAction) { Name = name; EffectAction = effectAction; }
}
public class EffectAction : INode
{
    public List<IActionStatement> ActionStatements = new List<IActionStatement>();
}
public interface IActionStatement
{
    public void DoAction();
}
public class PrintAction : IActionStatement
{
    public string Message;
    public PrintAction(string message) { Message = message; }

    public void DoAction()
    {
        UserRead.Write(Message);
    }
}