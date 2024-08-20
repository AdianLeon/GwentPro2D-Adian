using System.Collections.Generic;

public class FullDeclarationParser : Parser
{
    public override INode ParseTokens()
    {
        FullDeclaration fullDeclaration = new FullDeclaration { blockDeclarations = new List<BlockDeclaration>(), positionsInCode = new Queue<int>() };
        fullDeclaration.positionsInCode.Enqueue(Current.position);
        while (!Current.Is("$"))
        {
            if (!Current.Is(TokenType.blockDeclaration)) { Errors.Write("Se esperaba 'card' o 'effect'", Current); hasFailed = true; return null; }
            else if (Current.Is("card")) { fullDeclaration.blockDeclarations.Add((BlockDeclaration)new CardParser().ParseTokens()); }
            else if (Current.Is("effect")) { fullDeclaration.blockDeclarations.Add((BlockDeclaration)new EffectsParser().ParseTokens()); }
            if (hasFailed) { return null; }
            fullDeclaration.positionsInCode.Enqueue(Current.position);
        }
        return fullDeclaration;
    }
}