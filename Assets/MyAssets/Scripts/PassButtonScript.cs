using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassButtonScript : MonoBehaviour
{
    public GameObject objectToToggle;

    public void Toggle(){
        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}