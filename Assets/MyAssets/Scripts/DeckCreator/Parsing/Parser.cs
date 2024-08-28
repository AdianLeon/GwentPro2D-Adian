using System.Collections.Generic;
using System.Linq;

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
}