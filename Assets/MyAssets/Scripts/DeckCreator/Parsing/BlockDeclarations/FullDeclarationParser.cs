using System.Collections.Generic;

public class FullDeclarationParser : Parser
{
    public override INode ParseTokens()
    {
        FullDeclaration fullDeclaration = new FullDeclaration();
        fullDeclaration.PositionsInCode.Enqueue(Current.Position);
        while (!Current.Is("$"))
        {
            if (!Current.Is(TokenType.blockDeclaration)) { Errors.Write("Se esperaba 'card' o 'effect'", Current); hasFailed = true; return null; }
            else if (Current.Is("card")) { fullDeclaration.BlockDeclarations.Add((BlockDeclaration)new CardParser().ParseTokens()); }
            else if (Current.Is("effect")) { fullDeclaration.BlockDeclarations.Add((BlockDeclaration)new EffectParser().ParseTokens()); }
            if (hasFailed) { return null; }
            fullDeclaration.PositionsInCode.Enqueue(Current.Position);
        }
        return fullDeclaration;
    }
}