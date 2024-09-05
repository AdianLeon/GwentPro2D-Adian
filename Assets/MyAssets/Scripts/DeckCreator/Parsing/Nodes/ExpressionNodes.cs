using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public interface IExpression<ReturnType> : IReference { public ReturnType Evaluate(); }
public abstract class BinaryExpression<MemberType, ReturnType> : IExpression<ReturnType>
{
    public abstract VarType Type { get; }
    public IExpression<MemberType> Left; public Token Operator; public IExpression<MemberType> Right;
    protected BinaryExpression(IExpression<MemberType> left, Token op, IExpression<MemberType> right) { Left = left; Operator = op; Right = right; }
    public abstract ReturnType Evaluate();
}

//Aritmeticas
public class NumberExpression : IExpression<int>
{
    public VarType Type => VarType.Number;
    private int Value;
    public int Evaluate() => Value;
    public NumberExpression(string number) { Value = int.Parse(number); }
}
public class ArithmeticExpression : BinaryExpression<int, int>
{
    public override VarType Type => VarType.Number;
    public ArithmeticExpression(IExpression<int> left, Token op, IExpression<int> right) : base(left, op, right) { }
    public override int Evaluate()
    {
        var type = Type;
        int left = Left.Evaluate(); int right = Right.Evaluate();
        switch (Operator.Text)
        {
            case "+": return left + right;
            case "-": return left - right;
            case "*": return left * right;
            case "/": return left / (right == 0 ? throw new Exception("Division por 0, si desea ignorar esta excepcion descomente el codigo siguiente")/*int.MaxValue*/ : right);
            case "^": return (int)Math.Pow(left, right);
            default: throw new NotImplementedException("El operador: '" + Operator.Text + "' no esta definido");
        }
    }
}

//Booleanas
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

//Comparacion
public class ComparationExpression : BinaryExpression<IComparable, bool>
{
    public override VarType Type => VarType.Boolean;
    public ComparationExpression(IExpression<IComparable> left, Token op, IExpression<IComparable> right) : base(left, op, right) { }
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

//String
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
