using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Executer : MonoBehaviour
{
    public GameObject errorScreen;
    public static GameObject ErrorScreen => GameObject.Find("Canvas").GetComponent<Executer>().errorScreen;
    public static bool FailedAtLoadingAnyEffect => failedAtLoadingAnyEffect;
    private static bool failedAtLoadingAnyEffect;
    private static Dictionary<string, EffectDeclaration> createdEffects;
    public static void LoadEffectsAndCards()
    {
        failedAtLoadingAnyEffect = false;
        GameObject.Find("Canvas").GetComponent<Executer>().LoadEffects();
        GameObject.Find("Canvas").GetComponent<CardLoader>().LoadCards(createdEffects.Keys);
    }
    public void LoadEffects()
    {
        createdEffects = new Dictionary<string, EffectDeclaration>();
        string[] addressesOfEffects = Directory.GetFiles(Application.dataPath + "/MyAssets/Database/CreatedEffects", "*.txt");//Obtiene dentro del directorio del deck solo la direccion de los archivos con extension txt (ignora los meta)

        errorScreen.SetActive(true);
        foreach (string address in addressesOfEffects)
        {//Para cada uno de los archivos con extension json
            string codeEffect = File.ReadAllText(address);//Lee el archivo
            EffectDeclaration effectDeclaration = Parser.ProcessEffectCode(codeEffect);//Convierte el string en json a un objeto 
            if (effectDeclaration != null) { createdEffects.Add(effectDeclaration.Name.Evaluate(), effectDeclaration); }
            else { Errors.Write("No se pudo procesar el texto del efecto en: " + address); failedAtLoadingAnyEffect = true; }
        }
    }
    public static void ExecuteOnActivation(Card card)
    {
        if (card.OnActivation == null) { return; }
        foreach (EffectCall effectCall in card.OnActivation.effectCalls)
        {
            if (effectCall == null) { return; }
            else if (effectCall is ScriptEffectCall)
            {
                Type effectType = Type.GetType(effectCall.EffectName.Evaluate());
                ICardEffect effectScript = (ICardEffect)card.GetComponent(effectType);
                effectScript.TriggerEffect();
            }
            else if (effectCall is CreatedEffectCall)
            {
                if (!createdEffects.ContainsKey(effectCall.EffectName.Evaluate())) { return; }
                List<DraggableCard> targets = SelectTargets(((CreatedEffectCall)effectCall).EffectSelector);
                ExecuteEffect((CreatedEffectCall)effectCall, targets);
            }
            else { throw new NotImplementedException("El EffectCall no es ScriptEffectCall ni CreatedEffectCall"); }
        }
    }
    private static void ExecuteEffect(CreatedEffectCall effectCall, List<DraggableCard> targets)
    {
        VariableScopes.Reset();
        VariableScopes.AddNewVar("targets", new CardReferenceList(targets));
        createdEffects[effectCall.EffectName.Evaluate()].EffectAction.ActionStatements.ForEach(action => action.PerformAction());
    }
    private static List<DraggableCard> SelectTargets(EffectSelector selector)
    {
        if (selector == null) { throw new Exception("El selector no existe"); }
        List<DraggableCard> cards;
        switch (selector.Source.Evaluate())
        {
            case "board": cards = Field.PlayedFieldCards.Cast<DraggableCard>().ToList(); break;
            case "field": cards = Field.PlayerCards.Cast<DraggableCard>().ToList(); break;
            case "otherField": cards = Field.EnemyCards.Cast<DraggableCard>().ToList(); break;
            case "hand": cards = Hand.PlayerCards.ToList(); break;
            case "otherHand": cards = Hand.EnemyCards.ToList(); break;
            case "deck": cards = Deck.PlayerCards.ToList(); break;
            case "otherDeck": cards = Deck.EnemyCards.ToList(); break;
            default: throw new NotImplementedException();
        }
        if (selector.CardPredicate != null) { foreach (DraggableCard card in cards) { if (!selector.CardPredicate.EvaluateCard(new CardReference(card))) { cards.Remove(card); } } }
        if (cards.Count == 0) { return new List<DraggableCard>(); }
        if (selector.Single.Evaluate()) { return new List<DraggableCard> { cards[0] }; }
        return cards;
    }
}
