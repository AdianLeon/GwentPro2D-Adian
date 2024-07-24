//Script para las DropZones de cartas de clima
public class DZWeather : DropZone
{
    public DZUnit TargetP1;//Primer objetivo que el clima afecta
    public DZUnit TargetP2;//Segundo objetivo que el clima afecta
    public override bool IsDropValid(DraggableCard card)
    {//El drop sera valido si la carta dropeada es de clima
        return card.GetComponent<WeatherZoneCard>() != null;
    }
}
