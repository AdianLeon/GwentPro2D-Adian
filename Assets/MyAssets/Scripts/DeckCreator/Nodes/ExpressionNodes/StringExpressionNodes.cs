using System;

public class StringValueExpression : IExpression<string>
{
    public VarType Type => VarType.String;
    private string value;
    public string Evaluate() => value;
    public StringValueExpression(string str) { value = str; }
    public override string ToString() => Evaluate();
}
public class StringVariableReference : IExpression<string>
{
    public VarType Type => VarType.String;
    private string varName;
    public string Evaluate() => ((IExpression<string>)varName.ScopeValue()).Evaluate();
    public StringVariableReference(VariableReference variableReference)
    {
        // if (variableReference.DeReference() is not IExpression<string>) { throw new Exception("La variable no guarda una expresion de string"); }
        varName = variableReference.VarName;
    }
}
public class StringExpression : BinaryExpression<string, string>
{
    public override VarType Type => VarType.String;
    public StringExpression(IExpression<string> left, Token op, IExpression<string> right) : base(left, op, right) { }
    public override string Evaluate()
    {
        string left = Left.Evaluate(); string right = Right.Evaluate();
        switch (Operator.Text)
        {
            case "@": return left + right;
            case "@@": return left + " " + right;
            default: throw new NotImplementedException("El operador: '" + Operator.Text + "' no esta definido");
        }
    }
}