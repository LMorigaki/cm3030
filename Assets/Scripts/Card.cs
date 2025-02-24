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
    public int upkeep;
    /// <summary>
    /// Address of building model in asset database
    /// </summary>
    public string buildingID;
    /// <summary>
    /// Type of card
    /// </summary>
    public CardType type;

    public Card(string title, string description, CardType type, int upkeep)
    {
        this.title = title;
        this.description = description;
        this.upkeep = upkeep;
        this.type = type;
    }
}
