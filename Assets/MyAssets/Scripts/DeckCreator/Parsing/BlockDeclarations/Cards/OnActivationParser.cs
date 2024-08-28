using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using UnityEditor.PackageManager;
//
public class OnActivationParser : Parser
{
    public override INode ParseTokens()
    {
        OnActivation onActivation = new OnActivation();
        if (!Current.Is("[", true)) { hasFailed = true; return null; }
        bool expectingEffectCall = true;
        while (expectingEffectCall)
        {
            if (!Next().Is("{", true)) { hasFailed = true; return null; }
            Next();
            if (Current.Is("Ignore")) { }
            else if (Current.Is("ScriptEffect")) { onActivation.effectCalls.Add(ParseScriptEffect()); }
            else if (Current.Is("Effect")) { onActivation.effectCalls.Add(ParseCreatedEffect()); }
            else { Errors.Write("Se esperaba las llamadas de efecto 'Effect' o 'ScriptEffect' en vez de '" + Current.Text + "'"); }

            if (hasFailed) { return null; }

            if (!Next().Is("}", true)) { hasFailed = true; return null; }

            expectingEffectCall = Next().Is(",");
        }
        if (!Current.Is("]", true)) { hasFailed = true; return null; }
        return onActivation;
    }
    private ScriptEffectCall ParseScriptEffect()
    {
        if (!Next().Is(":", true)) { hasFailed = true; return null; }
        if (!Next().Is(TokenType.literal, true)) { hasFailed = true; return null; }

        IEnumerable<string> allTypesNames = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(ICardEffect).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).Select(type => type.Name);
        if (!allTypesNames.Contains(Current.Text)) { Errors.Write("El efecto de script indicado no existe", Current); hasFailed = true; return null; }
        return new ScriptEffectCall(Current.Text);
    }
    private EffectCall ParseCreatedEffect()
    {
        if (!Next().Is(":", true)) { hasFailed = true; return null; }
        Next();
        if (Current.Is(TokenType.literal)) { return new CreatedEffectCall(Current.Text, null); }
        else { Errors.Write("Se esperaba un literal o el caracter '{'", Current); hasFailed = true; return null; }
    }
}