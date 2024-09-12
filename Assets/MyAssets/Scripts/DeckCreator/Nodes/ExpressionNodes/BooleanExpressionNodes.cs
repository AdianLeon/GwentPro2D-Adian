using System;

public class BooleanValueExpression : IExpression<bool>
{
    public VarType Type => VarType.Bool;
    private bool value;
    public bool Evaluate() => value;
    public BooleanValueExpression(string boolean) { value = bool.Parse(boolean); }
    public override string ToString() => Evaluate().ToString();
}
public class BooleanVariableReference : IExpression<bool>
{
    public VarType Type => VarType.Bool;
    private string varName;
    public bool Evaluate() => ((IExpression<bool>)varName.ScopeValue()).Evaluate();
    public BooleanVariableReference(VariableReference variableReference)
    {
        if (variableReference.DeReference() is not IExpression<bool>) { throw new Exception("La variable no guarda una expresion booleana"); }
        varName = variableReference.VarName;
    }
}
public class BooleanExpression : BinaryExpression<bool, bool>
{
    public override VarType Type => VarType.Bool;
    public BooleanExpression(IExpression<bool> left, string op, IExpression<bool> right) : base(left, op, right) { }
    public override bool Evaluate()
    {
        bool left = Left.Evaluate(); bool right = Right.Evaluate();
        switch (Operator)
        {// == != < > <= >= && ||
            case "&&": return left && right;
            case "||": return left || right;
            default: throw new NotImplementedException("El operador: '" + Operator + "' no esta definido");
        }
    }
}
