using System;
using System.Collections.Generic;
using System.Linq;

public enum VarType { None, Number, Bool, String, Card, Player, Container, CardList }//Tipos de variable
public static class VariableScopes
{//Clase estatica para representar la declaracion de variables
    public static bool IsEmpty() => scopes.Count == 0;
    private static Stack<Dictionary<string, IReference>> scopes;
    public static void Reset() { scopes = new Stack<Dictionary<string, IReference>>(); AddNewScope(); }
    public static bool ContainsVar(string varName) { return scopes.Any(dict => dict.ContainsKey(varName)); }
    public static void AddNewScope() => scopes.Push(new Dictionary<string, IReference>());
    public static void PopLastScope() => scopes.Pop();
    public static void AddNewVar(VariableDeclaration declaration) => AddNewVar(declaration.VarName, declaration.VarValue);
    public static void AddNewVar(string varName, IReference varValue)
    {
        if (ContainsVar(varName)) { scopes.Single(scope => scope.ContainsKey(varName))[varName] = varValue; }
        else { scopes.Peek().Add(varName, varValue); }
    }
    public static IReference ScopeValue(this string varName)
    {
        foreach (Dictionary<string, IReference> scope in scopes) { if (scope.ContainsKey(varName)) { return scope[varName].DeReference(); } }
        throw new Exception("La variable llamada: '" + varName + "' no ha sido declarada en este contexto");
    }
    public static IReference DeReference(this IReference possiblyVariableReference)
    {
        IReference reference = possiblyVariableReference;
        while (reference is VariableReference) { reference = ((VariableReference)reference).VarName.ScopeValue(); }
        return reference;
    }
}
public class VariableDeclaration : IActionStatement
{
    public string VarName;
    public IReference VarValue;
    public VariableDeclaration(string varName, IReference varValue) { VarName = varName; VarValue = varValue; }
    public void PerformAction() => VariableScopes.AddNewVar(this);
}
public class VariableReference : IReference
{
    public VarType Type { get; }
    public string VarName;
    public VariableReference(string varName, VarType type) { VarName = varName; Type = type; }
}
