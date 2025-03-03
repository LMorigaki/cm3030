using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Possible types of cards
/// </summary>
public enum CardType
{
    Building,
    Event
}

/// <summary>
/// possible types of adjacent and event bonus
/// </summary>
public enum BonusType
{
    RatioUpkeep,
    FixedUpkeep,
    RatioBonus,
    FixedBonus
}

/// <summary>
/// represents a bonus provided by a building
/// </summary>
public struct Bonus
{
    /// <summary>
    /// type of bonus
    /// </summary>
    public BonusType type;
    /// <summary>
    /// building type of target which is this bonus applied to
    /// </summary>
    public BuildingType[] targetTypes;
    /// <summary>
    /// relative location from original building and attenuation of bouns<br/>
    /// 0 means building at the position will not gain bonus<br/>
    /// 1 means building at the position will gain 100% effect of bonus<br/>
    /// 0.5 means building at the position will gain 50% effect of bonus<br/>
    /// see SetAdjacentBonus() for more
    /// </summary>
    public float[,] targets;
    /// <summary>
    /// value of effect of bonus
    /// </summary>
    public float value;
    public Bonus(BonusType type, BuildingType[] targetTypes, float[,] targets, float value)
    {
        this.type = type;
        this.targetTypes = targetTypes;
        this.targets = targets;
        this.value = value;
    }
}

/// <summary>
/// represents a event bonus
/// </summary>
public struct EventBonus
{
    /// <summary>
    /// type of bonus
    /// </summary>
    public BonusType type;
    /// <summary>
    /// building type of target which is this bonus applied to
    /// </summary>
    public BuildingType[] targetTypes;
    /// <summary>
    /// value of effect of bonus
    /// </summary>
    public float value;
    /// <summary>
    /// number of turns the effect last for
    /// </summary>
    public byte turns;
    public EventBonus(BonusType type, BuildingType[] targetTypes, float value, byte turns)
    {
        this.type = type;
        this.targetTypes = targetTypes;
        this.value = value;
        this.turns = turns;
    }
}

/// <summary>
/// Class for storing variables of a card
/// </summary>
public class Card
{
    /// <summary>
    /// Card title
    /// </summary>
    public string title;
    /// <summary>
    /// Card description
    /// </summary>
    public string description; 
    /// <summary>
    /// Type of card
    /// </summary>
    public CardType type; 
}

public class BuildingCard : Card
{
    /// <summary>
    /// basic upkeep of this building<br/>
    /// total upkeep = upkeeps * (1 + ratio upkeeps) + fixed upkeeps
    /// </summary>
    public short upkeep;
    /// <summary>
    /// basic profit of this building<br/>
    /// total profit = profits * (1 + ratio bonus) + fixed bonus
    /// </summary>
    public short profit;
    /// <summary>
    /// list of all adjacent bonus provided by this card/building
    /// </summary>
    public Bonus[] bonus;
    /// <summary>
    /// ID of building model in asset database<br/>
    /// </summary>
    public BuildingID buildingID;
    /// <summary>
    /// Type of card
    /// </summary>
    public new readonly CardType type = CardType.Building;

    public BuildingCard(string title, string description, BuildingID id)
    {
        this.title = title;
        this.description = description;
        this.upkeep = 0;
        this.profit = 0;
        this.buildingID = id;
        SetAdjacentBonus();
    }

    /// <summary>
    /// returns a random building card with specified building type
    /// </summary>
    /// <param name="type">type of building</param>
    public static BuildingCard RandomBuildingCard(BuildingType type)
    {
        BuildingID buildingID = ModelManager.GetRandomBuilding(type);
        BuildingCard card = new BuildingCard(
            type.ToString(),
            "Builds a " + type.ToString().ToLower() + " building",
            buildingID
            );
        card.upkeep = 10;
        card.profit = 20;
        return card;
    }

    /// <summary>
    /// sets adjacent bonus of a card by given building
    /// </summary>
    /// <param name="buildingID">building id</param>
    void SetAdjacentBonus()
    {
        switch (buildingID.type)
        {
            case BuildingType.Residential:
                switch (buildingID.id)
                {
                    // define for residential building 10 to 14
                    case ushort n when (n >= 10 && n <= 14):
                        Bonus[] _bonus = new Bonus[]
                        {
                            // add a -10% upkeep bonus to all residential building
                            // 1 tile around this building
                            new Bonus 
                            (
                                BonusType.RatioUpkeep,
                                new BuildingType[] { BuildingType.Residential },
                                new float[,] {
                                                {0, 1, 0},
                                                {1, 0, 1},
                                                {0, 1, 0} 
                                            },
                                -0.1f
                            ),
                            // add a 20% profit bonus to all commercial building
                            // 2 tile around this building, building at 1 tile away,
                            // gains 100% effect bonus whicle building at 2 tiles away,
                            // gains 50% effect of bonus only
                            new Bonus
                            (
                                BonusType.RatioBonus,
                                new BuildingType[] { BuildingType.Commercial },
                                new float[,] {
                                                {   0,    0, 0.5f,    0,    0},
                                                {   0, 0.5f,    1, 0.5f,    0},
                                                {0.5f,    1,    0,    1, 0.5f},
                                                {   0, 0.5f,    1, 0.5f,    0},
                                                {   0,    0, 0.5f,    0,    0}
                                            },
                                0.2f
                            )
                        };
                        this.bonus = _bonus;
                        return;
                }
                break;
            case BuildingType.Commercial:
                switch (buildingID.id)
                {

                }
                break;
            case BuildingType.Industrial:
                switch (buildingID.id)
                {

                }
                break;
        }
        this.bonus = new Bonus[0];
    }
}

public class EventCard : Card
{
    public static readonly int eventCount = 0;

    /// <summary>
    /// ID of event
    /// </summary>
    public byte? eventID;
    /// <summary>
    /// Type of card
    /// </summary>
    public new readonly CardType type = CardType.Event;

    public EventBonus eventBonus;

    public EventCard(string title, string description)
    {
        this.title = title;
        this.description = description;
    }

    public static EventCard RandomEventCard()
    {
        int id = Random.Range(1, eventCount);
        switch (id)
        {
            case 1:
                // increase profit form all buildings for 20% in 2 turns
                EventCard eventCard = new EventCard("Tax increase", 
                    "Increase profit from all building for 20 % in 2 turns");
                eventCard.eventBonus = new EventBonus(
                        BonusType.RatioBonus,
                        new BuildingType[]{
                            BuildingType.Residential,
                            BuildingType.Commercial,
                            BuildingType.Industrial
                        },
                        20,
                        2
                    );

                return eventCard;
            default:
                Debug.LogError("Undefined event card was created");
                break;
        }
        return null;
    }
}