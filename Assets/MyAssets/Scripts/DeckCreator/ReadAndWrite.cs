using UnityEngine;
using System.IO;
using TMPro;
//Script para escribir y leer en el txt Code
public class ReadAndWrite : MonoBehaviour
{
    public TMP_InputField inputField;
    public void LoadTxtToCodeEditor() => inputField.text = File.ReadAllText(Application.dataPath + "/MyAssets/Database/Code.txt");//Se llama cuando se activa el menu Crear Deck
    public void SaveTextToFile() => File.WriteAllText(Application.dataPath + "/MyAssets/Database/Code.txt", inputField.text);//Guarda el texto del editor de codigo a el txt, se llama cuando se pulsa el boton
    public void ReadTextFromFile()
    {//Obtiene el texto del txt, se llama cuando se pulsa el boton (despues de SaveTextFile)
        string allText = File.ReadAllText(Application.dataPath + "/MyAssets/Database/Code.txt");
        MainCompiler.ProcessText(allText);
    }
}
