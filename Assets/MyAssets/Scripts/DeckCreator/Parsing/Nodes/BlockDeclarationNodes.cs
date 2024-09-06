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
    public IExpression<string> Name;
}
public class CardDeclaration : BlockDeclaration
{
    public IExpression<string> Type;
    public IExpression<string> Faction;
    public IExpression<string> Description;
    public IExpression<int> TotalCopies;
    public IExpression<int> Power;
    public UnitCardZone Range;
    public OnActivation OnActivation;

    public CardDeclaration(IExpression<string> name, IExpression<string> type, IExpression<string> description, IExpression<int> totalCopies, IExpression<string> faction, IExpression<int> power, UnitCardZone range, OnActivation onActivation)
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
    public EffectDeclaration(IExpression<string> name, EffectAction effectAction) { Name = name; EffectAction = effectAction; }
}
