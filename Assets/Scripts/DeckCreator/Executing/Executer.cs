using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
//Script para cargar efectos y cartas y ejecutar efectos cuando se requiera 
public class Executer : MonoBehaviour
{
    public GameObject errorScreen;//Referencia a el objeto que mostrara los errores de compilacion o de ejecucion
    public static GameObject ErrorScreen => GameObject.Find("Canvas").GetComponent<Executer>().errorScreen;
    public static void RaiseError(string errorMessage) { ErrorScreen.SetActive(true); Errors.Write(errorMessage); }//Activa la pantalla de errores y muestra el error
    public static bool FailedAtLoadingAnyEffect => failedAtLoadingAnyEffect;
    private static bool failedAtLoadingAnyEffect;
    private static Dictionary<string, EffectDeclaration> createdEffects;//Diccionario donde se guardaran todos los efectos compilados correctamente
    public static void LoadEffectsAndCards()
    {//Carga las cartas y efectos, este metodo es llamado por el StateManager tan pronto se carga la escena
        failedAtLoadingAnyEffect = false;
        GameObject.Find("Canvas").GetComponent<Executer>().LoadEffects();
        GameObject.Find("Canvas").GetComponent<CardLoader>().LoadCards(createdEffects.Keys);
    }
    public void LoadEffects()
    {//Carga los efectos
        createdEffects = new Dictionary<string, EffectDeclaration>();
        string[] addressesOfEffects = Directory.GetFiles(Application.persistentDataPath + "/CreatedEffects", "*.txt");//Obtiene dentro del directorio del deck solo la direccion de los archivos con extension txt (ignora los meta)

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
    {//Ejecuta el OnActivation de una carta en particular o sea realiza las acciones descritas por los efectos de la carta
        if (card.OnActivation == null) { return; }
        if (card.OnActivation.effectCalls.Any(effectCall => effectCall == null)) { throw new Exception("Habia una llamada a de efecto invalida en el OnActivation de la carta: " + card.CardName); }
        card.OnActivation.effectCalls.ForEach(effectCall => ExecuteEffectCall(card, effectCall));
    }
    private static void ExecuteEffectCall(Card card, EffectCall effectCall)
    {//Ejecuta una llamada a efecto
        if (effectCall is ScriptEffectCall)
        {//Si es una llamada a efecto de script, se obtiene ese script en la carta y se activa su efecto
            Type effectType = Type.GetType(effectCall.EffectName.Evaluate());
            ICardEffect effectScript = (ICardEffect)card.GetComponent(effectType);
            effectScript.TriggerEffect();
        }
        else if (effectCall is CreatedEffectCall)
        {//Si es una llamada a un efecto creado se chequea que se haya cargado correctamente y se ejecuta
            if (!createdEffects.ContainsKey(effectCall.EffectName.Evaluate())) { return; }
            ExecuteCreatedEffect(card, (CreatedEffectCall)effectCall);
        }
        else { throw new NotImplementedException("El EffectCall no es ScriptEffectCall ni CreatedEffectCall"); }
    }
    private static void ExecuteCreatedEffect(Card card, CreatedEffectCall effectCall)
    {//Ejecuta un efecto creado
        List<DraggableCard> targets = SelectTargets(effectCall);//Selecciona los targets del efecto
        VariableScopes.Reset();//Inicia las variables y parametros a los que se debe tener acceso
        VariableScopes.AddNewVar("targets", new CardReferenceList(targets));
        if (effectCall.Parameters != null) { effectCall.Parameters.ForEach(parameter => VariableScopes.AddNewVar(parameter.Item1, parameter.Item2)); }

        createdEffects[effectCall.EffectName.Evaluate()].EffectAction.ActionStatements.ForEach(action => action.PerformAction());//Ejecuta una por una las acciones descritas en el Action

        if (effectCall.EffectPostAction != null) { ExecuteEffectCall(card, effectCall.EffectPostAction); }//Si el efecto tiene PostAction, lo ejecuta
    }
    private static List<DraggableCard> SelectTargets(CreatedEffectCall effectCall)
    {//Selecciona las cartas objetivo del efecto
        if (effectCall.EffectSelector == null) { throw new Exception("El selector no existe"); }
        List<DraggableCard> cards;
        //Source
        switch (effectCall.EffectSelector.Source.Evaluate())
        {//Devuelve todas las cartas de la fuente seleccionada
            case "board": cards = Field.PlayedFieldCards.Cast<DraggableCard>().Randomize().ToList(); break;
            case "field": cards = Field.PlayerCards.Cast<DraggableCard>().ToList(); break;
            case "otherField": cards = Field.EnemyCards.Cast<DraggableCard>().ToList(); break;
            case "hand": cards = Hand.PlayerCards.ToList(); break;
            case "otherHand": cards = Hand.EnemyCards.ToList(); break;
            case "deck": cards = Deck.PlayerCards.ToList(); break;
            case "otherDeck": cards = Deck.EnemyCards.ToList(); break;
            case "parent": if (effectCall is EffectPostAction) { cards = SelectTargets(((EffectPostAction)effectCall).Parent); } else { throw new Exception("Se uso la fuente 'parent' sin ser PostAction"); }; break;
            default: throw new NotImplementedException();
        }
        //Predicate
        if (effectCall.EffectSelector.CardPredicate != null)
        {//Filtra las cartas segun el predicado
            cards = cards.Where(card => effectCall.EffectSelector.CardPredicate.EvaluateCard(new CardReference(card))).ToList();
        }
        if (cards.Count == 0) { return new List<DraggableCard>(); }
        //Single
        if (effectCall.EffectSelector.Single.Evaluate()) { return new List<DraggableCard> { cards[0] }; }//Devuelve el primer elemento si se activo la propiedad Single
        return cards;
    }
}
