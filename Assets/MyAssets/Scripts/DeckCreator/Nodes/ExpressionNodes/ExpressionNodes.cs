
using System.Runtime.InteropServices;

public interface IExpression<out T> : IReference { public T Evaluate(); }
public abstract class BinaryExpression<MemberType, ReturnType> : IExpression<ReturnType>
{
    public abstract VarType Type { get; }
    public IExpression<MemberType> Left; public Token Operator; public IExpression<MemberType> Right;
    protected BinaryExpression(IExpression<MemberType> left, Token op, IExpression<MemberType> right) { Left = left; Operator = op; Right = right; }
    public abstract ReturnType Evaluate();
    public override string ToString() => Evaluate().ToString();
}
