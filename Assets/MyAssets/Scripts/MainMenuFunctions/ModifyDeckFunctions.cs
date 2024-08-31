using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class ModifyDeckFunctions : MonoBehaviour
{
    public TMP_InputField code;
    public TMP_Dropdown effectsChoice;
    public TMP_Dropdown decksChoice;
    public TMP_Dropdown cardsChoice;
    private static string GetCurrentText(TMP_Dropdown dropdown) => dropdown.options[dropdown.value].text;
    private static string RemoveTxt(string nameWithExtension) => nameWithExtension.Substring(0, nameWithExtension.Length - 4);

    public void OnConfirmChangesButtonClick() => MainCompiler.ProcessTextAndSave(code.text);
    public void OnMenuActivation()
    {//Este metodo se llama cuando el menu modificar deck es activado por el boton
        effectsChoice.ClearOptions();
        LoadOptionsInDropdown(effectsChoice, "CreatedEffects/", ".txt");
        FinishSettingDropdown(effectsChoice);

        decksChoice.ClearOptions();
        decksChoice.onValueChanged.AddListener(delegate { SetCardDropdown(); });
        LoadOptionsInDropdown(decksChoice, "Decks/", "");
        FinishSettingDropdown(decksChoice);

        SetCardDropdown();
        UpdateCode();
    }
    private void UpdateCode()
    {//Este metodo se llama cuando el menu modificar deck es activado por el boton luego de LoadChoicesOnDropdowns() y cada vez que se cambia el valor de alguno de los dropdown
        string allCode = "";

        if (GetCurrentText(effectsChoice) == "Todas")
        {
            Directory.GetFiles(Application.dataPath + "/MyAssets/Database/CreatedEffects/", "*.txt").
            ForEach(address => allCode += File.ReadAllText(address) + '\n');
        }
        else if (GetCurrentText(effectsChoice) != "Ninguna")
        {
            allCode += File.ReadAllText(Application.dataPath + "/MyAssets/Database/CreatedEffects/" + GetCurrentText(effectsChoice) + ".txt") + '\n';
        }

        if (GetCurrentText(cardsChoice) == "Todas")
        {
            if (GetCurrentText(decksChoice) == "Todas")
            {
                Directory.GetFiles(Application.dataPath + "/MyAssets/Database/Decks/", "*.txt", SearchOption.AllDirectories).
                ForEach(address => allCode += File.ReadAllText(address) + '\n');
            }
            else if (GetCurrentText(decksChoice) != "Ninguna")
            {
                Directory.GetFiles(Application.dataPath + "/MyAssets/Database/Decks/" + GetCurrentText(decksChoice) + "/", "*.txt").
                ForEach(address => allCode += File.ReadAllText(address) + '\n');
            }
        }
        else if (GetCurrentText(cardsChoice) != "Ninguna")
        {
            if (GetCurrentText(decksChoice) == "Todas")
            {
                foreach (string address in Directory.GetFiles(Application.dataPath + "/MyAssets/Database/Decks/", "*.txt", SearchOption.AllDirectories))
                {
                    if (Path.GetFileName(address) == GetCurrentText(cardsChoice) + ".txt")
                    {
                        allCode += File.ReadAllText(address) + '\n';
                        break;
                    }
                }
            }
            else
            {
                allCode += File.ReadAllText(Application.dataPath + "/MyAssets/Database/Decks/" + GetCurrentText(decksChoice) + "/" + GetCurrentText(cardsChoice) + ".txt") + '\n';
            }
        }

        code.text = allCode;
    }
    public static void LoadOptionsInDropdown(TMP_Dropdown dropdown, string address, string extension)
    {//Obtiene todos los directorios en la carpeta Decks y los anade como opcion en el dropdown
        IEnumerable<string> names;
        if (extension == "") { names = new DirectoryInfo(Application.dataPath + "/MyAssets/Database/" + address).GetDirectories().Select(subDir => subDir.Name); }
        else if (extension == ".txt") { names = Directory.GetFiles(Application.dataPath + "/MyAssets/Database/" + address, "*" + extension).Select(path => RemoveTxt(Path.GetFileName(path))); }
        else { return; }
        //Anade todas las carpetas (decks) como opciones del dropdown
        names.ForEach(name => dropdown.options.Add(new TMP_Dropdown.OptionData(name)));
    }
    private void FinishSettingDropdown(TMP_Dropdown dropdown)
    {
        dropdown.options.Add(new TMP_Dropdown.OptionData("Ninguna"));
        dropdown.options.Add(new TMP_Dropdown.OptionData("Todas"));
        dropdown.SetValueWithoutNotify(dropdown.options.Count - 1);
        dropdown.onValueChanged.AddListener(delegate { UpdateCode(); });
    }
    private void SetCardDropdown()
    {
        cardsChoice.ClearOptions();//Quita todas las opciones del dropdown
        if (GetCurrentText(decksChoice) == "Todas")
        {
            for (int i = 0; i < decksChoice.options.Count - 2; i++) { LoadOptionsInDropdown(cardsChoice, "Decks/" + decksChoice.options[i].text + "/", ".txt"); }
            FinishSettingDropdown(cardsChoice);
        }
        else if (GetCurrentText(decksChoice) != "Ninguna")
        {
            LoadOptionsInDropdown(cardsChoice, "Decks/" + GetCurrentText(decksChoice), ".txt");
            FinishSettingDropdown(cardsChoice);
        }
        else { FinishSettingDropdown(cardsChoice); }
    }
}
