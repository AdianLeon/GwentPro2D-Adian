using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
//Script para escribir en el txt
public class ReadAndWrite : MonoBehaviour
{
    public TMP_InputField inputField;
    private static string filePath;
    public void SaveTextToFile(){//Guarda el texto del editor de codigo a el txt
        filePath=Application.dataPath+"/MyAssets/Database/Code.txt";
        string textToSave=inputField.text;
        File.WriteAllText(filePath,textToSave);
    }
    public void ReadTextFromFile(){//Obtiene el texto del txt
        string allText=File.ReadAllText(filePath);
    }
    public void LoadTxtToCodeEditor(){
        inputField.text=File.ReadAllText(Application.dataPath+"/MyAssets/Database/Code.txt");
    }
}
