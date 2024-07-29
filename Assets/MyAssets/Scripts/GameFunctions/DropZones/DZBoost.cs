//Script para las DropZones de cartas de aumento
public class DZBoost : DropZone
{
    public DZUnit Target;//Objetivo del efecto aumento
    public override bool IsDropValid(DraggableCard card) => card.GetComponent<BoostCard>() != null && gameObject.Field() == card.GetComponent<DraggableCard>().Owner;
}
