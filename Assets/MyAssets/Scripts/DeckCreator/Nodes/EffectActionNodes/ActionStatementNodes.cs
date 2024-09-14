using System;
using System.Collections.Generic;
//Nodos de declaracion de accion
public interface IActionStatement : INode { public void PerformAction(); }//Cada declaracion de accion describe como ejecutar su accion
public class PrintAction : IActionStatement
{//Escribe el mensaje en el UserRead
    public IExpression<string> Message;
    public PrintAction(IExpression<string> message) { Message = message; }
    public void PerformAction() => UserRead.Write(Message.Evaluate());
}
public abstract class ContextMethod : IActionStatement
{//Clase abstracta para todos los metodos de accion del contexto
    public ContainerReference Container;
    public abstract void PerformAction();
}
public class ContextFindMethod : ContextMethod, IReference
{
    public VarType Type => VarType.CardList;
    public CardPredicate CardPredicate;
    public ContextFindMethod(ContainerReference container, CardPredicate cardPredicate) { Container = container; CardPredicate = cardPredicate; }
    public override void PerformAction() => ContextExecution.FindCards(this);
}
public class ContextCardParameterMethod : ContextMethod
{
    public string ActionType;
    public IReference Card;

    public ContextCardParameterMethod(ContainerReference container, string actionType, IReference card)
    {
        Container = container;
        ActionType = actionType;
        if (card.Type != VarType.Card) { throw new Exception("El tipo de parametro de un metodo de contexto con parametro carta no es carta"); }
        Card = card;
    }
    public override void PerformAction() => ContextExecution.DoActionForCardParameterMethod(this);
}
public class ContextPopMethod : ContextMethod, IReference
{
    public VarType Type => VarType.Card;
    public ContextPopMethod(ContainerReference container) { Container = container; }
    public override void PerformAction() => ContextExecution.PopContainer(this);
}
public class ContextShuffleMethod : ContextMethod
{
    public ContextShuffleMethod(ContainerReference container) { Container = container; }
    public override void PerformAction() => ContextExecution.ShuffleContainer(this);
}
public class CardPowerAlteration : IActionStatement
{
    public IReference CardReference;
    public string Operation;
    public IExpression<int> Right;
    public CardPowerAlteration(IReference cardReference, string operation)
    {
        if (cardReference.Type != VarType.Card) { throw new Exception("El tipo de parametro de un metodo de contexto con parametro carta no es carta"); }
        if (operation != "++" && operation != "--") { throw new Exception("Se debe usar el otro constructor para proporcionar la expresion derecha"); }
        CardReference = cardReference; Operation = operation;
    }
    public CardPowerAlteration(IReference cardReference, string operation, IExpression<int> right)
    {
        if (cardReference.Type != VarType.Card) { throw new Exception("El tipo de parametro de un metodo de contexto con parametro carta no es carta"); }
        CardReference = cardReference; Operation = operation; Right = right;
    }
    public void PerformAction()
    {
        IReference reference = CardReference.DeReference();
        if (reference is CardReference)
        {
            switch (Operation)
            {
                case "=": ((CardReference)reference).Power = Right.Evaluate(); break;
                case "+=": ((CardReference)reference).Power += Right.Evaluate(); break;
                case "-=": ((CardReference)reference).Power -= Right.Evaluate(); break;
                case "*=": ((CardReference)reference).Power *= Right.Evaluate(); break;
                case "/=": ((CardReference)reference).Power /= Right.Evaluate(); break;
                case "^=": Math.Pow(((CardReference)reference).Power, Right.Evaluate()); break;
                case "++": ((CardReference)reference).Power++; break;
                case "--": ((CardReference)reference).Power--; break;
                default: throw new NotImplementedException("Operacion no definida");
            }
        }
        else { throw new NotImplementedException(); }
    }
}
public class ForEachCycle : IActionStatement
{
    public string IteratorVarName;
    public IReference CardReferences;
    public List<IActionStatement> ActionStatements = new List<IActionStatement>();
    public ForEachCycle(string iteratorVarName, IReference cardReferences, List<IActionStatement> actionStatements) { IteratorVarName = iteratorVarName; CardReferences = cardReferences; ActionStatements = actionStatements; }
    public void PerformAction()
    {
        List<DraggableCard> cards;
        if (CardReferences is VariableReference)
        {
            IReference reference = CardReferences.DeReference();
            if (reference is not CardReferenceList) { throw new Exception("La variable llamada: '" + ((VariableReference)CardReferences).VarName + "' no contiene una CardReferenceList"); }
            cards = ((CardReferenceList)reference).Cards;
        }
        else if (CardReferences is ContextFindMethod)
        {
            cards = ContextExecution.FindCards((ContextFindMethod)CardReferences);
        }
        else { throw new NotImplementedException("No se ha anadido la forma de evaluar demandada"); }
        VariableScopes.AddNewScope();
        foreach (DraggableCard card in cards)
        {
            VariableScopes.AddNewVar(IteratorVarName, new CardReference(card));
            ActionStatements.ForEach(action => action.PerformAction());
        }
        VariableScopes.PopLastScope();
    }
}
public class WhileCycle : IActionStatement
{
    public IExpression<bool> Condition;
    public List<IActionStatement> ActionStatements = new List<IActionStatement>();
    public WhileCycle(IExpression<bool> condition, List<IActionStatement> actionStatements) { Condition = condition; ActionStatements = actionStatements; }
    public void PerformAction()
    {
        int count = 0; int limit = 100000;
        while (Condition.Evaluate() && ((++count) < limit)) { ActionStatements.ForEach(action => action.PerformAction()); }
        if (count >= limit) { throw new Exception("La cantidad de iteraciones de un ciclo while fue de: " + count + " lo cual no esta permitido"); }
    }
}
public class VariableAlteration : IActionStatement
{
    public string VarName;
    public Token Operation;
    public IExpression<int> Right;
    public void PerformAction()
    {
        if (VarName.ScopeValue() is not IExpression<int>) { throw new Exception("No es expresion aritmetica"); }
        IExpression<int> Left = (IExpression<int>)VarName.ScopeValue();
        IExpression<int> result = new ArithmeticExpression(Left, Operation.Text[0].ToString(), Right);
        VariableScopes.AddNewVar(VarName, result);
    }
    public VariableAlteration(string varName, Token operation, IExpression<int> right) { VarName = varName; Operation = operation; Right = right; }
}