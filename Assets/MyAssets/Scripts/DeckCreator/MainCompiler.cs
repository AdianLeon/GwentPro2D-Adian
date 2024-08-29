using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class MainCompiler
{
    public static void ProcessText(string allText)
    {
        Debug.Log("Compilacion iniciada");
        Errors.Clean();
        allText.Trim();//Elimina los espacios al principio y al final del texto
        List<Token> tokens = Lexer.TokenizeCode(allText);
        if (tokens == null) { Errors.Write("No se pudo tokenizar el codigo!"); return; }
        // Debug.Log("Resultado del lexer ----------------------------------------------------------------------"); foreach (Token token in tokens) { Debug.Log(token.ToString()); }; Debug.Log("Fin del resultado del lexer --------------------------------------------------------------");
        Parser.StartParsing(tokens);
        FullDeclaration fullDeclaration = (FullDeclaration)new FullDeclarationParser().ParseTokens();
        if (Parser.HasFailed) { Errors.Write("No se pudo parsear el codigo"); return; }
        Debug.Log("Compilacion terminada");
        SaveOnTxt(fullDeclaration, allText);
    }
    private static void SaveOnTxt(FullDeclaration fullDeclaration, string allText)
    {
        if (fullDeclaration == null) { return; }
        foreach (BlockDeclaration blockDeclaration in fullDeclaration.BlockDeclarations)
        {
            string fileName = blockDeclaration.Name + ".txt";
            string address = Application.dataPath + "/MyAssets/Database/";

            if (blockDeclaration is CardDeclaration) { address += "/Decks/" + (blockDeclaration as CardDeclaration).Faction + "/"; }
            else if (blockDeclaration is EffectDeclaration) { address += "/CreatedEffects/"; }

            int start = fullDeclaration.PositionsInCode.Dequeue();
            int end = fullDeclaration.PositionsInCode.Dequeue();
            string text = allText.Substring(start, end - start);
            Debug.Log("La blockDeclaration: " + text);
            if (!Directory.Exists(address)) { Directory.CreateDirectory(address); }
            File.WriteAllText(address + fileName, text);
        }
    }
}
