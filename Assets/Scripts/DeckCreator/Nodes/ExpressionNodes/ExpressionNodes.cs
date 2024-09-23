
using System;
//Expresiones genericas y de carta
public interface IExpression<out T> : IReference { public T Evaluate(); }
public abstract class BinaryExpression<MemberType, ReturnType> : IExpression<ReturnType>
{
    public abstract VarType Type { get; }
    public IExpression<MemberType> Left; public string Operator; public IExpression<MemberType> Right;
    protected BinaryExpression(IExpression<MemberType> left, string op, IExpression<MemberType> right) { Left = left; Operator = op; Right = right; }
    public abstract ReturnType Evaluate();
    public override string ToString() => Evaluate().ToString();
}
public class PowerCardPropertyExpression : IExpression<int>
{
    private CardPropertyReference cardPropertyReference;
    public VarType Type { get; }

    public int Evaluate()
    {
        CardReference cardReference;
        if (cardPropertyReference.CardReference is CardReference) { cardReference = (CardReference)cardPropertyReference.CardReference; }
        else if (cardPropertyReference.CardReference is VariableReference) { cardReference = (CardReference)cardPropertyReference.CardReference.DeReference(); }
        else { throw new NotImplementedException(); }
        if (cardPropertyReference.PropertyAccessed == "Power") { return cardReference.Power; }
        else { throw new NotImplementedException("Se ha intentado acceder a la propiedad de carta: " + cardPropertyReference.PropertyAccessed); }
    }
    public PowerCardPropertyExpression(CardPropertyReference card)
    {
        cardPropertyReference = card;
        if (card.Type == VarType.Number) { Type = VarType.Number; }
        else { throw new NotImplementedException("No era Type Number"); }
    }
}
public class StringCardPropertyExpression : IExpression<string>
{
    private CardPropertyReference cardPropertyReference;
    public VarType Type { get; }

    public string Evaluate()
    {
        CardReference cardReference;
        if (cardPropertyReference.CardReference is CardReference) { cardReference = (CardReference)cardPropertyReference.CardReference; }
        else if (cardPropertyReference.CardReference is VariableReference) { cardReference = (CardReference)cardPropertyReference.CardReference.DeReference(); }
        else { throw new NotImplementedException(); }
        switch (cardPropertyReference.PropertyAccessed)
        {
            case "Name": return cardReference.Name;
            case "Faction": return cardReference.Faction;
            default: throw new NotImplementedException("Se ha intentado acceder a la propiedad de carta: " + cardPropertyReference.PropertyAccessed);
        }
    }
    public StringCardPropertyExpression(CardPropertyReference card)
    {
        cardPropertyReference = card;
        if (card.Type == VarType.String) { Type = VarType.String; }
        else { throw new NotImplementedException("No era Type String"); }
    }
}