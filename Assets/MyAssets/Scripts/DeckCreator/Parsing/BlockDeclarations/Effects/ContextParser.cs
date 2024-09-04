using System;

public class ContextParser : EffectActionParser
{
    public INode Parse()
    {//Parsea cualquier declaracion que sea de acceso al context
        if (!Current.Is("context", true)) { hasFailed = true; return null; }
        if (!Next().Is(".", true)) { hasFailed = true; return null; }

        if (Next().Is("Board") || Current.Text.Contains("Hand") || Current.Text.Contains("Deck") || Current.Text.Contains("Field") || Current.Text.Contains("Graveyard")) { return ContextContainerParser(); }
        else if (Current.Is("TriggerPlayer")) { return new PlayerReference("Self"); }
        else if (Current.Is("TriggerEnemy")) { return new PlayerReference("Other"); }
        else { Errors.Write("No existe la propiedad del contexto: '" + Current.Text + "'"); hasFailed = true; return null; }
    }
    public INode ContextContainerParser()
    {
        ContainerReference container;
        if (Current.Is("Board")) { container = new ContainerReference(Current.Text, new PlayerReference()); }
        else if (Current.Is("Hand") || Current.Is("Deck") || Current.Is("Field") || Current.Is("Graveyard")) { container = new ContainerReference(Current.Text, new PlayerReference("Self")); }
        else if (Current.Is("OtherHand") || Current.Is("OtherDeck") || Current.Is("OtherField") || Current.Is("OtherGraveyard")) { container = new ContainerReference(Current.Text.Substring(5), new PlayerReference("Other")); }
        else if (Current.Is("HandOfPlayer") || Current.Is("DeckOfPlayer") || Current.Is("FieldOfPlayer") || Current.Is("GraveyardOfPlayer"))
        {
            string containerName = Current.Text.Substring(0, Current.Text.Length - 8);
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            INode hopefullyPlayerReference;

            if (Next().Is("context")) { hopefullyPlayerReference = Parse(); }
            else if (Current.Is(TokenType.identifier))
            {
                if (scopes.ContainsVar(Current.Text)) { hopefullyPlayerReference = new VariableReference(Current.Text, scopes.GetValue(Current.Text).Type); }
                else { Errors.Write("La variable: '" + Current.Text + "' no existe en este contexto", Current); hasFailed = true; return null; }
            }
            else { hopefullyPlayerReference = null; }
            if (hopefullyPlayerReference is IReference && (hopefullyPlayerReference as IReference).Type == VarType.Player)
            {
                while (hopefullyPlayerReference is VariableReference) { hopefullyPlayerReference = scopes.GetValue((hopefullyPlayerReference as VariableReference).VarName); }
                if (hopefullyPlayerReference is not PlayerReference) { throw new System.Exception("Las referencias no apuntaron a un jugador valido"); }
                PlayerReference player = (PlayerReference)hopefullyPlayerReference;
                if (!Next().Is(")", true)) { hasFailed = true; return null; }
                container = new ContainerReference(containerName, player);
            }
            else { Errors.Write("Se esperaba una referencia a algun jugador", Current); hasFailed = true; return null; }
        }
        else { Errors.Write("No existe la propiedad del contexto: '" + Current.Text + "'", Current); hasFailed = true; return null; }

        if (Peek().Is(".")) { return ContextContainerMethodParser(container); }
        return container;
    }
    public IActionStatement ContextContainerMethodParser(ContainerReference container)
    {
        if (!Next().Is(".", true)) { hasFailed = true; return null; }

        if (Next().Is("Shuffle") || Current.Is("Pop"))
        {
            if (!Next().Is("(", true) || !Next().Is(")", true)) { hasFailed = true; return null; }
            if (Peek(-2).Is("Shuffle")) { return new ContextShuffleMethod(container); }
            else if (Peek(-2).Is("Pop")) { return new ContextPopMethod(container); }
            else { throw new NotImplementedException(); }
        }
        else if (Current.Is("Push") || Current.Is("SendBottom") || Current.Is("Remove"))
        {
            if (!Current.Is("Remove") && (container.ContainerName == "Board" || container.ContainerName == "Field"))
            { Errors.Write("El metodo '" + Current.Text + "' no esta disponible para el contenedor '" + container.ContainerName + "'", Current); hasFailed = true; return null; }

            Token methodToken = Current;
            if (!Next().Is("(", true)) { hasFailed = true; return null; }
            IReference cardReference;
            INode hopefullyCardReference = null;
            if (Next().Is("context")) { hopefullyCardReference = Parse(); }
            else if (Current.Is(TokenType.identifier))
            {
                if (scopes.ContainsVar(Current.Text)) { hopefullyCardReference = new VariableReference(Current.Text, scopes.GetValue(Current.Text).Type); }
                else { Errors.Write("La variable: '" + Current.Text + "' no existe en este contexto", Current); hasFailed = true; return null; }
            }
            if (hopefullyCardReference is IReference && ((IReference)hopefullyCardReference).Type == VarType.Card) { cardReference = (IReference)hopefullyCardReference; }
            else { Errors.Write("El parametro pasado a '" + methodToken.Text + "' no es una referencia a una carta"); hasFailed = true; return null; }
            if (!Next().Is(")", true)) { hasFailed = true; return null; }
            return new ContextCardParameterMethod(container, methodToken.Text, cardReference);
        }
        else { Errors.Write("El metodo del contenedor " + container.ContainerName + ": '" + Current.Text + "' no esta definido", Current); hasFailed = true; return null; }
    }
}
