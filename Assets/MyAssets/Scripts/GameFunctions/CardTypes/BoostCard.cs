using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Script para las cartas de aumento
public class BoostCard : DraggableCard, IShowZone, ISpecialCard
{
    public string GetEffectDescription=>"Aumenta el poder de las cartas de la fila seleccionada";
    public override Color GetCardViewColor=>new Color(0.4f,1,0.3f);
    public int Boost;//Cantidad de poder aumentado cuando una carta es afectada por el aumento
    public override void LoadInfo(){
        base.LoadInfo();
        GameObject.Find("Type").GetComponent<TextMeshProUGUI>().text="[A]";

        GameObject.Find("Power").GetComponent<TextMeshProUGUI>().text="+"+Boost;
        GameObject.Find("BGPower").GetComponent<Image>().color=new Color(0.2f,0.2f,0.2f,1);

        GameObject.Find("AddedPower").GetComponent<TextMeshProUGUI>().text="";
        GameObject.Find("BGAddedPower").GetComponent<Image>().color=new Color(1,1,1,0);
    }
    public override bool IsPlayable{get=>this.transform.parent.gameObject.GetComponent<DZBoost>()!=null;}
    public void TriggerSpecialEffect(){//Efecto de las cartas aumento
        GameObject target=this.transform.parent.GetComponent<DZBoost>().Target;//Objetivo padre de las cartas a las que anadirle poder
        foreach(Transform card in target.transform){
            if(card.GetComponent<IAffectable>()!=null){//Si es afectable
                card.GetComponent<PowerCard>().AddedPower+=this.GetComponent<BoostCard>().Boost;//Aumenta el poder
            }
        }
        Graveyard.SendToGraveyard(this.gameObject);//Envia la carta de aumento al cementerio
    }
}
