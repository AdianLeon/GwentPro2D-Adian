using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Executer : MonoBehaviour
{
    public GameObject errorScreen;
    public static bool FailedAtLoadingAnyEffect => failedAtLoadingAnyEffect;
    private static bool failedAtLoadingAnyEffect = false;
    private static Dictionary<string, EffectDeclaration> createdEffects;
    public static VariableScopes scopes;
    public static void LoadEffectsAndCards()
    {
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
            EffectDeclaration effectDeclaration = EffectParser.ProcessCode(codeEffect);//Convierte el string en json a un objeto 
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
                Type effectType = Type.GetType(effectCall.EffectName);
                ICardEffect effectScript = (ICardEffect)card.GetComponent(effectType);
                effectScript.TriggerEffect();
            }
            else if (effectCall is CreatedEffectCall)
            {
                if (!createdEffects.ContainsKey(effectCall.EffectName)) { return; }
                List<DraggableCard> targets = SelectTargets(((CreatedEffectCall)effectCall).EffectSelector);
                ExecuteEffect((CreatedEffectCall)effectCall, targets);
            }
            else { throw new NotImplementedException("El EffectCall no es ScriptEffectCall ni CreatedEffectCall"); }
        }
    }
    private static void ExecuteEffect(CreatedEffectCall effectCall, List<DraggableCard> targets)
    {
        scopes = new VariableScopes();
        scopes.AddNewScope();
        scopes.AddNewVar("targets", new CardReferenceList(targets));
        ProcessActionStatements(createdEffects[effectCall.EffectName].EffectAction.ActionStatements);
    }
    private static void ProcessActionStatements(IEnumerable<IActionStatement> actionStatements)
    {
        foreach (IActionStatement actionStatement in actionStatements)
        {
            if (actionStatement is VariableDeclaration) { scopes.AddNewVar((VariableDeclaration)actionStatement); }
            else if (actionStatement is PrintAction) { UserRead.Write((actionStatement as PrintAction).Message); }
            else if (actionStatement is ContextMethod) { ContextUtils.AssignMethod((ContextMethod)actionStatement); }
            else if (actionStatement is CardPowerSetting) { SetPower((CardPowerSetting)actionStatement); }
            else if (actionStatement is ForEachCycle)
            {
                ForEachCycle forEachCycle = (ForEachCycle)actionStatement;
                List<DraggableCard> cards;
                if (forEachCycle.CardReferences is VariableReference)
                {
                    IReference reference = forEachCycle.CardReferences;
                    while (reference is VariableReference) { reference = scopes.GetValue(((VariableReference)reference).VarName); }
                    if (reference is not CardReferenceList) { throw new Exception("La variable llamada: '" + ((VariableReference)forEachCycle.CardReferences).VarName + "' no contiene una CardReferenceList"); }
                    cards = ((CardReferenceList)reference).Cards;
                }
                else { throw new NotImplementedException("No se ha anadido la forma de evaluar demandada"); }
                scopes.AddNewScope();
                foreach (DraggableCard card in cards)
                {
                    scopes.AddNewVar(forEachCycle.IteratorVarName, new CardReference(card));
                    ProcessActionStatements(forEachCycle.ActionStatements);
                }
                scopes.PopLastScope();
            }
            else { throw new Exception("La accion no esta definida"); }
        }
    }

    private static void SetPower(CardPowerSetting actionStatement)
    {
        IReference reference = actionStatement.CardReference;
        Debug.Log("Antes del while");
        while (reference is VariableReference) { Debug.Log("Obteniendo el valor de: " + ((VariableReference)reference).VarName); reference = scopes.GetValue(((VariableReference)reference).VarName); }
        Debug.Log("Despues del while");
        Debug.Log("reference is VarReference: " + (reference is VariableReference));
        if (reference is CardReference) { ((CardReference)reference).Power = actionStatement.NewPower; }
        else { throw new Exception("La referencia no era hacia una carta"); }
    }

    private static List<DraggableCard> SelectTargets(EffectSelector selector)
    {
        if (selector == null) { throw new Exception("El selector no existe"); }
        List<DraggableCard> cards;
        switch (selector.Source)
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
        //Filter with predicate
        //if(selector.CardPredicate!=null){foreach(DraggableCard card in cards){if(!selector.CardPredicate.EvaluateCard(new CardReference(card))){cards.Remove(card);}}}
        if (cards.Count == 0) { return cards; }
        if (selector.Single) { return new List<DraggableCard> { cards[0] }; }
        return cards;
    }
}
