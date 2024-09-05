using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArithmeticExpressionsParser : Parser
{//(2+4)*2/3+6-5*4+1==-9
    public override INode ParseTokens() => ParseSum();
    private IExpression<int> ParseSum()
    {
        IExpression<int> left = ParseMultiplication(); if (hasFailed) { return null; }
        while (Current.Is("+") || Current.Is("-"))
        {
            Token op = Current; Next();
            var right = ParseMultiplication(); if (hasFailed) { return null; }
            left = new ArithmeticExpression(left, op, right);
        }
        return left;
    }
    private IExpression<int> ParseMultiplication()
    {
        IExpression<int> left = ParseNumber(); if (hasFailed) { return null; }
        while (Current.Is("*") || Current.Is("/"))
        {
            Token op = Current; Next();
            IExpression<int> right = ParseNumber(); if (hasFailed) { return null; }
            left = new ArithmeticExpression(left, op, right);
        }
        return left;
    }
    private IExpression<int> ParseNumber()
    {
        IExpression<int> left;
        if (Current.Is("-") && Peek().Is(TokenType.number)) { left = new NumberExpression(Current.Text + Next().Text); Next(); }
        else if (Current.Is(TokenType.number)) { left = new NumberExpression(Current.Text); Next(); }
        else if (Current.Is("("))
        {
            Next();
            left = ParseSum(); if (hasFailed) { return null; }
            if (!Current.Is(")", true)) { hasFailed = true; return null; }
            Next();
        }
        else { Errors.Write(Current); hasFailed = true; return null; }

        while (Current.Is("^"))
        {
            Token op = Current; Next();
            IExpression<int> right = ParseNumber(); if (hasFailed) { return null; }
            left = new ArithmeticExpression(left, op, right);
        }
        return left;
    }
}