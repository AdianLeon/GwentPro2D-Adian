using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using System.IO;

public static partial class Parser
{
    private static INode ParseOnActivation()
    {
        OnActivation onActivation = new OnActivation();
        if (!Current.Is("[", true)) { hasFailed = true; return null; }

        bool expectingEffectCall = true;
        while (expectingEffectCall)
        {
            onActivation.effectCalls.Add(ParseEffectCall(new EffectSelector(new StringValueExpression("board"), new BooleanValueExpression("false"), null), false));
            if (hasFailed) { return null; }
            if (!Next().Is("}", true)) { hasFailed = true; return null; }
            expectingEffectCall = Next().Is(",");
        }
        if (!Current.Is("]", true)) { hasFailed = true; return null; }
        return onActivation;
    }
    private static EffectCall ParseEffectCall(EffectSelector selector, bool isPostActionCalling)
    {
        if (!Next().Is("{", true)) { hasFailed = true; return null; }
        if (Peek().Is("ScriptEffect")) { return ParseScriptEffect(); }
        else
        {
            IExpression<string> effectName = null;
            List<(string, IReference)> parameters = null;
            EffectPostAction effectPostAction = null;
            bool expectingEffectProperty = true;
            while (expectingEffectProperty)
            {
                if (Next().Is("Effect"))
                {
                    if (effectName != null) { Errors.Write("El nombre del efecto ya ha sido declarado", Current); hasFailed = true; return null; }
                    if (!Next().Is(":", true)) { hasFailed = true; return null; }
                    if (Next().Is(TokenType.literal))
                    {
                        effectName = ParseStringExpression();
                        if (hasFailed) { return null; }
                        List<(string, VarType)> demandedParameters = GetDemandedParameters(effectName.Evaluate());
                        if (demandedParameters != null && demandedParameters.Count > 0)
                        {
                            Errors.Write("Existen parametros para el efecto: '" + effectName.Evaluate() + "', debes declarar los siguientes: " + demandedParameters.Select(parameter => parameter.Item1).FlattenText(), Current);
                            hasFailed = true; return null;
                        }
                    }
                    else if (Current.Is("{"))
                    {
                        if (!Next().Is("Name", true)) { hasFailed = true; return null; }
                        if (!Next().Is(":", true)) { hasFailed = true; return null; }
                        Next();
                        effectName = ParseStringExpression();
                        if (hasFailed) { return null; }
                        List<(string, VarType)> demandedParameters = GetDemandedParameters(effectName.Evaluate());
                        if (!Next().Is("}") && !Current.Is(",")) { Errors.Write("Se esperaba '}' o ',' (para declarar parametros)", Current); hasFailed = true; return null; }
                        else if (Current.Is(","))
                        {
                            if (parameters != null) { Errors.Write("Los parametros del efecto ya han sido declarados", Current); hasFailed = true; return null; }
                            parameters = new List<(string, IReference)>();
                            List<string> allDeclaredParameters = demandedParameters.Select(parameter => parameter.Item1).ToList();
                            bool expectingParameterDeclaration = true;
                            while (expectingParameterDeclaration)
                            {
                                parameters.Add(ParseParameterCall(allDeclaredParameters, demandedParameters, effectName.Evaluate()));
                                expectingParameterDeclaration = Next().Is(",");
                            }
                            if (!Current.Is("}", true)) { Errors.Write("Puede ser que hayas olvidado el token ',' entre declaraciones"); hasFailed = true; return null; }
                            if (allDeclaredParameters.Count > 0) { Errors.Write("Quedaron por declarar los siguientes parametros para el efecto: '" + effectName.Evaluate() + "': " + demandedParameters.Select(parameter => parameter.Item1).FlattenText(), Current); hasFailed = true; return null; }
                        }
                        else if (demandedParameters.Count > 0)
                        {
                            Errors.Write("Existen parametros para el efecto: '" + effectName.Evaluate() + "', debes declarar los siguientes: " + demandedParameters.Select(parameter => parameter.Item1).FlattenText(), Current);
                            hasFailed = true; return null;
                        }
                    }
                    else { Errors.Write("Se esperaba el nombre del efecto o '{'", Current); hasFailed = true; return null; }
                }
                else if (Current.Is("Selector"))
                {
                    if (!Next().Is(":", true)) { hasFailed = true; return null; }
                    if (!Next().Is("{", true)) { hasFailed = true; return null; }
                    bool expectingSelectorProperty = true;
                    bool declaredSourceProperty = false;
                    bool declaredSingleProperty = false;
                    bool declaredCardPredicate = false;
                    while (expectingSelectorProperty)
                    {
                        if (Next().Is("Source"))
                        {
                            if (!Next().Is(":", true)) { hasFailed = true; return null; }
                            if (Next().Is("board") || Current.Is("hand") || Current.Is("otherHand") || Current.Is("deck") || Current.Is("otherDeck") || Current.Is("field") || Current.Is("otherField") || (Current.Is("parent") && isPostActionCalling))
                            {
                                if (declaredSourceProperty) { Errors.Write("La propiedad 'Source' ya ha sido declarada"); hasFailed = true; return null; }
                                selector.Source = ParseStringExpression();
                                if (hasFailed) { return null; }
                                declaredSourceProperty = true;
                            }
                            else { Errors.Write("El 'Source' no es valido. Intenta con: 'board', 'hand', 'otherHand', 'deck', 'otherDeck', 'field' u 'otherField'", Current); hasFailed = true; return null; }
                        }
                        else if (Current.Is("Single"))
                        {
                            if (declaredSingleProperty) { Errors.Write("La propiedad 'Single' ya ha sido declarada"); hasFailed = true; return null; }
                            if (!Next().Is(":", true)) { hasFailed = true; return null; }
                            Next();
                            if (!Try(ParseExpressions, out selector.Single)) { Errors.Write("Se esperaba una expresion booleana", Current); hasFailed = true; return null; }
                            if (hasFailed) { return null; }
                            declaredSingleProperty = true;
                        }
                        else if (Current.Is("Predicate"))
                        {
                            if (declaredCardPredicate) { Errors.Write("La propiedad 'Predicate' ya ha sido declarada"); hasFailed = true; return null; }
                            if (!Next().Is(":", true)) { hasFailed = true; return null; }
                            selector.CardPredicate = ParseCardPredicate();
                            if (hasFailed) { return null; }
                            declaredCardPredicate = true;
                        }
                        else { Errors.Write("Se esperaba alguna de las propiedades del 'Selector': 'Source', 'Single' o 'Predicate'", Current); hasFailed = true; return null; }
                        expectingSelectorProperty = Next().Is(",");
                    }
                }
                else if (Current.Is("PostAction"))
                {
                    if (effectName == null || effectName.Evaluate().Trim() == "") { Errors.Write("El efecto padre del PostAction debe tener declarado al menos el nombre", Current); hasFailed = true; return null; }
                    if (!Next().Is(":", true)) { hasFailed = true; return null; }
                    EffectCall effectCall = ParseEffectCall(selector, true); if (hasFailed) { return null; }
                    if (effectCall is CreatedEffectCall) { effectPostAction = new EffectPostAction((CreatedEffectCall)effectCall, new CreatedEffectCall(effectName, parameters, selector, effectPostAction)); }
                    else { throw new Exception("No es valido crear un PostAction con un ScriptEffectCall"); }
                    if (!Next().Is("}", true)) { hasFailed = true; return null; }
                }
                else { Errors.Write("Se esperaba las declaraciones de propiedad de efecto 'Effect', 'Selector' o 'PostAction' en vez de '" + Current.Text + "'", Current); hasFailed = true; return null; }

                if (hasFailed) { return null; }
                expectingEffectProperty = Peek().Is(",");
                if (expectingEffectProperty) { Next(); }
            }
            if (effectName == null || effectName.Evaluate().Trim() == "") { Errors.Write("El efecto no tiene nombre", Current); hasFailed = true; return null; }
            return new CreatedEffectCall(effectName, parameters, selector, effectPostAction);
        }
    }

