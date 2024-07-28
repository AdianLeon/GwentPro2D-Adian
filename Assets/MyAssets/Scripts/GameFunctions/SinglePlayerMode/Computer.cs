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

    public void TryPlay()
    {//Cuando se termina algun turno se llama siempre a este metodo
        if (!IsActive) { return; }//Si el modo un solo jugador esta desactivado
        if (Judge.GetPlayer != Player.P2) { return; }//Si no es el turno de P2
        if (!Judge.CanPlay) { return; }//Si no se puede jugar
        Play();
    }
    public void Play()
    {//Toma acciones en dependencia del estado del juego
        isPlaying = true;//Declara que la computadora esta jugando y hasta que no se cambie su valor el jugador no accionar en el juego
        GFUtils.FindGameObjectsOfType<HandCover>().ForEach(cover => cover.gameObject.SetActive(true));//Bloquea la vision del jugador en ambas
        //Si es el inicio del juego intenta intercambiar los senuelos de la mano
        if (Judge.TurnNumber == 2) { Hand.PlayerCards.Where(card => card.GetComponent<BaitCard>() != null).ForEach(card => TryTradeCard(card)); }

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
        if (Field.P2ForceValue > Field.P1ForceValue || Hand.PlayerCards.Count(card => card.GetComponent<BaitCard>() == null) == 0)
        {//Si ya es suficiente para ganar la ronda o no nos quedan cartas por jugar que no sean senuelos, terminamos el turno
            DoAfterTime(delegate { FinishTurn(); }, 1); return;
        }
        if (TryPlayOneCard()) { DoAfterTime(delegate { TryPlayCardsUntilWin(); }, 2); }//Si podemos jugar una carta lo hacemos y repetimos este proceso
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
    {//Juega la carta de unidad de mayor poder en la zona menos afectada por climas
        if (Hand.PlayerCards.Where(card => card.GetComponent<UnitCard>() != null).Count() == 0) { chosenCard = null; chosenZone = null; return false; }
        //Hallando una carta random
        chosenCard = Hand.PlayerCards.Where(card => card.GetComponent<UnitCard>() != null).RandomElement();
        //Halla las zonas clima que afectan a las zonas donde la carta se puede jugar. O sea que las zonas donde la carta se puede jugar contienen a el segundo objetivo de la zona de clima
        DraggableCard aux = chosenCard;//Esta variable auxiliar es para evitar un error en la siguiente linea relativo al uso de out en chosenCard
        IEnumerable<DZUnit> validDrops = FindObjectsOfType<DZUnit>().Where(zone => zone.IsDropValid(aux));
        IEnumerable<DZWeather> weatherZonesAffecting = FindObjectsOfType<DZWeather>().Where(zone => validDrops.Contains(zone.TargetP2));

        if (weatherZonesAffecting.All(weatherZone => weatherZone.gameObject.CardsInside<WeatherCard>().Count() == 0))
        {//Si las zonas clima no hacen dano se toma un elemento random de los drops validos
            chosenZone = validDrops.RandomElement();
        }
        else
        {//Si por lo menos una hace dano se ordenan por cuanto dano sus climas acumulan y se escoge la de menor dano total
            weatherZonesAffecting.OrderBy(zone => zone.gameObject.CardsInside<WeatherCard>().Sum(weatherCard => weatherCard.Damage));
            chosenZone = weatherZonesAffecting.First().TargetP2;
        }
        return true;
    }
    private bool TryPlayBoostCard(out DraggableCard chosenCard, out DropZone chosenZone)
    {//Intenta jugar una carta de aumento en la mejor zona
        if (Hand.PlayerCards.Where(card => card.GetComponent<BoostCard>() != null).Count() == 0) { chosenCard = null; chosenZone = null; return false; }//Si no hay cartas de aumento en la mano
        if (Field.PlayerCards.Where(card => card.GetComponent<IAffectable>() != null).Count() == 0) { chosenCard = null; chosenZone = null; return false; }//Si no hay cartas cuyo poder aumentar en el campo
        chosenCard = Hand.PlayerCards.Where(card => card.GetComponent<BoostCard>() != null).First();//Elige una carta de aumento
        //Se selecciona de todas las dropzones de cartas de aumento la que tenga como objetivo la dropzone de cartas de unidad de mayor cantidad de cartas afectables
        chosenZone = FindObjectsOfType<DZBoost>().Where(zone => zone.gameObject.Field() == Player.P2).OrderByDescending(zone => CountIAffectables(zone.Target)).First();
        return true;
    }
    private bool TryPlayWeatherCard(out DraggableCard chosenCard, out DropZone chosenZone)
    {//Intenta jugar la carta de clima de mayor damage en la mejor zona
        //Se obtienen todas las posibles cartas de clima
        IEnumerable<DraggableCard> validCards = Hand.PlayerCards.Where(card => card.GetComponent<WeatherCard>() != null);
        if (validCards.Count() == 0) { chosenCard = null; chosenZone = null; return false; }//Si no hay
        //Elige a la carta de clima de mayor damage y se halla la zona de clima que afecte a la mayor cantidad de cartas de P1
        chosenCard = validCards.OrderByDescending(card => card.GetComponent<WeatherCard>().Damage).First();
        chosenZone = FindObjectsOfType<DZWeather>().OrderByDescending(zone => CountIAffectables(zone.TargetP1) - CountIAffectables(zone.TargetP2)).First();
        //Si afecta a mas o las mismas cartas de P2 que P1 entonces no se juega el clima
        if (CountIAffectables(chosenZone.GetComponent<DZWeather>().TargetP1) - CountIAffectables(chosenZone.GetComponent<DZWeather>().TargetP2) <= 0) { return false; }
        return true;
    }
    private bool TryPlayClearWeatherCard(out DraggableCard chosenCard, out DropZone chosenZone)
    {//Intenta jugar la carta de clima de mayor damage en la mejor zona
        //Se obtienen todas las posibles cartas de despeje
        IEnumerable<DraggableCard> validCards = Hand.PlayerCards.Where(card => card.GetComponent<ClearWeatherCard>() != null);
        if (validCards.Count() == 0) { chosenCard = null; chosenZone = null; return false; }//Si no hay
        chosenCard = validCards.First();//La carta escogida es la primera, ya que no importa que despeje juguemos
        //Ordenamos las zonas de clima por cuanto dano hacen a P2 por encima de P1
        IEnumerable<DZWeather> validZones = FindObjectsOfType<DZWeather>().OrderByDescending(zone => zone.gameObject.CardsInside<WeatherCard>().Select(card => card.Damage).Sum() * (CountIAffectables(zone.TargetP2) - CountIAffectables(zone.TargetP1)));
        chosenZone = validZones.First();
        if (chosenZone.gameObject.CardsInside<WeatherCard>().Select(card => card.Damage).Sum() * (CountIAffectables(chosenZone.GetComponent<DZWeather>().TargetP2) - CountIAffectables(chosenZone.GetComponent<DZWeather>().TargetP1)) <= 0) { return false; }
        return true;
    }
    private void FinishTurn() { UserRead.Write("P2 ha acabado su turno"); Judge.EndTurnOrRound(); isPlaying = false; }//Termina el turno y permite accionar al jugador
    private int CountIAffectables(DZUnit zone) { return zone.gameObject.CardsInside<IAffectable>().Count(); }//Devuelve la cantidad de cartas afectables de la zona
    private void DoAfterTime(Action action, int time) { StartCoroutine(CorroutineDoAfterTime(action, time)); }//Realiza la accion luego de que el tiempo pase
    private IEnumerator CorroutineDoAfterTime(Action action, int time) { yield return new WaitForSeconds(time); action(); }
}