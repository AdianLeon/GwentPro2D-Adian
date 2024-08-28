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
    public int TotalCopies;
    public int Power;
    public UnitCardZone Range;
    public OnActivation OnActivation;

    public CardDeclaration(string name, string type, string description, int totalCopies, string faction, int power, UnitCardZone range, OnActivation onActivation)
    {
        Name = name;
        Type = type;
        Description = description;
        TotalCopies = totalCopies;
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
