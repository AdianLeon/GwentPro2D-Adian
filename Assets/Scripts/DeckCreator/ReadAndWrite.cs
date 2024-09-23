using UnityEngine;
using System.IO;
using TMPro;
using System;
//Script para escribir y leer en el txt Code
public class ReadAndWrite : MonoBehaviour
{
    public TMP_InputField inputField;
    public void LoadTxtToCodeEditor() => inputField.text = File.ReadAllText(Application.persistentDataPath + "/Code.txt");//Se llama cuando se activa el menu Crear Deck
    public void SaveTextToFile() => File.WriteAllText(Application.persistentDataPath + "/Code.txt", inputField.text);//Guarda el texto del editor de codigo a el txt, se llama cuando se pulsa el boton
    public void ReadTextFromFile()
    {//Obtiene el texto del txt, se llama cuando se pulsa el boton (despues de SaveTextFile)
        string allText = File.ReadAllText(Application.persistentDataPath + "/Code.txt");
        MainCompiler.ProcessTextAndSave(allText);
    }
    public void RestoreDefaultDeck() => CreateDefaultDeck();
    public static void CreateDefaultDeck()
    {
        string minionsDeck = "card{\n\tType: \"Senuelo\",\n\tName: \"Agnes\",\n\tFaction: \"Minions\"\n}\n\ncard{\n\tType: \"Aumento\",\n\tName: \"Banana\",\n\tFaction: \"Minions\",\n\tPower: 25\n}\n\ncard{\n\tDescription: \"Cuando esta carta es jugada su poder se convierte en el promedio del poder de todas las cartas jugadas en el campo\",\n\tTotalCopies: 3,\n\tType: \"Plata\",\n\tName: \"Bob\",\n\tFaction: \"Minions\",\n\tRange:[\"Ranged\"],\n\tPower: 0,\n\tOnActivation: [{ScriptEffect: \"PromEffect\"}]\n}\n\ncard{\n\tTotalCopies: 2,\n\tType: \"Plata\",\n\tName: \"Doctor Nefario\",\n\tFaction: \"Minions\",\n\tRange:[\"Ranged\",\"Siege\"],\n\tPower: 25\n}\n\ncard{\n\tDescription: \"Cuando esta carta es jugada elimina a la carta de mayor poder jugada en el campo\",\n\tType: \"Oro\",\n\tName: \"El Macho\",\n\tFaction: \"Minions\",\n\tRange:[\"Melee\"],\n\tPower: 35,\n\tOnActivation: [{ScriptEffect: \"MostPowerEffect\"}]\n}\n\ncard{\n\tDescription: \"Ordena a los minions a que roben dos cartas de la mano enemiga, aunque conociendo a los minions eso puede salir mal\",\n\tType: \"Lider\",\n\tName: \"Gru\",\n\tFaction: \"Minions\",\n\tOnActivation: [{ScriptEffect: \"GruEffect\"}]\n}\n\ncard{\n\tType: \"Aumento\",\n\tName: \"Inyeccion de minion purpura\",\n\tFaction: \"Minions\",\n\tPower: 20\n}\n\ncard{\n\tTotalCopies: 3,\n\tDescription: \"Cuando esta carta es jugada su poder se convierte en el promedio del poder de todas las cartas jugadas en el campo\",\n\tType: \"Plata\",\n\tName: \"Kevin\",\n\tFaction: \"Minions\",\n\tRange:[\"Melee\"],\n\tPower: 0,\n\tOnActivation: [{ScriptEffect: \"PromEffect\"}]\n}\n\ncard{\n\tTotalCopies: 3,\n\tDescription: \"Cuando esta carta es jugada convierte su poder en la cantidad de Kyles que se encuentren en el campo\",\n\tType: \"Plata\",\n\tName: \"Kyle\",\n\tFaction: \"Minions\",\n\tRange:[\"Melee\",\"Siege\"],\n\tPower: 10,\n\tOnActivation: [{ScriptEffect: \"MultiplyEffect\"}]\n}\n\ncard{\n\tType: \"Aumento\",\n\tName: \"Manzana\",\n\tFaction: \"Minions\",\n\tPower: 20\n}\n\ncard{\n\tType: \"Senuelo\",\n\tName: \"Minion Bebe\",\n\tFaction: \"Minions\"\n}\n\ncard{\n\tType: \"Despeje\",\n\tName: \"Muchas Bananas\",\n\tFaction: \"Minions\"\n}\n\ncard{\n\tTotalCopies: 2,\n\tType: \"Plata\",\n\tName: \"Ninas\",\n\tFaction: \"Minions\",\n\tRange:[\"Melee\",\"Ranged\"],\n\tPower: 20\n}\n\ncard{\n\tType: \"Clima\",\n\tName: \"Peluche\",\n\tFaction: \"Minions\",\n\tPower: 25\n}\n\ncard{\n\tType: \"Clima\",\n\tName: \"Rayo Congelador\",\n\tFaction: \"Minions\",\n\tPower: 30\n}\n\ncard{\n\tDescription: \"Cuando esta carta es jugada se roba una carta del deck\",\n\tType: \"Oro\",\n\tName: \"Scarlett Overkill\",\n\tFaction: \"Minions\",\n\tRange:[\"Melee\",\"Ranged\",\"Siege\"],\n\tPower: 30,\n\tOnActivation: [{ScriptEffect: \"DrawOneEffect\"}]\n}\n\ncard{\n\tTotalCopies: 3,\n\tDescription: \"Cuando esta carta es jugada su poder se convierte en el promedio del poder de todas las cartas jugadas en el campo\",\n\tType: \"Plata\",\n\tName: \"Stuart\",\n\tFaction: \"Minions\",\n\tRange:[\"Siege\"],\n\tPower: 0,\n\tOnActivation: [{ScriptEffect: \"PromEffect\"}]\n}\n\ncard{\n\tDescription: \"Cuando esta carta es jugada elimina a la carta de menor poder jugada en el campo\",\n\tType: \"Oro\",\n\tName: \"Vector\",\n\tFaction: \"Minions\",\n\tRange:[\"Ranged\",\"Siege\"],\n\tPower: 30,\n\tOnActivation: [{ScriptEffect: \"LessPowerEffect\"}]\n}";
        MainCompiler.ProcessTextAndSave(minionsDeck);
    }
    public void ShowExampleCode() => inputField.text = "effect{\n\tName: \"Damage\",\n\tParams: {\n\t\tAmount: Number\n\t},\n\tAction:(targets, context)=> {\n\t\tfor target in targets {\n\t\t\ti=0;\n\t\t\twhile (i < Amount){\n\t\t\t\ti++;\n\t\t\t\ttarget.Power-=1;\n\t\t\t};\n\t\t};\n\t}\n}\neffect{\n\tName: \"Draw\",\n\tAction: (targets, context) => {\n\t\ttopCard = context.Deck.Pop();\n\t\tcontext.Hand.Push(topCard);\n\t\tcontext.Hand.Shuffle(); \n\t}\n}\neffect {\n\tName: \"ReturnToDeck\",\n\tAction: (targets, context) => {\n\t\tfor target in targets {\n\t\t\towner = target.Owner;\n\t\t\tdeck = context.DeckOfPlayer(owner);\n\t\t\tdeck.Push(target);\n\t\t\tdeck.Shuffle();\n\t\t\tcontext.Board.Remove(target);\n\t\t};\n\t}\n}\ncard{\n\tDescription:\"Dana a todas las unidades del campo de la faccion 'Northern Realms' y luego envia de vuelta al deck a aquellas que tienen dano menor que 1, entonces toma una carta del deck\",\n\tType: \"Oro\",\n\tName: \"Beluga\",\n\tFaction: \"Northern Realms\",\n\tPower: 10,\n\tRange: [\"Melee\", \"Ranged\"],\n\tOnActivation: [\n\t\t{\n\t\t\tEffect: {\n\t\t\t\tName: \"Damage\",\n\t\t\t\tAmount: 5\n\t\t\t},\n\t\t\tSelector: {\n\t\t\t\tSource: \"board\",\n\t\t\t\tSingle: false,\n\t\t\t\tPredicate: (unit) => unit.Faction == \"Northern\" @@ \"Realms\"\n\t\t\t},\n\t\t\tPostAction:{\n\t\t\t\tEffect:\"ReturnToDeck\",\n\t\t\t\tSelector: {\n\t\t\t\t\tSource: \"parent\",\n\t\t\t\t\tSingle: false,\n\t\t\t\t\tPredicate: (unit) => unit.Power < 1\n\t\t\t\t}\n\t\t\t}\n\t\t},\n\t\t{\n\t\t\tEffect: \"Draw\"\n\t\t}\n\t]\n}";
}
// 	OnActivation:
// [
//         {
// Effect:
//     {
//     Name: "Damage",
// 				Amount: 5,
// 			},
// 			Selector:
//     {
//     Source: "board",
// 				Single: false,
// 				Predicate: (unit) => unit.Faction == "Northern" @@ "Realms"
//             },
// 			PostAction{
//     Type: "ReturnToDeck",
// 				Selector:
//         {
//         Source: "parent",
// 					Single: false,
// 					Predicate (unit) => unit.Power < 1
//                 }
//     }
// },
// 		{
// Effect: Draw
//         }
// 	]
// }
//______________________________________________________________________________________
//______________________________________________________________________________________
// 	OnActivation:
// [
//         {
// Effect:
//     {
//     Name: "Damage",
// 				Amount: 5

//             },
// 			Selector:
//     {
//     Source: "board",
// 				Single: false,
// 				Predicate: (unit) => unit.Faction == "Northern" @@ "Realms"
//             },
// 			PostAction:
//     {
//     Effect: "ReturnToDeck",
// 				Selector:
//         {
//         Source: "parent",
// 					Single: false,
// 					Predicate: (unit) => unit.Power < 1
//                 }
//     }
// },
// 		{
// Effect: "Draw"
//         }
// 	]
// }
