/// <summary>
/// Possible types of cards
/// </summary>
public enum CardType
{
    Building,
    Event
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
    /// additional upkeeps rate<br/>
    /// total upkeep = upkeeps * (1 + ratio upkeeps) + fixed upkeeps
    /// </summary>
    public float[,] ratioUpkeeps;
    /// <summary>
    /// additional upkeeps<br/>
    /// total upkeep = upkeeps * (1 + ratio upkeeps) + fixed upkeeps
    /// </summary>
    public float[,] fixedUpkeeps;
    /// <summary>
    /// basic profit of this building<br/>
    /// total profit = profits * (1 + ratio bonus) + fixed bonus
    /// </summary>
    public int profit;
    /// <summary>
    /// additional profit rate<br/>
    /// total profit = profits * (1 + ratio bonus) + fixed bonus
    /// </summary>
    public float[,] ratioBonus;
    /// <summary>
    /// addition profit<br/>
    /// total upkeep = upkeeps * (1 + ratio bonus) + fixed bonus
    /// </summary>
    public float[,] fixedBonus;

    /// <summary>
    /// ID of building model in asset database<br/>
    /// A string begin with a character r/c/i followed by 2 digit number<br/>
    /// r : residential<br/>
    /// c : commercial<br/>
    /// i : industrial
    /// </summary>
    public string buildingID;
    /// <summary>
    /// Type of card
    /// </summary>
    public new readonly CardType type = CardType.Building;

    public BuildingCard(string title, string description)
    {
        this.title = title;
        this.description = description;
        this.upkeep = 0;
        this.profit = 0;
        this.ratioUpkeeps = new float[0,0];
        this.fixedUpkeeps = new float[0, 0];
        this.ratioBonus = new float[0, 0];
        this.fixedBonus = new float[0, 0];
    }

    /// <summary>
    /// returns a random building card with specified building type
    /// </summary>
    /// <param name="type">type of building</param>
    public static BuildingCard RandomBuildingCard(BuildingType type)
    {
        string buildingID = ModelManager.GetRandomBuilding(type);
        BuildingCard card = new BuildingCard(type.ToString(),
            "Builds a " + type.ToString().ToLower() + " building");
        card.buildingID = buildingID;
        // todo: assign upkeep
        card.upkeep = 10;
        card.profit = 20;
        return card;
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