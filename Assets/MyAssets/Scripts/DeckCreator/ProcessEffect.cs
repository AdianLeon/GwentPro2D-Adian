using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessEffect : MonoBehaviour
{
    public static void SaveEffectOnJson(List<Token> tokenList,int start,int end){

    }
    public static void ExecuteEffect(GameObject caller,string onActivationCodeName){
        Debug.Log("Effect! Card: "+caller.name+" code is "+onActivationCodeName);
    }
}