    private static (string, IReference) ParseParameterCall(List<string> allDeclaredParameters, List<(string, VarType)> demandedParameters, string effectName)
    {
        if (!Next().Is(TokenType.identifier, true)) { hasFailed = true; return default; }
        string parameterName = Current.Text;
        if (!allDeclaredParameters.Contains(parameterName)) { Errors.Write("El parametro '" + parameterName + "' no fue declarado en el efecto: '" + effectName + "' o ya fue declarado anteriormente"); hasFailed = true; return default; }
        allDeclaredParameters.Remove(parameterName);
        if (!Next().Is(":", true)) { hasFailed = true; return default; }
        VarType parameterType = demandedParameters.Single(parameter => parameter.Item1 == parameterName).Item2;
        IReference parameterValue;
        Next();
        if (parameterType == VarType.Number) { parameterValue = ParseArithmeticExpression(); }
        else if (parameterType == VarType.Bool) { parameterValue = ParseBooleanExpression(); }
        else if (parameterType == VarType.String) { parameterValue = ParseStringExpression(); }
        else { throw new Exception("La variable no contenia un tipo aceptable"); }
        return new(parameterName, parameterValue);
    }

    private static List<(string, VarType)> GetDemandedParameters(string effectName)
    {
        if (!File.Exists(Application.dataPath + "/MyAssets/Database/CreatedEffects/" + effectName + ".txt")) { Errors.Write("El efecto '" + effectName + "' no existe en la base de datos", Current); hasFailed = true; return null; }
        string effectCode = File.ReadAllText(Application.dataPath + "/MyAssets/Database/CreatedEffects/" + effectName + ".txt");
        ParsingOrder parsingOrder = new ParsingOrder(tokens, index, hasFailed);
        List<(string, VarType)> effectDeclaration = ProcessEffectCode(effectCode).Parameters;
        ResumeParsing(parsingOrder);
        return effectDeclaration;
    }

