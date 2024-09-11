using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class Parser
{
    public static void StartParsing(List<Token> tokens) { Parser.tokens = tokens; index = 0; hasFailed = false; }
    public static bool HasFailed => hasFailed;
    private static bool hasFailed;
    private static List<Token> tokens;
    private static int index;
    private static Token Current => tokens[index];
    private static Token Peek(int forward = 1) => tokens[index + forward];
    private static Token Next(int forward = 1) { index += forward; return tokens[index]; }
    private static bool Try<T>(Func<INode> parser, out T aux, bool showErrorMessage = true) where T : INode
    {
        int startingIndex = index;
        INode node = parser();
        hasFailed = false;
        if (!showErrorMessage) { Errors.Clean(); }

        if (node is T) { aux = (T)node; return true; }
        else { aux = default; index = startingIndex; return false; }
    }
    private static IReference ParseExpressions()
    {
        List<Func<IReference>> expressionParsers = new List<Func<IReference>>() { ParseComparisonExpression, ParseBooleanExpression, ParseArithmeticExpression, ParseStringExpression };
        int startingIndex = index;
        foreach (Func<IReference> parser in expressionParsers)
        {
            Debug.Log("Trying parsing the expression: " + parser.Method.Name + " in token: " + Current);
            IReference expressionResultant = parser();
            if (expressionResultant != null && !hasFailed) { Debug.Log("Success!!"); return expressionResultant; }
            hasFailed = false;
            index = startingIndex;
        }
        hasFailed = true; return null;
    }
}