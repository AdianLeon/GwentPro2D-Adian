using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//Script para escribir en el txt
public class ReadAndWrite : MonoBehaviour
{
    public TMPro.TMP_InputField inputField;
    private static string filePath;
    void Start(){
        filePath=Application.dataPath+"/MyAssets/DeckCreator/Code.txt";
    }

    public void SaveTextToFile(){//Guarda el texto del editor de codigo a el txt
        string textToSave=inputField.text;
        File.WriteAllText(filePath,textToSave);
    }
    public void ReadTextFromFile(){//Obtiene el texto del txt
        string allText=File.ReadAllText(filePath);
        Debug.Log(allText);
    }
}
