using System;
using System.Collections.Generic;
using UnityEditor.Macros;

public class ComparisonValueExpression : IExpression<IReference>
{
    public VarType Type => VarType.Boolean;
    private IReference value;
    public IReference Evaluate() => value;
    public ComparisonValueExpression(IReference reference) { value = reference; }
    public override string ToString() => Evaluate().ToString();
}
public class ComparisonExpression : BinaryExpression<IReference, bool>
{
    public override VarType Type => VarType.Boolean;
    public ComparisonExpression(IExpression<IReference> left, Token op, IExpression<IReference> right) : base(left, op, right) { }
    public override bool Evaluate()
    {
        object left = Left.Evaluate(); object right = Right.Evaluate();
        EvaluateForAllTypes(ref left); EvaluateForAllTypes(ref right);
        switch (Operator.Text)
        {
            case "==": return Equals(left, right);
            case "!=": return !Equals(left, right);
        }
        if (!(left is int) || !(right is int)) { throw new Exception("No se comparo con == o != y no es una expresion aritmetica"); }
        switch (Operator.Text)
        {
            case "<": return (int)left < (int)right;
            case ">": return (int)left > (int)right;
            case "<=": return (int)left <= (int)right;
            case ">=": return (int)left >= (int)right;
            default: throw new NotImplementedException("El operador: '" + Operator.Text + "' no esta definido");
        }
    }
    private void EvaluateForAllTypes(ref object reference)
    {
        if (reference is IExpression<int>) { reference = ((IExpression<int>)reference).Evaluate(); }
        else if (reference is IExpression<bool>) { reference = ((IExpression<bool>)reference).Evaluate(); }
        else if (reference is IExpression<string>) { reference = ((IExpression<string>)reference).Evaluate(); }
    }
}