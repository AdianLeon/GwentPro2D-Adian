using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine;
//Script que toma el control de las acciones de P2
public class Computer : MonoBehaviour, IStateSubscriber
{
    private static bool isPlaying;
    public static bool IsPlaying => isPlaying;
    public static bool IsActive => PlayerPrefs.GetInt("SinglePlayerMode") != 0;
    public List<StateSubscription> GetStateSubscriptions => new List<StateSubscription>
    {
        new (new List<State>{ State.EndingTurn, State.EndingRound },new Execution(stateInfo => TryPlay(), 3))
    };
    private void TryPlay()
    {//Cuando se termina algun turno se llama siempre a este metodo
        if (!IsActive) { return; }//Si el modo un solo jugador esta desactivado
        if (Judge.GetPlayer != Player.P2) { return; }//Si no es el turno de P2
        if (!Judge.CanPlay) { return; }//Si no se puede jugar
        Play();
    }
    private void Play()
    {//Toma acciones en dependencia del estado del juego
        isPlaying = true;//Declara que la computadora esta jugando y hasta que no se cambie su valor el jugador no podra accionar en el juego
        GFUtils.FindGameObjectsOfType<HandCover>().ForEach(cover => cover.gameObject.SetActive(true));//Bloquea la vision del jugador en ambas
        //Si es el inicio del juego intenta intercambiar los senuelos de la mano
        if (Judge.TurnNumber <= 2) { Hand.PlayerCards.Where(card => card.GetComponent<BaitCard>() != null).ForEach(card => TryTradeCard(card)); }

        if (Judge.IsLastTurnOfRound) { DoAfterTime(delegate { TryPlayCardsUntilWin(); }, 1); }//Si es el ultimo turno de la ronda intenta jugar cartas hasta que gane
        else { DoAfterTime(delegate { TryPlayOneCard(); FinishTurn(); }, UnityEngine.Random.Range(1, 4)); }//Si no es el ultimo turno de la ronda se intenta jugar una carta
    }
    private void TryTradeCard(DraggableCard card)
    {//Intenta intercambiar la carta con el deck
        DeckTrade deck = FindObjectsOfType<DeckTrade>().SingleOrDefault(deck => deck.gameObject.Field() == Player.P2 && deck.IsDropValid(card));
        if (deck != null) { deck.OnDropAction(card); }
    }
    private void TryPlayCardsUntilWin()
    {//Intenta jugar cartas hasta que se gane la ronda o se acaben las cartas
        if (Field.P2ForceValue > Field.P1ForceValue) { DoAfterTime(delegate { FinishTurn(); }, 1); }//Si ya es suficiente para ganar la ronda terminamos el turno
        else if (TryPlayOneCard()) { DoAfterTime(delegate { TryPlayCardsUntilWin(); }, 2); }//Si podemos jugar una carta lo hacemos y repetimos este proceso
        else { DoAfterTime(delegate { FinishTurn(); }, 1); }//Si no podemos jugar una carta terminamos el turno
    }
    private bool TryPlayOneCard()
    {//Juega la mejor de las cartas, primero se intenta jugar una carta de unidad, luego de aumento, luego de clima y finalmente de despeje
        DraggableCard chosenCard;
        DropZone chosenZone;
        if (TryPlayUnitCard(out chosenCard, out chosenZone) || TryPlayBoostCard(out chosenCard, out chosenZone)
        || TryPlayWeatherCard(out chosenCard, out chosenZone) || TryPlayClearWeatherCard(out chosenCard, out chosenZone))
        {
            chosenCard.PlayCardIn(chosenZone);
            return true;
        }
        UserRead.Write("P2 elige no jugar en este turno");//Si no se puede jugar ninguna carta
        return false;
    }
    private bool TryPlayUnitCard(out DraggableCard chosenCard, out DropZone chosenZone)
    {//Intenta jugar una carta de unidad en la zona menos afectada por climas
        chosenCard = GetRandomCardFromHand<UnitCard>();
        if (chosenCard == null) { chosenZone = null; return false; }
        //Halla las zonas clima que afectan a las zonas donde la carta se puede jugar. O sea que la coleccion de zonas donde la carta se puede jugar contiene a el segundo objetivo de la zona de clima
        IEnumerable<DZUnit> validDrops = GetValidDrops<DZUnit>(chosenCard);
        IEnumerable<DZWeather> weatherZonesAffecting = FindObjectsOfType<DZWeather>().Where(zone => validDrops.Contains(zone.TargetP2));
        //Si ninguna de las zonas clima hace dano se toma una zona random. Si por lo menos una hace dano se escoge la que sufra menor dano total
        if (weatherZonesAffecting.All(weatherZone => weatherZone.gameObject.CardsInside<WeatherCard>().Sum(weatherCard => weatherCard.Damage) == 0)) { chosenZone = validDrops.RandomElement(); }
        else { chosenZone = weatherZonesAffecting.MinBy(zone => zone.gameObject.CardsInside<WeatherCard>().Sum(weatherCard => weatherCard.Damage)).TargetP2; }
        return true;
    }
    private bool TryPlayBoostCard(out DraggableCard chosenCard, out DropZone chosenZone)
    {//Intenta jugar una carta de aumento en la mejor zona
        chosenCard = GetRandomCardFromHand<BoostCard>();
        if (chosenCard == null) { chosenZone = null; return false; }
        if (Field.PlayerCards.Where(card => card.GetComponent<IAffectable>() != null).Count() == 0) { chosenCard = null; chosenZone = null; return false; }//Si no hay cartas cuyo poder aumentar en el campo
        //Se selecciona de todas las dropzones de cartas de aumento la que tenga como objetivo la dropzone de cartas de unidad de mayor cantidad de cartas afectables
        chosenZone = GetValidDrops<DZBoost>(chosenCard).MaxBy(zone => CountIAffectables(zone.Target));
        return true;
    }
    private bool TryPlayWeatherCard(out DraggableCard chosenCard, out DropZone chosenZone)
    {//Intenta jugar una carta de clima en la mejor zona
        chosenCard = GetRandomCardFromHand<WeatherCard>();
        if (chosenCard == null) { chosenZone = null; return false; }
        //Halla la zona de clima que afecte a la mayor cantidad de cartas de P1 y menor cantidad de cartas de P2
        chosenZone = FindObjectsOfType<DZWeather>().MaxBy(zone => GetAffectedZonesDifference(zone));
        //Si afecta a mas o las mismas cartas de P2 que P1 entonces no se juega el clima
        if (GetAffectedZonesDifference(chosenZone.GetComponent<DZWeather>()) <= 0) { return false; }
        return true;
    }
    private bool TryPlayClearWeatherCard(out DraggableCard chosenCard, out DropZone chosenZone)
    {//Intenta jugar una carta de despeje en la mejor zona
        chosenCard = GetRandomCardFromHand<ClearWeatherCard>();
        if (chosenCard == null) { chosenZone = null; return false; }
        //Ordenamos las zonas de clima por cuanto dano hacen a P2 por encima de P1 y escogemos la que mayor dano haga
        chosenZone = FindObjectsOfType<DZWeather>().MinBy(zone => zone.gameObject.CardsInside<WeatherCard>().Sum(card => card.Damage) * GetAffectedZonesDifference(zone));
        if (chosenZone.gameObject.CardsInside<WeatherCard>().Select(card => card.Damage).Sum() * GetAffectedZonesDifference(chosenZone.GetComponent<DZWeather>()) >= 0) { return false; }
        return true;
    }
    private T GetRandomCardFromHand<T>() where T : DraggableCard
    {//Retorna una carta random de la mano de un tipo pasado, si no hay cartas de ese tipo en la mano devuelve null
        IEnumerable<T> validCards = Hand.PlayerCards.Where(card => card.GetComponent<T>() != null).Cast<T>();
        if (validCards.Count() == 0) { return null; }
        return validCards.RandomElement();
    }
    private IEnumerable<T> GetValidDrops<T>(DraggableCard card) where T : DropZone => FindObjectsOfType<T>().Where(zone => zone.IsDropValid(card));//Devuelve las DropZone donde se puede soltar una carta
    private int GetAffectedZonesDifference(DZWeather zone) => CountIAffectables(zone.TargetP1) - CountIAffectables(zone.TargetP2);//Devuelve la diferencia de la cantidad de IAffectables en las zonas objetivo de la zona clima (P1-P2)
    private int CountIAffectables(DZUnit zone) => zone.gameObject.CardsInside<IAffectable>().Count();//Devuelve la cantidad de cartas afectables de la zona
    private void DoAfterTime(Action action, int time) => StartCoroutine(CorroutineDoAfterTime(action, time));//Realiza la accion luego de que el tiempo pase
    private IEnumerator CorroutineDoAfterTime(Action action, int time) { yield return new WaitForSeconds(time); action(); }
    private void FinishTurn() { UserRead.Write("P2 ha acabado su turno"); Judge.EndTurnOrRound(); isPlaying = false; }//Termina el turno y permite accionar al jugador
}