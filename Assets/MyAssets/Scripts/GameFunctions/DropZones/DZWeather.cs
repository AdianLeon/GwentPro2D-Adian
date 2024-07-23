//Script para las DropZones de cartas de clima
public class DZWeather : DropZone
{
    public DZUnit Target1;//Primer objetivo que el clima afecta
    public DZUnit Target2;//Segundo objetivo que el clima afecta
    public override bool IsDropValid(DraggableCard card)
    {
        if (card.GetComponent<WeatherZoneCard>() == null)
        {//Si la carta no es de zona de clima
            return false;
        }
        return true;
    }
}
