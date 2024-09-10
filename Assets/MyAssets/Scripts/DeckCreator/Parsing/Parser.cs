using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Parser
{
    public static void StartParsing(List<Token> tokens) { Parser.tokens = tokens; index = 0; hasFailed = false; }
    private static List<Token> tokens;
    protected static List<Token> GetTokens => tokens.Select(type => type).ToList();
    private static int index;
    protected static bool hasFailed;
    public static bool HasFailed => hasFailed;
    protected static Token Current => tokens[index];
    protected static Token Peek(int forward = 1) => tokens[index + forward];
    protected static Token Next(int forward = 1) { index += forward; return tokens[index]; }
    public abstract INode ParseTokens();
    protected bool Try<T>(Func<INode> parser, out T aux) where T : INode
    {
        INode node = parser();
        if (node is T) { aux = (T)node; return true; }
        else { aux = default; return false; }
    }
    protected bool Try<T>(Func<bool, INode> parser, bool param, out T aux) where T : INode
    {
        INode node = parser(param);
        if (node is T) { aux = (T)node; return true; }
        else { aux = default; return false; }
    }
    protected INode ParseGeneric()
    {
        Debug.Log("Analyzing the token: " + Current);
        INode node = ParseSemiGeneric();
        if (node != null && !hasFailed) { return node; }
        hasFailed = false;
        IReference reference = ParseExpression();
        if (reference != null) { return reference; }

        Errors.Write("Se intento procesar pero no hay ninguna definicion correspondiente", Current); hasFailed = true; return null;
    }
    protected INode ParseSemiGeneric()
    {
        switch (Current.Text)
        {
            case "context": return new ContextParser().ParseTokens();
            case "Print": return new EffectActionParser().ParsePrintAction();
            case "for": return new EffectActionParser().ParseForEachCycle();
            case "while": return new EffectActionParser().ParseWhileCycle();
        }
        if (Current.Is(TokenType.identifier)) { return new VariableParser().ParseTokens(); }
        hasFailed = true; return null;
    }
    protected IReference ParseExpression(bool includeComparison = true)
    {
        List<Func<INode>> expressionParsers = new() { new ArithmeticExpressionsParser().ParseTokens, new BooleanExpressionsParser().ParseTokens, new StringExpressionsParser().ParseTokens };
        if (includeComparison) { expressionParsers.Insert(0, new ComparisonExpressionsParser().ParseTokens); }
        int startingIndex = index;
        foreach (Func<INode> parser in expressionParsers)
        {
            Debug.Log("Trying parsing the expression: " + parser + " in token: " + Current);
            IReference expressionResultant = (IReference)parser();
            if (expressionResultant != null && !hasFailed) { Debug.Log("Success!!"); return expressionResultant; }
            hasFailed = false;
            index = startingIndex;
        }
        hasFailed = true; return null;
    }
}