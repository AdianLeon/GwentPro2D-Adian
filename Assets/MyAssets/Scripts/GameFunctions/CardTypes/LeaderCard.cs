using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
//Script para la carta Lider
public class LeaderCard : Card, IPointerClickHandler, IStateListener
{
    public override Color CardViewColor => new Color(0.7f, 0.1f, 0.5f);
    private bool hasUsedSkill;//Si la habilidad ha sido usada
    public int GetPriority => 0;
    public void CheckState() { if (Judge.CurrentState == State.SettingUpGame) { hasUsedSkill = false; } }//Al inicio de cada juego resetea el uso de la habilidad de lider
    public override void LoadInfo()
    {
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text = "[L]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text = "";
        GameObject.Find("BGPower").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text = "";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
    public override bool IsPlayable => LeaderSkillConditions();
    private bool LeaderSkillConditions()
    {
        if (WhichPlayer != Judge.GetPlayer || (PlayerPrefs.GetInt("SinglePlayerMode") == 1 && Judge.GetPlayer == Player.P2)) { UserRead.Write("Ese no es el lider de tu deck"); return false; }
        if (hasUsedSkill) { UserRead.Write("La habilidad del lider: " + CardName + " ya ha sido usada. La habilidad de lider solo puede ser usada una vez por partida"); return false; }
        return true;
    }
    public void OnPointerClick(PointerEventData eventData) { if (TryPlay()) { hasUsedSkill = true; } }//Intenta jugar el lider cuando se clickee, si lo logra ha usado la habilidad
}