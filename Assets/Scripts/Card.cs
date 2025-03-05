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
/// possible types of affective range of bonus
/// </summary>
public enum BonusRange
{
    /// <summary>
    /// bonus applies to desinated adjacent positions
    /// </summary>
    Adjacent,
    /// <summary>
    /// bonus applies to all at same horizontal line
    /// </summary>
    Horizontal,
    /// <summary>
    /// bonus applies to all at same vertical line
    /// </summary>
    Vertical,
    /// <summary>
    /// bonus applies to all on board
    /// </summary>
    Board
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
    /// affective range of bonus
    /// </summary>
    public BonusRange range;
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
    public Bonus(BonusType type, BuildingType[] targetTypes, BonusRange range, float[,] targets, float value)
    {
        this.type = type;
        this.targetTypes = targetTypes;
        this.range = range;
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

    public BuildingCard(string title, string description, BuildingID id)
    {
        this.title = title;
        this.description = description;
        this.type = CardType.Building;
        this.upkeep = 0;
        this.profit = 0;
        this.buildingID = id;
        Initialise();
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
        return card;
    }

    /// <summary>
    /// returns a random building card
    /// </summary>
    /// <returns></returns>
    public static BuildingCard RandomBuildingCard()
    {
        //BuildingType type = (BuildingType)Random.Range(0, System.Enum.GetValues(typeof(BuildingType)).Length);
        BuildingType type = (BuildingType)Random.Range(0, 2);
        BuildingID buildingID = ModelManager.GetRandomBuilding(type);
        BuildingCard card = new BuildingCard(
            type.ToString(),
            "Builds a " + type.ToString().ToLower() + " building",
            buildingID
            );
        return card;
    }

    /// <summary>
    /// sets profit, upkeep and adjacent bonus of a card
    /// </summary>
    void Initialise()
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
                                BonusRange.Adjacent,
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
                                BonusRange.Adjacent,
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
                        this.upkeep = 10;
                        this.profit = 20;
                        return;

                    // define for residential building 1 to 5
                    case ushort n when (n >= 1 && n <= 5):
                        Bonus[] _bonus15 = new Bonus[]
                        {
                            // add a flat 1 upkeep bonus to all residential building
                            // 1 tile around this building
                            new Bonus
                            (
                                BonusType.FixedUpkeep,
                                new BuildingType[] { BuildingType.Residential },
                                BonusRange.Adjacent,
                                new float[,] {
                                                {0, 1, 0},
                                                {1, 0, 1},
                                                {0, 1, 0}
                                            },
                                -1
                            ),
                            // add a flat 1 profit bonus to all commercial building
                            // 2 tile around this building, building at 1 tile away,
                            // gains 100% effect bonus whicle building at 2 tiles away,
                            // gains 50% effect of bonus only
                            new Bonus
                            (
                                BonusType.FixedBonus,
                                new BuildingType[] { BuildingType.Commercial },
                                BonusRange.Adjacent,
                                new float[,] {
                                                {0, 1, 0},
                                                {1, 0, 1},
                                                {0, 1, 0}
                                            },
                                1
                            )
                        };
                        this.bonus = _bonus15;
                        this.upkeep = 3;
                        this.profit = 5;
                        return;
                }
                break;
            case BuildingType.Commercial:
                switch (buildingID.id)
                {
                    // small commercial
                    case ushort n when (n >= 1 && n <=4):
                        Bonus[] _bonus = new Bonus[]
                        {
                            // increase adjacent residential profit by flat 3
                            new Bonus
                            (
                                BonusType.FixedBonus,
                                new BuildingType[] { BuildingType.Residential },
                                BonusRange.Adjacent,
                                new float[,] {
                                                {0, 1, 0},
                                                {1, 0, 1},
                                                {0, 1, 0}
                                             },
                                1
                            ),
                            // Decrease profit of adjacent commercial building by 20%, increase profit of all other nearby commercial by 20%
                            new Bonus
                            (
                                BonusType.RatioBonus,
                                new BuildingType[] { BuildingType.Residential },
                                BonusRange.Adjacent,
                                new float[,] {
                                                {   0,    0, 1,    0,    0},
                                                {   0, 1,   -1, 1,    0},
                                                {1,    -1,    0,    -1, 1},
                                                {   0, 1,    -1, 1f,    0},
                                                {   0,    0, 1f,    0,    0}
                                             },
                                0.2f
                            )
                        };
                        this.bonus = _bonus;
                        this.upkeep = 20;
                        this.profit = 25;
                        return;

                }
                break;
            case BuildingType.Industrial:
                switch (buildingID.id)
                {
                    // all industrial buildings
                    case ushort n when n >= 0:
                        Bonus[] _bonus = new Bonus[]
                        {
                            // reduce profit generated by residentials by 50% if placed next to a industrial building
                            new Bonus
                            (
                                BonusType.RatioBonus,
                                new BuildingType[] { BuildingType.Residential },
                                BonusRange.Adjacent,
                                new float[,] {
                                                {0.5f, 1, 0.5f},
                                                {1, 0, 1},
                                                {0.5f, 1, 0.5f}
                                             },
                                -0.5f
                            )
                        };
                        this.bonus = _bonus;
                        this.upkeep = 50;
                        this.profit = 15;
                        return;
                }
                break;
        }
        this.bonus = new Bonus[0];
    }
}

public class EventCard : Card
{
    public static readonly int eventCount = 1;

    /// <summary>
    /// ID of event
    /// </summary>
    public byte? eventID;

    public EventBonus eventBonus;

    public EventCard(string title, string description)
    {
        this.title = title;
        this.description = description;
        this.type = CardType.Event;
    }

    public static EventCard RandomEventCard()
    {
        byte id = (byte)Random.Range(1, eventCount);
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