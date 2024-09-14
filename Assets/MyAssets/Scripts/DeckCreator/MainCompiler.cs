using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//Compilador principal, se llama cuando se desea compilar un archivo que contiene varias declaraciones de cartas y efectos
public static class MainCompiler
{
    public static void ProcessTextAndSave(string allText)
    {//Crea todas las cartas y efectos descritas y las guarda en formato txt
        Debug.Log("Compilacion iniciada");
        FullDeclaration fullDeclaration = ProcessText(allText);
        Debug.Log("Compilacion terminada");
        if (fullDeclaration != null) { SaveOnTxt(fullDeclaration, allText); }
    }
    public static FullDeclaration ProcessText(string allText)
    {//Devuelve el arbol de declaraciones de cartas y efectos
        Errors.Clean();//Limpia la pantalla que muestra los errores
        allText.Trim();//Elimina los espacios al principio y al final del texto
        List<Token> tokens = new Lexer(allText).TokenizeCode();
        if (tokens == null) { Errors.Write("No se pudo tokenizar el codigo!"); return null; }
        // Debug.Log("Resultado del lexer:"); tokens.ForEach(token => Debug.Log(token.ToString())); Debug.Log("Fin del resultado del lexer...");
        Parser parser = new Parser(tokens);
        FullDeclaration fullDeclaration = parser.ParseFullDeclaration();
        if (parser.HasFailed) { Errors.Write("No se pudo parsear el codigo"); return null; } else { Errors.PureWrite("El codigo se ha compilado exitosamente"); }
        return fullDeclaration;
    }
    private static void SaveOnTxt(FullDeclaration fullDeclaration, string allText)
    {//Guarda en archivos txt todos los efectos y cartas, seccionando un solo archivo que los contiene todos por las posiciones donde comienzan y terminan dichas declaraciones
        if (fullDeclaration == null) { return; }
        foreach (BlockDeclaration blockDeclaration in fullDeclaration.BlockDeclarations)
        {
            string fileName = blockDeclaration.Name + ".txt";
            string address = Application.dataPath + "/MyAssets/Database/";

            if (blockDeclaration is CardDeclaration) { address += "/Decks/" + (blockDeclaration as CardDeclaration).Faction + "/"; }
            else if (blockDeclaration is EffectDeclaration) { address += "/CreatedEffects/"; }

            int start = fullDeclaration.PositionsInCode.Dequeue();
            int end = fullDeclaration.PositionsInCode.Dequeue();
            string text = allText.Substring(start, end - start + 1);
            if (!Directory.Exists(address)) { Directory.CreateDirectory(address); }
            File.WriteAllText(address + fileName, text);
        }
    }
}
