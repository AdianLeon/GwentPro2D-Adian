using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class DeckAssignDropdowns : MonoBehaviour
{
    private TMP_Dropdown deckDropdown;
    private string playerDeck;
    void Start(){
        deckDropdown=this.gameObject.GetComponent<TMP_Dropdown>();
        playerDeck=this.name.Substring(0,6);

        LoadFilesInDropdown(deckDropdown);

        SetDropDownOption(deckDropdown,PlayerPrefs.GetString(playerDeck));

        deckDropdown.onValueChanged.AddListener(delegate{OnDeckValueChanged();});
    }
    public void OnDeckValueChanged(){
        PlayerPrefs.SetString(playerDeck,deckDropdown.options[deckDropdown.value].text);
    }    private static void SetDropDownOption(TMP_Dropdown dropdown,string option){
        for(int i=0;i<dropdown.options.Count;i++){
            if(dropdown.options[i].text==option){
                dropdown.SetValueWithoutNotify(i);
                break;
            }
        }
    }
    public static void LoadFilesInDropdown(TMP_Dropdown dropdown){//Obtiene todos los directorios en la carpeta Decks y los anade como opcion en el dropdown
        DirectoryInfo dir=new DirectoryInfo(Application.dataPath+"/MyAssets/Database/Decks/");
        DirectoryInfo[] subDirs=dir.GetDirectories();//Carpetas dentro de Decks
        dropdown.ClearOptions();//Quita todas las opciones del dropdown

        for(int i=0;i<subDirs.Length;i++){//Anade todas las carpetas (decks)
            dropdown.options.Add(new TMP_Dropdown.OptionData(subDirs[i].Name));
        }
    }
}
