using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            actionStatement = ContextActionParser();
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

    private IActionStatement ContextActionParser()
    {
        if (!Next().Is(".", true)) { hasFailed = true; return null; }
        Next();
        if (Current.Is("Board"))
        {
            ContainerReference container = new ContainerReference(Current.Text, new PlayerReference());
            if (!Next().Is(".", true)) { hasFailed = true; return null; }
            return AfterDotContextMethodParser(container);
        }
        else if (Current.Is("HandOfPlayer") || Current.Is("DeckOfPlayer") || Current.Is("FieldOfPlayer") || Current.Is("GraveyardOfPlayer"))
        {
            string containerName = Current.Text.Substring(8, Current.Text.Length - 8);
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            PlayerReference player = ContextPlayerParser();
            if (hasFailed) { return null; }
            if (!Next().Is(")", true)) { hasFailed = true; return null; }
            if (!Next().Is(".", true)) { hasFailed = true; return null; }
            ContainerReference container = new ContainerReference(containerName, player);
            return AfterDotContextMethodParser(container);
        }
        else if (Current.Is("Hand") || Current.Is("Deck") || Current.Is("Field") || Current.Is("Graveyard"))
        {
            ContainerReference container = new ContainerReference(Current.Text, new PlayerReference("Self"));
            if (!Next().Is(".", true)) { hasFailed = true; return null; }
            return AfterDotContextMethodParser(container);
        }
        else if (Current.Is("OtherHand") || Current.Is("OtherDeck") || Current.Is("OtherField") || Current.Is("OtherGraveyard"))
        {
            ContainerReference container = new ContainerReference(Current.Text.Substring(5), new PlayerReference("Other"));
            if (!Next().Is(".", true)) { hasFailed = true; return null; }
            return AfterDotContextMethodParser(container);
        }
        else { Errors.Write("No existe la propiedad del contexto: '" + Current.Text + "'"); hasFailed = true; return null; }
    }
    private PlayerReference ContextPlayerParser()
    {
        if (!Next().Is("context")) { hasFailed = true; return null; }
        if (!Next().Is(".")) { hasFailed = true; return null; }
        if (Next().Is("TriggerPlayer")) { return new PlayerReference("Self"); }
        else if (Next().Is("TriggerEnemy")) { return new PlayerReference("Other"); }
        else { Errors.Write("Se esperaba 'TriggerPlayer' o 'TriggerEnemy' en vez de '" + Current.Text + "'", Current); hasFailed = true; return null; }
    }
    private IActionStatement AfterDotContextMethodParser(ContainerReference container)
    {
        Next();
        if (Current.Is("Shuffle"))
        {
            if (!Next().Is("(", true) || !Next().Is(")", true)) { hasFailed = true; return null; }
            return new ContextShuffleMethod(container);
        }
        else { hasFailed = true; return null; }
    }
}
