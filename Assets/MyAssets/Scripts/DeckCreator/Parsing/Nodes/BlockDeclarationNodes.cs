using System.Collections.Generic;
public interface INode { }
public class FullDeclaration : INode
{
    public Queue<int> positionsInCode;
    public List<BlockDeclaration> blockDeclarations;
}
public class BlockDeclaration : INode
{
    public string Name;
}
public class CardDeclaration : BlockDeclaration
{
    public string Type;
    public string Faction;
    public int Power;
    public UnitCardZone Range;
    public OnActivation OnActivation;

    public CardDeclaration(string name, string type, string faction, int power, UnitCardZone range, OnActivation onActivation)
    {
        Name = name;
        Type = type;
        Faction = faction;
        Power = power;
        Range = range;
        OnActivation = onActivation;
    }
}
public class EfectDeclaration : BlockDeclaration
{

}