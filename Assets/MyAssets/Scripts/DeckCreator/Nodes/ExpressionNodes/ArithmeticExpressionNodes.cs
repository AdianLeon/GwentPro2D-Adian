using System;
//Expresiones aritmeticas
public class NumberExpression : IExpression<int>
{
    public VarType Type => VarType.Number;
    private int value;
    public int Evaluate() => value;
    public NumberExpression(string number) { value = int.Parse(number); }
    public override string ToString() => Evaluate().ToString();
}
public class NumberVariableReference : IExpression<int>
{
    public VarType Type => VarType.Number;
    private string varName;
    public int Evaluate() => ((IExpression<int>)varName.ScopeValue()).Evaluate();
    public NumberVariableReference(VariableReference variableReference) { varName = variableReference.VarName; }
}
public class ArithmeticExpression : BinaryExpression<int, int>
{
    public override VarType Type => VarType.Number;
    public ArithmeticExpression(IExpression<int> left, string op, IExpression<int> right) : base(left, op, right) { }
    public override int Evaluate()
    {
        int left = Left.Evaluate(); int right = Right.Evaluate();
        switch (Operator)
        {
            case "+": return left + right;
            case "-": return left - right;
            case "*": return left * right;
            case "/": return left / (right == 0 ? throw new Exception("Division por 0") : right);
            case "^": return (int)Math.Pow(left, right);
            default: throw new NotImplementedException("El operador: '" + Operator + "' no esta definido");
        }
    }
}