/// <summary>
/// Possible types of cards
/// </summary>
public enum CardType
{
    Building,
    Event
}

public enum BonusType
{
    RatioUpkeep,
    FixedUpkeep,
    RatioBonus,
    FixedBonus
}

/// <summary>
/// represent a bonus provided by a building
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
    public int upkeep;
    /// <summary>
    /// basic profit of this building<br/>
    /// total profit = profits * (1 + ratio bonus) + fixed bonus
    /// </summary>
    public int profit;
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
    public void SetAdjacentBonus()
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
    /// <summary>
    /// ID of event
    /// </summary>
    public int? eventID;
    /// <summary>
    /// Type of card
    /// </summary>
    public new readonly CardType type = CardType.Event;

    public EventCard(string title, string description)
    {
        this.title = title;
        this.description = description;
    }
}