using System;
using System.Collections.Generic;
using System.Linq;

public enum VarType { Number, Boolean, String, Card, Player, Container, CardList }
public class VariableScopes
{
    private Stack<Dictionary<string, IReference>> scopes = new Stack<Dictionary<string, IReference>>();
    public bool ContainsVar(string varName) => scopes.Any(dict => dict.ContainsKey(varName));
    public void AddNewScope() => scopes.Push(new Dictionary<string, IReference>());
    public void PopLastScope() => scopes.Pop();
    public void AddNewVar(VariableDeclaration declaration) => AddNewVar(declaration.VarName, declaration.VarValue);
    public void AddNewVar(string varName, IReference varValue)
    {
        if (ContainsVar(varName)) { scopes.Peek().Remove(varName); }
        scopes.Peek().Add(varName, varValue);
    }
    public IReference GetValue(string varName)
    {
        foreach (Dictionary<string, IReference> scope in scopes) { if (scope.ContainsKey(varName)) { return scope[varName]; } }
        throw new Exception("La variable llamada: '" + varName + "' no ha sido declarada en este contexto");
    }
}
public class VariableDeclaration : IActionStatement
{
    public string VarName;
    public IReference VarValue;
    public VariableDeclaration(string varName, IReference varValue) { VarName = varName; VarValue = varValue; }
}
public class VariableReference : IReference
{
    public VarType Type { get; }
    public string VarName;
    public VariableReference(string varName, VarType type) { VarName = varName; Type = type; }
}
