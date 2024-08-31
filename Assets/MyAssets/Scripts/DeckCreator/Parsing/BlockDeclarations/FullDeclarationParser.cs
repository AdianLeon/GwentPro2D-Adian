using System.Collections.Generic;

public class FullDeclarationParser : Parser
{
    public override INode ParseTokens()
    {
        FullDeclaration fullDeclaration = new FullDeclaration();
        while (!Current.Is("$"))
        {
            fullDeclaration.PositionsInCode.Enqueue(Current.Position);
            if (Current.Is("card")) { fullDeclaration.BlockDeclarations.Add((BlockDeclaration)new CardParser().ParseTokens()); }
            else if (Current.Is("effect")) { fullDeclaration.BlockDeclarations.Add((BlockDeclaration)new EffectParser().ParseTokens()); }
            else { Errors.Write("Se esperaba 'card' o 'effect' en vez de: '" + Current.Text + "'", Current); hasFailed = true; return null; }
            if (hasFailed) { return null; }
            fullDeclaration.PositionsInCode.Enqueue(Current.Position);
            Next();
        }
        return fullDeclaration;
    }
}