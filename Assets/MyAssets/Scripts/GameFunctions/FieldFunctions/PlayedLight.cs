using System.Collections.Generic;
using UnityEngine;
//Script de las luces del juego que indican si se puede jugar
public class PlayedLight : MonoBehaviour, IStateSubscriber
{
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (new Execution(stateInfo=>{ if (Judge.CanPlay) { PaintGreen(); } else { PaintRed(); } }, 1)),
        new (State.EndingGame, new Execution (stateInfo => PaintRed(), 0))
    };
    private void PaintGreen() => gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 1, 0, 0.2f);
    private void PaintRed() => gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 0, 0, 0.2f);
}
