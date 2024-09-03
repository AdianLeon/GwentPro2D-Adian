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
            if (Peek().Is("ScriptEffect")) { onActivation.effectCalls.Add(ParseScriptEffect()); }
            else
            {
                string effectName = null;
                string source = null;
                bool expectingEffectProperty = true;
                while (expectingEffectProperty)
                {
                    if (Next().Is("Effect"))
                    {
                        if (effectName != null) { Errors.Write("El nombre del efecto ya ha sido declarado", Current); hasFailed = true; return null; }
                        if (!Next().Is(":", true)) { hasFailed = true; return null; }
                        if (!Next().Is(TokenType.literal, true)) { hasFailed = true; return null; }
                        effectName = Current.Text;
                    }
                    else if (Current.Is("Selector"))
                    {
                        if (!Next().Is(":", true)) { hasFailed = true; return null; }
                        if (!Next().Is("{", true)) { hasFailed = true; return null; }
                        bool expectingSelectorProperty = true;
                        while (expectingSelectorProperty)
                        {
                            if (Next().Is("Source"))
                            {
                                if (!Next().Is(":", true)) { hasFailed = true; return null; }
                                if (Next().Is("board") || Current.Is("hand") || Current.Is("otherHand") || Current.Is("deck") || Current.Is("otherDeck") || Current.Is("field") || Current.Is("otherField"))
                                {
                                    if (source == null) { source = Current.Text; } else { Errors.Write("La propiedad 'Source' ya ha sido declarada"); hasFailed = true; return null; }
                                }
                                else { Errors.Write("El 'Source' no es valido. Intenta con: 'board', 'hand', 'otherHand', 'deck', 'otherDeck', 'field' u 'otherField'", Current); hasFailed = true; return null; }
                            }
                            else { Errors.Write("Se esperaba alguna de las propiedades del 'Selector': 'Source', (...)", Current); hasFailed = true; return null; }
                            expectingSelectorProperty = Next().Is(",");
                        }
                    }
                    else { Errors.Write("Se esperaba las declaraciones de propiedad de efecto 'Effect', 'Selector' o 'PostAction' en vez de '" + Current.Text + "'", Current); hasFailed = true; return null; }

                    if (hasFailed) { return null; }
                    expectingEffectProperty = Peek().Is(",");
                    if (expectingEffectProperty) { Next(); }
                }
                if (effectName == null) { Errors.Write("El efecto no tiene nombre", Current); hasFailed = true; return null; }
                if (source == null) { source = "board"; }
                onActivation.effectCalls.Add(new CreatedEffectCall(effectName, new EffectSelector(source)));
            }
            if (!Next().Is("}", true)) { hasFailed = true; return null; }
            expectingEffectCall = Next().Is(",");
        }
        if (!Current.Is("]", true)) { hasFailed = true; return null; }
        return onActivation;
    }
    private ScriptEffectCall ParseScriptEffect()
    {
        if (!Next().Is("ScriptEffect")) { hasFailed = true; return null; }
        if (!Next().Is(":", true)) { hasFailed = true; return null; }
        if (!Next().Is(TokenType.literal, true)) { hasFailed = true; return null; }

        IEnumerable<string> allTypesNames = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(ICardEffect).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).Select(type => type.Name);
        if (!allTypesNames.Contains(Current.Text)) { Errors.Write("El efecto de script indicado no existe", Current); hasFailed = true; return null; }
        return new ScriptEffectCall(Current.Text);
    }
}