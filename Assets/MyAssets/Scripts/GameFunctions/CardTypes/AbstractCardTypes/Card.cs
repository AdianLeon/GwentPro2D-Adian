using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//Script que contiene los comportamientos y propiedades en comun de todas las cartas
public abstract class Card : MonoBehaviour, IGlow, IPointerEnterHandler, IPointerExitHandler
{
    protected static Card EnteredCard;//En esta variable se guarda el objeto debajo del puntero el cual mostramos en CardView
    public string Faction;//Faccion de la carta
    public string CardName;//Nombre de la carta
    public string Description;//Descripcion de la carta
    public OnActivation OnActivation;//Descripcion de acciones (efectos) para cuando la carta se active
    public Sprite Artwork;//Imagen para mostrar en el CardView
    public Player Owner;//Jugador dueno de la carta
    public virtual void LoadInfo()
    {//Esta funcion es especifica para cada tipo de carta, pero todas comparten lo siguiente
        GameObject.Find("Faction").GetComponent<TextMeshProUGUI>().text = Faction;
        GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text = CardName;
        GameObject.Find("BGType").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        //EffectDescription
        GameObject.Find("EffectDescription").GetComponent<TextMeshProUGUI>().text = Description;
        if (GetComponent<ISpecialCard>() != null && Description == "") { GameObject.Find("EffectDescription").GetComponent<TextMeshProUGUI>().text = GetComponent<ISpecialCard>().GetEffectDescription; }
        //Quality, Image
        GameObject.Find("Quality").GetComponent<Image>().sprite = Resources.Load<Sprite>("BlankImage");
        GameObject.Find("CardPreview").GetComponent<Image>().sprite = Artwork;
        //Colores
        GameObject.Find("BackGroundCard").GetComponent<Image>().color = CardViewColor;
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().color = CardViewColor;
        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().color = CardViewColor;
    }
    public abstract Color CardViewColor { get; }//Retorna el color de la carta en el CardView
    public abstract bool IsPlayable { get; }//Conjunto de condiciones para que la carta se pueda jugar, diferente para todas las cartas
    public bool TryPlay()
    {//Intenta jugar la carta y devuelve si se cumplieron las condiciones de IsPlayable
        if (!IsPlayable) { return false; }
        UserRead.Write("Se ha jugado a " + CardName);
        gameObject.GetComponent<ISpecialCard>()?.TriggerSpecialEffect();//Si la carta tiene efecto de carta especial, que se active
        Execute.DoEffect(this);//Se ejecuta el OnActivation
        StateManager.Publish(State.PlayingCard, new StateInfo { CardPlayed = this, Player = Owner });//Todos los IStateListener reaccionan ante la carta jugada
        LoadInfo();//Carga la info de la carta luego de que todos los IStateListener han reaccionado al estado
        return true;
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {//Se activa cuando el mouse entra en la carta
        EnteredCard = this;
        TriggerGlow();//La carta se sombrea cuando pasamos por encima
        LoadInfo();//Se carga toda la informacion de esta carta en el CardView
    }
    public void OnPointerExit(PointerEventData eventData)
    {//Se llama cuando el mouse sale de la carta
        EnteredCard = null;//Ya no se esta encima de ninguna carta 
        GFUtils.FindGameObjectsOfType<IGlow>().ForEach(glower => glower.RestoreGlow());//Se dessombrean todas las cartas jugadas y se desactiva la iluminacion de todas las zonas
    }
    public void TriggerGlow() => gameObject.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 1); //Cuando una carta desactiva su glow se oscurece
    public void RestoreGlow() => gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);//Cuando una carta activa su glow se devuelve a su estado normal totalmente visible
}