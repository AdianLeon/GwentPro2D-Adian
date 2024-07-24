using System.Linq;
using UnityEngine;
//Script para el efecto de la carta lider
public class GruEffect : MonoBehaviour, ICardEffect
{
    public string GetEffectDescription => "Ordena a los minions que roben dos cartas de la mano enemiga aunque conociendo a los minions eso puede salir mal (Solo se puede activar una vez por partida)";
    private static bool StealRandomEnemyCard()
    {//Robar del enemigo una carta random
        if (Hand.EnemyCards.Count() == 0) { return false; }//Si no tiene cartas devuelve false
        DraggableCard cardToSteal = Hand.EnemyCards.RandomElement().GetComponent<DraggableCard>();//Carta random de la mano objetivo
        cardToSteal.transform.SetParent(GameObject.Find("Hand" + Judge.GetPlayer).transform);//Pone la carta robada en la mano del ladron
        cardToSteal.WhichPlayer = Judge.GetPlayer;//Cambia el campo de la carta al del jugador que desencadeno el efecto
        return true;
    }
    public void TriggerEffect()
    {
        int r = UnityEngine.Random.Range(0, 4);
        switch (r)
        {
            case 0://Se roba 2 cartas al enemigo
                if (!StealRandomEnemyCard()) { UserRead.Write("Los minions han intentado robar dos cartas al enemigo, pero no tenia ninguna"); return; }
                if (!StealRandomEnemyCard()) { UserRead.Write("Los minions han intentado robar dos cartas al enemigo, pero solo tenia una"); return; }
                UserRead.Write("Los minions han robado dos cartas de la mano enemiga exitosamente"); return;
            case 1://Se roba 1 carta al enemigo
            case 2:
                if (!StealRandomEnemyCard()) { UserRead.Write("Los minions han intentado robar dos cartas al enemigo, pero no tenia ninguna"); return; }
                UserRead.Write("Los minions han intentado robar dos cartas al enemigo, pero solo consiguieron robar una"); return;
            case 3://Fallo
                UserRead.Write("Los minions intentaron robar dos cartas al enemigo, pero no robaron ninguna"); return;
        }
    }
}