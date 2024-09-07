using System;

public class ComparisonValueExpression : IExpression<IComparable>
{
    public VarType Type => VarType.Boolean;
    private IComparable value;
    public IComparable Evaluate() => value;
    public ComparisonValueExpression(string text)
    {
        int aux;
        if (int.TryParse(text, out aux)) { value = aux; }
        else { value = text; }
    }
    public override string ToString() => Evaluate().ToString();
}
public class ComparisonExpression : BinaryExpression<IComparable, bool>
{
    public override VarType Type => VarType.Boolean;
    public ComparisonExpression(IExpression<IComparable> left, Token op, IExpression<IComparable> right) : base(left, op, right) { }
    public override bool Evaluate()
    {
        IComparable left = Left.Evaluate(); IComparable right = Right.Evaluate();
        switch (Operator.Text)
        {
            case "==": return left.CompareTo(right) == 0;
            case "!=": return left.CompareTo(right) != 0;
            case "<": return left.CompareTo(right) < 0;
            case ">": return left.CompareTo(right) > 0;
            case "<=": return left.CompareTo(right) <= 0;
            case ">=": return left.CompareTo(right) >= 0;
            default: throw new NotImplementedException("El operador: '" + Operator.Text + "' no esta definido");
        }
    }
}