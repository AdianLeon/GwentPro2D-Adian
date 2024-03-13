using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="New Card",menuName="Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite artwork;
    public int power;
    public enum rank{Melee,Ranged,Siege,Aumento,Clima,Despeje,Senuelo}
    public rank cRank;
    public enum quality{Gold,Silver,None}
    public quality cQuality;
    public bool isPlayed=false;
    public void PlayCard(){
        isPlayed=true;
    }
}
