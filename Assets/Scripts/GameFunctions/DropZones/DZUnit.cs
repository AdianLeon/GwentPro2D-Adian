using System;
//Script para las DropZones de cartas de unidad
public class DZUnit : DropZone
{
    public UnitDropZoneType GetValidZone => (UnitDropZoneType)Enum.Parse(typeof(UnitDropZoneType), name[0].ToString());
    public override bool IsDropValid(DraggableCard card) => card.GetComponent<UnitCard>() != null && gameObject.Field() == card.GetComponent<DraggableCard>().Owner && card.GetComponent<UnitCard>().Range.ToString().Contains(GetValidZone.ToString());
}