    private static CardPredicate ParseCardPredicate()
    {
        if (!Next().Is("(", true)) { hasFailed = true; return null; }
        if (!Next().Is(TokenType.identifier, true)) { hasFailed = true; return null; }
        string cardParameterName = Current.Text;
        bool wasEmpty = false;
        if (VariableScopes.IsEmpty()) { VariableScopes.AddNewScope(); wasEmpty = true; }
        VariableScopes.AddNewVar(cardParameterName, new FutureReference(VarType.Card));
        if (!Next().Is(")", true)) { hasFailed = true; return null; }
        if (!Next().Is("=>", true)) { hasFailed = true; return null; }
        Next();
        IExpression<bool> filter;
        if (!Try(ParseExpressions, out filter)) { Errors.Write("Se esperaba una expresion booleana", Current); hasFailed = true; return null; }
        if (wasEmpty) { VariableScopes.PopLastScope(); }
        return new CardPredicate(cardParameterName, filter);
    }
    private static ScriptEffectCall ParseScriptEffect()
    {
        if (!Next().Is("ScriptEffect")) { hasFailed = true; return null; }
        if (!Next().Is(":", true)) { hasFailed = true; return null; }
        Next();
        IExpression<string> scriptEffectName = ParseStringExpression();
        if (hasFailed) { return null; }
        IEnumerable<string> allTypesNames = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(ICardEffect).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).Select(type => type.Name);
        if (!allTypesNames.Contains(scriptEffectName.Evaluate())) { Errors.Write("El efecto de script indicado no existe", Current); hasFailed = true; return null; }
        return new ScriptEffectCall(scriptEffectName);
    }
}