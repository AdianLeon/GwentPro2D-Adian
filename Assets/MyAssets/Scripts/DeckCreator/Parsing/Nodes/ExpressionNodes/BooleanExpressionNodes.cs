using System;

public class BooleanValueExpression : IExpression<bool>
{
    public VarType Type => VarType.Boolean;
    private bool value;
    public bool Evaluate() => value;
    public BooleanValueExpression(string boolean) { value = bool.Parse(boolean); }
    public override string ToString() => Evaluate().ToString();
}
public class BooleanExpression : BinaryExpression<bool, bool>
{
    public override VarType Type => VarType.Boolean;
    public BooleanExpression(IExpression<bool> left, Token op, IExpression<bool> right) : base(left, op, right) { }
    public override bool Evaluate()
    {
        bool left = Left.Evaluate(); bool right = Right.Evaluate();
        switch (Operator.Text)
        {// == != < > <= >= && ||
            case "&&": return left && right;
            case "||": return left || right;
            default: throw new NotImplementedException("El operador: '" + Operator.Text + "' no esta definido");
        }
    }
}
