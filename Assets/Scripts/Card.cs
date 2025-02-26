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
    /// Upkeep cost of building
    /// </summary>
    public int? upkeep;
    /// <summary>
    /// ID of building model in asset database<br/>
    /// A string begin with a character r/c/i followed by 2 digit number<br/>
    /// r : residential<br/>
    /// c : commercial<br/>
    /// i : industrial
    /// </summary>
    public string buildingID;
    /// <summary>
    /// ID of event
    /// </summary>
    public int? eventID;
    /// <summary>
    /// Type of card
    /// </summary>
    public CardType type;

    public Card(string title, string description, CardType type)
    {
        this.title = title;
        this.description = description;
        this.type = type;
    }

    public static Card RandomBuildingCard(BuildingType type)
    {
        string buildingID = ModelManager.GetRandomBuilding(type);
        Card card = new Card(type.ToString(),
            "Builds a " + type.ToString().ToLower() + " building",
            CardType.Building);
        card.buildingID = buildingID;
        // todo: assign upkeep
        return card;
    }
}
