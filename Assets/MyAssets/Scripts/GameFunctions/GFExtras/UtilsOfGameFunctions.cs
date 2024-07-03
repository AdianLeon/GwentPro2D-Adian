using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameFunctionsUtils : MonoBehaviour
{
    public List<GameObject> GetChildrensIn(List<GameObject> places){
        List<GameObject> childrens=new List<GameObject>();
        foreach(GameObject place in places){
            childrens.AddRange(GetChildrensIn(place));
        }
        return childrens;
    }
    public List<GameObject> GetChildrensIn(GameObject place){
        List<GameObject> childrens=new List<GameObject>();
        for(int i=0;i<place.transform.childCount;i++){
            childrens.Add(place.transform.GetChild(i).gameObject);
        }
        return childrens;
    }
}
