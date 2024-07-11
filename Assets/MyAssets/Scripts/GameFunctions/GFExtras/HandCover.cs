using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//Script para reaccionar activar el objeto cuando sea turno del enemigo
public class HandCover : MonoBehaviour
{
    public static void UpdateCovers(){//Actualiza el estado de todos los covers del campo
        HandCover[] covers=Resources.FindObjectsOfTypeAll<HandCover>();
        foreach(HandCover cover in covers){
            cover.UpdateVisibility();
        }
    }
    private void UpdateVisibility(){//Si es el turno de su enemigo, se activa, si es el turno de su jugador, se desactiva
        this.gameObject.SetActive(GFUtils.GetField(this.name)==Judge.GetEnemy);
    }
}
