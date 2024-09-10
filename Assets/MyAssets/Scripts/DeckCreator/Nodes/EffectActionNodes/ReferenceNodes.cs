using System;
using System.Collections.Generic;

public interface IReference/*<T>*/ : INode
{
    public VarType Type { get; }
    /*public T Value { get; }*/
}
public class FutureReference : IReference
{
    public VarType Type { get; }
    public FutureReference(VarType varType) { Type = varType; }
}
public class PlayerReference : IReference
{
    public VarType Type => VarType.Player;
    public string Player;
    public PlayerReference(string player = "None") { if (!(player == "Self" || player == "Other" || player == "None")) { throw new Exception("Player: " + player); } Player = player; }
}
public class ContainerReference : IReference
{
    public VarType Type => VarType.Container;
    public string ContainerName;
    public IReference Owner;
    public ContainerReference(string containerName, IReference owner = null)
    {
        if (owner != null && owner.Type != VarType.Player) { throw new Exception("El tipo de variable debe ser Player"); }
        Owner = owner;
        ContainerName = containerName;
    }
}
public class CardReferenceList : IReference
{
    public VarType Type => VarType.CardList;
    public List<DraggableCard> Cards;
    public CardReferenceList(List<DraggableCard> cards) { Cards = cards; }
}
public class CardReference : IReference
{
    public VarType Type => VarType.Card;
    public DraggableCard Card;
    public string CardType { get { return ""; } }
    public string Name => Card.CardName;
    public string Faction => Card.Faction;
    public int Power
    {
        get => Card.GetComponent<PowerCard>() ? Card.GetComponent<PowerCard>().Power : Card.GetComponent<BoostCard>() ? Card.GetComponent<BoostCard>().Boost : Card.GetComponent<WeatherCard>() ? Card.GetComponent<WeatherCard>().Damage : 0;
        set { if (Card.GetComponent<PowerCard>()) { Card.GetComponent<PowerCard>().Power = value; } else if (Card.GetComponent<BoostCard>()) { Card.GetComponent<BoostCard>().Boost = value; } else if (Card.GetComponent<WeatherCard>()) { Card.GetComponent<WeatherCard>().Damage = value; } }
    }
    public Player Owner => Card.Owner;
    public CardReference(DraggableCard card) { Card = card; }
}
public class CardPropertyReference : IReference
{
    public IReference CardReference;
    public VarType Type { get; }
    public string PropertyAccessed;
    public CardPropertyReference(IReference cardReference, string propertyAccessed)
    {
        CardReference = cardReference;
        PropertyAccessed = propertyAccessed;
        switch (propertyAccessed)
        {
            case "Power": Type = VarType.Number; break;
            case "Owner": Type = VarType.Player; break;
            default: throw new NotImplementedException("Se ha intentado construir una referencia a la propiedad de carta: " + propertyAccessed);
        }
    }
}