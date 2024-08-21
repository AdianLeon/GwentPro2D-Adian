using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EffectActionParser : Parser
{
    public override INode ParseTokens()
    {
        EffectAction effectAction = new EffectAction();
        if (!Current.Is("(", true)) { hasFailed = true; return null; }
        if (!Next().Is("targets", true)) { hasFailed = true; return null; }
        if (!Next().Is(",", true)) { hasFailed = true; return null; }
        if (!Next().Is("context", true)) { hasFailed = true; return null; }
        if (!Next().Is(")", true)) { hasFailed = true; return null; }
        if (!Next().Is("=>", true)) { hasFailed = true; return null; }
        if (!Next().Is("{", true)) { hasFailed = true; return null; }
        while (!Next().Is("}"))
        {
            if (Current.Is("Print"))
            {
                if (!Next().Is("(", true)) { hasFailed = true; return null; }
                if (!Next().Is(TokenType.literal, true)) { hasFailed = true; return null; }
                effectAction.ActionStatements.Add(new PrintAction(Current.Text));
                if (!Next().Is(")", true)) { hasFailed = true; return null; }
            }
            else
            {
                Errors.Write("Accion desconocida: '" + Current.Text + "'", Current);
            }
            if (!Next().Is(";", true)) { hasFailed = true; return null; }
        }
        return effectAction;
    }
}
