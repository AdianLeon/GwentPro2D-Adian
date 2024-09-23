using System;
using System.Collections.Generic;
using UnityEngine;
//Script principal de la clase Parser distribuida por varios archivos
public partial class Parser
{
    public Parser(List<Token> tokensList) { tokens = tokensList; index = 0; hasFailed = false; }
    public bool HasFailed => hasFailed;
    private bool hasFailed;
    private List<Token> tokens;
    private int index;
    private Token Current => tokens[index];
    private Token Peek(int forward = 1) => tokens[index + forward];
    private Token Next(int forward = 1) { index += forward; return tokens[index]; }
    private bool Try<T>(Func<INode> parser, out T aux, bool showErrorMessage = true) where T : INode
    {
        int startingIndex = index;
        INode node = parser();
        hasFailed = false;
        if (!showErrorMessage) { Errors.Clean(); }

        if (node is T) { aux = (T)node; return true; }
        else { aux = default; index = startingIndex; return false; }
    }
    private IReference ParseExpressions()
    {
        if (hasFailed) { throw new Exception("Se llamo a parsear expresion pero ya se fallo anteriormente"); }
        List<Func<IReference>> expressionParsers = new List<Func<IReference>>() { ParseComparisonExpression, ParseBooleanExpression, ParseArithmeticExpression, ParseStringExpression };
        int startingIndex = index;
        foreach (Func<IReference> parser in expressionParsers)
        {
            // Debug.Log("Trying parsing the expression: " + parser.Method.Name + " in token: " + Current);
            IReference expressionResultant = parser();
            if (expressionResultant != null && !hasFailed && !Peek().Is(TokenType.operatorToken)) { return expressionResultant; }
            hasFailed = false;
            index = startingIndex;
        }
        hasFailed = true; return null;
    }
}