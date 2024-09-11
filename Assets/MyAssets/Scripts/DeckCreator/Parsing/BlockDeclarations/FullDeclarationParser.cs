using System.Collections.Generic;

public static partial class Parser
{
    public static FullDeclaration ParseFullDeclaration()
    {
        FullDeclaration fullDeclaration = new FullDeclaration();
        while (!Current.Is("$"))
        {
            fullDeclaration.PositionsInCode.Enqueue(Current.Position);
            if (Current.Is("card")) { fullDeclaration.BlockDeclarations.Add(ParseCard()); }
            else if (Current.Is("effect")) { fullDeclaration.BlockDeclarations.Add(ParseEffect()); }
            else { Errors.Write("Se esperaba 'card' o 'effect' en vez de: '" + Current.Text + "'", Current); hasFailed = true; return null; }
            if (hasFailed) { return null; }
            fullDeclaration.PositionsInCode.Enqueue(Current.Position);
            Next();
        }
        return fullDeclaration;
    }
}