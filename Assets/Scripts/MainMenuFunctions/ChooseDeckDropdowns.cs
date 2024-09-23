using UnityEngine;
using System.IO;
using TMPro;
//Script para asignar el deck de los jugadores a traves de un dropdown
public class ChooseDeckDropdowns : MonoBehaviour
{
    private TMP_Dropdown deckDropdown;
    private string playerDeck;
    void Awake()
    {
        deckDropdown = gameObject.GetComponent<TMP_Dropdown>();
        playerDeck = name;//El nombre de los dos objetos que contienen este script coincide con P1PrefDeck o P2PrefDeck

        deckDropdown.ClearOptions();//Quita todas las opciones del dropdown
        ModifyDeckFunctions.LoadOptionsInDropdown(deckDropdown, "Decks/", "");//Anade los decks al dropdown
        SetDropDownOption(deckDropdown, PlayerPrefs.GetString(playerDeck));//Hace que la opcion mostrada sea la guardada como deck escogido
        deckDropdown.onValueChanged.AddListener(delegate { OnDeckValueChanged(); });
    }
    public void OnDeckValueChanged()
    {//Cuando el valor del dropdown se modifique se llama a este metodo 
        PlayerPrefs.SetString(playerDeck, deckDropdown.options[deckDropdown.value].text);//Ahora el deck escogido es la opcion escogida
    }
    private static void SetDropDownOption(TMP_Dropdown dropdown, string option)
    {
        for (int i = 0; i < dropdown.options.Count; i++)
        {//Itera por las opciones y setea como opcion mostrada a la pasada como parametro
            if (dropdown.options[i].text == option)
            {
                dropdown.SetValueWithoutNotify(i);
                break;
            }
        }
        //Por alguna extrana razon si la opcion seteada es 0 no se muestra el nombre en el label del dropdown, la siguiente linea es para solucionar ese bug
        GameObject.Find(dropdown.gameObject.name + "Label").GetComponent<TextMeshProUGUI>().text = option;
    }
}
