using System;
//Script para las DropZones de cartas de unidad
public class DZUnit : DropZone
{
    public UnitDropZoneType GetValidZone => (UnitDropZoneType)Enum.Parse(typeof(UnitDropZoneType), name[0].ToString());
    public override bool IsDropValid(DraggableCard card)
    {
        if (card.GetComponent<UnitCard>() == null) { return false; }//Si no es carta de unidad
        if (gameObject.Field() != card.GetComponent<DraggableCard>().Owner) { return false; }//Si los campos no coinciden
        if (!card.GetComponent<UnitCard>().WhichZone.ToString().Contains(GetValidZone.ToString())) { return false; }//Si esta zona no es una de las de la carta
        return true;
    }
}
