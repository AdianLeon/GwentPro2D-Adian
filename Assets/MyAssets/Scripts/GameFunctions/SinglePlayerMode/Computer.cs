using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine;
//Script que toma el control de las acciones de P2
public class Computer : MonoBehaviour
{
    private static bool isPlaying;
    public static bool IsPlaying => isPlaying;
    public void CheckPlayConditions()
    {//Cuando se termina algun turno se llama siempre a este metodo
        if (PlayerPrefs.GetInt("SinglePlayerMode") == 0) { return; }//Si el modo un solo jugador esta desactivado
        if (Judge.GetPlayer != Player.P2) { return; }//Si no es el turno de P2
        if (!Judge.CanPlay) { return; }//Si no se puede jugar
        Play();
    }
    public void Play()
    {//Toma acciones en dependencia del estado del juego
        GFUtils.FindGameObjectsOfType<HandCover>().ForEach(cover => cover.gameObject.SetActive(true));//Bloquea la vision del jugador en ambas manos para evitar que pueda tomar acciones
        isPlaying = true;//Declara que la computadora esta jugando y hasta que no se cambie su valor el jugador no accionar en el juego
        if (Judge.IsFirstTurnOfPlayer)//Si es el inicio del juego
        {//Intenta intercambiar los senuelos y despejes de la mano (si hay)
            Hand.PlayerHandCards.Where(card => card.GetComponent<BaitCard>() != null || card.GetComponent<ClearWeatherCard>() != null).ForEach(card => TryTradeCard(card));
        }
        if (Judge.IsLastTurnOfRound) { TryPlayCardsUntilWin(); }//Si es el ultimo turno de la ronda intenta jugar cartas hasta que gane
        else { DoAfterTime(delegate { TryPlayOneCard(); FinishTurn(); }, UnityEngine.Random.Range(1, 4)); }//Si no es el ultimo turno de la ronda se intenta jugar una carta
    }
    private void TryTradeCard(DraggableCard card)
    {//Intenta intercambiar la carta con el deck
        DeckTrade deck = FindObjectsOfType<DeckTrade>().SingleOrDefault(deck => deck.gameObject.Field() == Player.P2 && deck.IsDropValid(card));
        if (deck != null) { deck.TradeCardWithDeck(card); }
    }
    private void TryPlayCardsUntilWin()
    {//Intenta jugar cartas hasta que se gane la ronda o se acaben las cartas
        if (Field.P2ForceValue > Field.P1ForceValue) { DoAfterTime(delegate { FinishTurn(); }, 1); return; }//Si ya es suficiente para ganar la ronda
        if (Hand.PlayerHandCards.Where(card => card.GetComponent<BaitCard>() == null || card.GetComponent<ClearWeatherCard>() == null).Count() == 0)
        {//Si no nos quedan cartas por jugar que no sean senuelos o despejes
            DoAfterTime(delegate { FinishTurn(); }, 1); return;
        }
        DoAfterTime(delegate { TryPlayOneCard(); TryPlayCardsUntilWin(); }, 2);//Intentamos jugar una carta y volvemos a iniciar este proceso
    }
    private void TryPlayOneCard()
    {//Juega la mejor de las cartas que puede jugar
        if (TryPlayUnitCard()) { return; }//Intenta jugar una carta de unidad
        if (TryPlayBoostCard()) { return; }//Si no se puede jugar una carta de unidad entonces intenta jugar una carta de aumento
        if (TryPlayWeatherCard()) { return; }//Si no se puede jugar una carta de aumento entonces intenta jugar una carta de clima
        // if (TryPlayClearWeatherCard()) { return; }//Si no se puede jugar una carta de clima entonces intenta jugar una carta de despeje
        UserRead.Write("El enemigo ha pasado sin jugar");//Si no se puede jugar ninguna carta
    }
    private bool TryPlayUnitCard()
    {//Juega la carta de unidad de mayor poder en la zona menos afectada por climas
        DraggableCard[] validCards = Hand.PlayerHandCards.Where(card => card.GetComponent<UnitCard>() != null).ToArray();
        if (validCards.Length == 0) { return false; }
        //Hallando la carta de mayor poder
        DraggableCard chosenCard = validCards.OrderByDescending(card => card.GetComponent<UnitCard>().TotalPower).First();
        //Halla las zonas climas que afectan a las zonas donde la carta se puede jugar. O sea que las zonas donde la carta se puede jugar contienen a el segundo objetivo de la zona de clima
        IEnumerable<DZWeather> weatherZonesAffecting = FindObjectsOfType<DZWeather>().Where(zone => FindObjectsOfType<DZUnit>().Where(zone => zone.IsDropValid(chosenCard)).Contains(zone.Target2));
        //Se ordenan por cuanto dano sus climas acumulan
        weatherZonesAffecting = weatherZonesAffecting.OrderBy(zone => zone.gameObject.CardsInside<WeatherCard>().Sum(card => card.Damage));
        DropZone chosenZone = weatherZonesAffecting.First().Target2;//Se elige la zona que afectan los climas que menos dano hacen
        chosenCard.TryPlayCardIn(chosenZone);
        UserRead.Write("El enemigo ha jugado a " + chosenCard.CardName + " en la zona: " + chosenZone.name);
        return true;
    }
    private bool TryPlayBoostCard()
    {//Intenta jugar la carta de aumento de mayor boost en la mejor zona
        DraggableCard[] validCards = Hand.PlayerHandCards.Where(card => card.GetComponent<BoostCard>() != null).ToArray();
        if (Hand.PlayerHandCards.Where(card => card.GetComponent<BoostCard>() != null).Count() == 0) { return false; }
        //Elige a la carta de aumento de mayor boost
        DraggableCard chosenCard = validCards.OrderByDescending(card => card.GetComponent<BoostCard>().Boost).First();
        //Se selecciona de todas las dropzones de cartas de aumento la que tenga como objetivo la dropzone de cartas de unidad de mayor cantidad de cartas afectables
        DZBoost chosenZone = FindObjectsOfType<DZBoost>().Where(zone => zone.gameObject.Field() == Player.P2).OrderByDescending(zone => CountIAffectables(zone.Target)).First();
        chosenCard.TryPlayCardIn(chosenZone);
        UserRead.Write("El enemigo ha jugado a " + chosenCard.CardName + " en la zona: " + chosenZone.name);
        return true;
    }
    private bool TryPlayWeatherCard()
    {//Intenta jugar la carta de clima de mayor damage en la mejor zona
        //Se obtienen todas las posibles cartas de clima, si no hay,
        IEnumerable<DraggableCard> validCards = Hand.PlayerHandCards.Where(card => card.GetComponent<WeatherCard>() != null);
        if (validCards.Count() == 0) { return false; }
        //Elige a la carta de clima de mayor damage
        DraggableCard chosenCard = validCards.OrderByDescending(card => card.GetComponent<WeatherCard>().Damage).First();
        //Se halla la zona de clima que afecte a la mayor cantidad de cartas de P1 y menor cartas de P2
        DZWeather chosenZone = FindObjectsOfType<DZWeather>().OrderByDescending(zone => CountIAffectables(zone.Target1) - CountIAffectables(zone.Target2)).First();
        //Si esta diferencia es negativa entonces no se juega el clima
        if (CountIAffectables(chosenZone.Target1) - CountIAffectables(chosenZone.Target2) < 0) { return false; }
        UserRead.Write("El enemigo ha jugado a " + chosenCard.CardName + " en la zona: " + chosenZone.name);
        chosenCard.TryPlayCardIn(chosenZone);
        return true;
    }
    private bool TryPlayClearWeatherCard()
    {//Intenta jugar la carta de clima de mayor damage en la mejor zona
        //Se obtienen todas las posibles cartas de clima, si no hay,
        IEnumerable<DraggableCard> validCards = Hand.PlayerHandCards.Where(card => card.GetComponent<ClearWeatherCard>() != null);
        if (validCards.Count() == 0) { return false; }
        return false;
    }
    private void FinishTurn()
    {//Termina el turno y permite accionar al jugador
        Judge.EndTurnOrRound();
        isPlaying = false;
    }
    private int CountIAffectables(DZUnit zone)
    {//Devuelve la cantidad de cartas afectables de la zona
        return zone.gameObject.CardsInside<IAffectable>().Count;
    }
    private void DoAfterTime(Action action, int time) { StartCoroutine(CorroutineDoAfterTime(action, time)); }//Realiza la accion luego de que el tiempo pase
    private IEnumerator CorroutineDoAfterTime(Action action, int time) { yield return new WaitForSeconds(time); action(); }
}