using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
//Script que contiene los comportamientos y propiedades en comun de todas las cartas
public abstract class Card : MonoBehaviour, IGlow, IShowZone, IPointerEnterHandler, IPointerExitHandler
{
    private static Card enteredCard;//En esta variable se guarda el objeto debajo del puntero el cual mostramos en CardView
    public static Card GetEnteredCard => enteredCard;
    public string Faction;//Faccion de la carta
    public string CardName;//Nombre de la carta
    public string OnActivationName;//Nombre de OnActivation
    public Sprite Artwork;//Imagen para mostrar en el CardView
    public Player WhichPlayer;//Jugador dueno de la carta
    public virtual Color GetCardViewColor => new Color(1, 1, 1);//Retorna el color de la carta en el CardView
    public virtual void LoadInfo()
    {//Esta funcion es especifica para cada tipo de carta, pero todas comparten lo siguiente
        GameObject.Find("Faction").GetComponent<TextMeshProUGUI>().text = Faction;
        GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text = CardName;
        GameObject.Find("BGType").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        //EffectDescription
        GameObject.Find("EffectDescription").GetComponent<TextMeshProUGUI>().text = "Esta carta no tiene efecto";
        /*if(OnActivation.EffectDescription!=""){
        }else*/
        if (GetComponent<ICardEffect>() != null)
        {
            GameObject.Find("EffectDescription").GetComponent<TextMeshProUGUI>().text = GetComponent<ICardEffect>().GetEffectDescription;
        }
        else if (GetComponent<ISpecialCard>() != null)
        {
            GameObject.Find("EffectDescription").GetComponent<TextMeshProUGUI>().text = GetComponent<ISpecialCard>().GetEffectDescription;
        }
        //Quality e Image
        GameObject.Find("Quality").GetComponent<Image>().sprite = Resources.Load<Sprite>("BlankImage");
        GameObject.Find("CardPreview").GetComponent<Image>().sprite = Artwork;

        //Colores
        GameObject.Find("BackGroundCard").GetComponent<Image>().color = GetCardViewColor;
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color = GetCardViewColor;
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color = GetCardViewColor;
    }
    public abstract bool IsPlayable { get; }//Conjunto de condiciones para que la carta se pueda jugar, diferente para todas las cartas
    public bool TryPlay()
    {//Juega la carta y devuelve si se cumplieron las condiciones para que se pudiese jugar
        if (!IsPlayable) { return false; }
        //Si la carta tiene efecto de carta especial, que se active
        gameObject.GetComponent<ISpecialCard>()?.TriggerSpecialEffect();
        Execute.DoEffect(gameObject, OnActivationName);//Se ejecuta el efecto en del OnActivation
        Judge.OnPlayedCard();//Notifica al juez de que se jugo una carta
        LoadInfo();//Carga la info de la carta luego de que todos los scripts han reaccionado al estado
        return true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {//Se activa cuando el mouse entra en la carta
        enteredCard = this;
        OffGlow();//La carta se sombrea cuando pasamos por encima
        LoadInfo();//Se carga toda la informacion de esta carta en el CardView
        TryShowZone();
    }
    public void OnPointerExit(PointerEventData eventData)
    {//Se llama cuando el mouse sale de la carta
        GFUtils.RestoreGlow();//Se dessombrean todas las cartas jugadas y se desactiva la iluminacion de todas las zonas
        OnGlow();//La carta se dessombrea
        enteredCard = null;//Ya no se esta encima de ninguna carta
    }
    public virtual void ShowZone()
    {
        //Todas las zonas que cumplen que el dropeo de esta carta es valido se iluminan
        GameObject.FindObjectsOfType<DropZone>().Where(zone => zone.IsDropValid(gameObject.GetComponent<DraggableCard>())).ForEach(zone => zone.OnGlow());
    }
    private void TryShowZone()
    {//Si el jugador puede jugar, tiene componente DraggableCard, no se esta arrastrando ninguna carta y ademas esta en la mano se iluminan las zonas donde la carta se puede soltar
        if (!Judge.CanPlay) { return; }//Si no se pude jugar
        if (enteredCard.GetComponent<DraggableCard>() == null) { return; }//Si no tiene componente DraggableCard
        if (DraggableCard.IsOnDrag) { return; }//Si se esta arrastrando
        if (!enteredCard.GetComponent<DraggableCard>().IsOnHand) { return; }//Si no esta en la mano
        ShowZone();
    }
    public void OnGlow() { gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1); }//Cuando una carta activa su glow se devuelve a su estado normal totalmente visible
    public void OffGlow() { gameObject.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 1); }//Cuando una carta desactiva su glow se oscurece
}