using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EffectActionParser : Parser
{
    public override INode ParseTokens()
    {
        EffectAction effectAction = new EffectAction();
        if (!Current.Is("(", true)) { hasFailed = true; }
        if (!Next().Is("targets", true)) { hasFailed = true; }
        if (!Next().Is(",", true)) { hasFailed = true; }
        if (!Next().Is("context", true)) { hasFailed = true; }
        if (!Next().Is(")", true)) { hasFailed = true; }
        if (!Next().Is("=>", true)) { hasFailed = true; }
        if (!Next().Is("{", true)) { hasFailed = true; }
        if (hasFailed) { return null; }
        while (!Next().Is("}"))
        {
            effectAction.ActionStatements.Add(ActionStatementParser());
            if (hasFailed) { Debug.Log("El parseo de actionStatement ha fallado"); return null; }
        }
        return effectAction;
    }
    private IActionStatement ActionStatementParser()
    {//Parsea una linea de codigo dentro del Action: (targets, contexts) => {...}
        IActionStatement actionStatement = null;
        if (Current.Is("context"))
        {
            if (!Next().Is(".", true)) { hasFailed = true; return null; }
            Next();
            if (Current.Is("Board") || Current.Is("Hand") || Current.Is("Deck") || Current.Is("Field") || Current.Is("Graveyard"))
            {
                ContainerReference.ContainerToGet type = (ContainerReference.ContainerToGet)Enum.Parse(typeof(ContainerReference.ContainerToGet), Current.Text);
                PlayerReference.PlayerToGet playerToGet = PlayerReference.PlayerToGet.Self;
                if (Current.Is("Board")) { playerToGet = PlayerReference.PlayerToGet.None; }
                PlayerReference player = new PlayerReference(playerToGet);
                ContainerReference container = new ContainerReference(type, player);
                if (!Next().Is(".", true)) { hasFailed = true; return null; }
                Next();
                if (Current.Is("Shuffle"))
                {
                    if (!Next().Is("(", true) || !Next().Is(")", true)) { hasFailed = true; return null; }
                    actionStatement = new ContextShuffleMethod(container);
                }
            }
        }
        else if (Current.Is("Print"))
        {
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            if (!Next().Is(TokenType.literal, true)) { hasFailed = true; return null; }
            actionStatement = new PrintAction(Current.Text);
            if (!Next().Is(")", true)) { hasFailed = true; return null; }
        }
        else if (Current.Is(TokenType.identifier))
        {
            //Variables
            hasFailed = true; return null;
        }
        else { Errors.Write("Accion desconocida: '" + Current.Text + "'", Current); hasFailed = true; return null; }

        if (!Next().Is(";", true)) { hasFailed = true; return null; }

        if (actionStatement == null) { Errors.Write("No se pudo procesar una accion en la linea: " + Current.Line); hasFailed = true; }
        return actionStatement;
    }
}
