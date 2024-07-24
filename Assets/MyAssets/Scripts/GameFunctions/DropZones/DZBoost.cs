//Script para las DropZones de cartas de aumento
public class DZBoost : DropZone
{
    public DZUnit Target;//Objetivo del efecto aumento
    public override bool IsDropValid(DraggableCard card)
    {
        if (card.GetComponent<BoostCard>() == null) { return false; }//Si la carta no es un aumento
        if (gameObject.Field() != card.GetComponent<DraggableCard>().WhichPlayer) { return false; }//Si es del jugador incorrecto
        return true;
    }
}
