using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
//Script para escribir en el txt
public class ReadAndWrite : MonoBehaviour
{
    public TMP_InputField inputField;
    public void SaveTextToFile(){//Guarda el texto del editor de codigo a el txt, se llama cuando se pulsa el boton
        string textToSave=inputField.text;
        File.WriteAllText(Application.dataPath+"/MyAssets/Database/Code.txt",textToSave);
    }
    public void ReadTextFromFile(){//Obtiene el texto del txt, se llama cuando se pulsa el boton (despues de SaveTextFile)
        string allText=File.ReadAllText(Application.dataPath+"/MyAssets/Database/Code.txt");
        Lexer.TokenizeCode(allText);
    }
    public void LoadTxtToCodeEditor(){//Se llama cuando se activa el menu Crear Deck
        inputField.text=File.ReadAllText(Application.dataPath+"/MyAssets/Database/Code.txt");
    }
}
