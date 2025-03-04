using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a table storing all bonus from all buildings
/// </summary>
struct BonusTable
{
    private float[,,,] bonuses;

    public BonusTable(int size)
    {
        bonuses = new float[4, size, size, 3];
    }

    public float Get(BonusType bonusType, int x, int y, BuildingType buildingType)
    {
        return bonuses[(int)bonusType, x, y, (int)buildingType];
    }

    public void Set(BonusType bonusType, int x, int y, BuildingType buildingType, float value)
    {
        bonuses[(int)bonusType, x, y, (int)buildingType] = value;
    }
}

/// <summary>
/// a table storing event bonus
/// </summary>
struct EventBonusTable
{
    private float[,] bonuses;

    public EventBonusTable(int x, int y)
    {
        bonuses = new float[x, y];
    }

    public float Get(BonusType bonusType, BuildingType buildingType)
    {
        return bonuses[(int)bonusType, (int)buildingType];
    }

    public void Add(BonusType bonusType, BuildingType buildingType, float value)
    {
        bonuses[(int)bonusType, (int)buildingType] += value;
    }

    public void Remove(BonusType bonusType, BuildingType buildingType, float value)
    {
        bonuses[(int)bonusType, (int)buildingType] -= value;
    }
}

/// <summary>
/// stores and provide functions about buildings, upkeeps, profits and adjacent bonus info on board
/// </summary>
public class Board
{
    /// <summary>
    /// size of the board
    /// </summary>
    readonly byte boardSize;
    /// <summary>
    /// placeable objects on board
    /// </summary>
    GameObject[,] tiles;
    /// <summary>
    /// card information of buildings on board
    /// </summary>
    BuildingCard[,] buildingCards;
    /// <summary>
    /// list of active event cards
    /// </summary>
    List<EventCard> eventCards;
    /// <summary>
    /// a table storing all adjacent bonus from all buildings
    /// </summary>
    BonusTable bonusTable;
    /// <summary>
    /// a table storing all applied event bonus
    /// </summary>
    EventBonusTable eventBonusTable;

    public Board(byte size)
    {
        boardSize = size;
        tiles = new GameObject[size, size];
        buildingCards = new BuildingCard[size, size];
        eventCards = new List<EventCard>();
        bonusTable = new BonusTable(size);
        eventBonusTable = new EventBonusTable(4, 3);
    }

    /// <summary>
    /// assign a building to specified location then update the upkeep and profit table
    /// </summary>
    /// <param name="building"></param>
    public void InsertBuilding(GameObject building, BuildingCard card, Vector3Int location)
    {
        Vector2Int index = CellToIndex(location);
        tiles[index.x, index.y] = building;
        buildingCards[index.x, index.y] = card;

        // updates adjacent bonus
        for (int i = 0; i < card.bonus.Length; i++)
        {
            ApplyBonus(card.bonus[i], index);
        }
    }

    /// <summary>
    /// remove a building on board and all bonus from the building
    /// </summary>
    /// <param name="location">cell location of building</param>
    public void RemoveBuilding(Vector3Int location)
    {
        Vector2Int index = CellToIndex(location);
        BuildingCard card = buildingCards[index.x, index.y];

        for (int i = 0; i < card.bonus.Length; i++)
        {
            ApplyBonus(card.bonus[i], index, true);
        }
        buildingCards[index.x, index.y] = null;
        tiles[index.x, index.y] = null;
    }

    /// <summary>
    /// add event bonus to event bonus table
    /// </summary>
    /// <param name="card"></param>
    public void ApplyEvent(EventCard card)
    {
        eventCards.Add(card);
        for (int i = 0; i < card.eventBonus.targetTypes.Length; i++)
        {
            eventBonusTable.Add(card.eventBonus.type, card.eventBonus.targetTypes[i], card.eventBonus.value);
        }
    }

    /// <summary>
    /// add or remove adjacent bonus to bonus table
    /// </summary>
    /// <param name="bonus">adjacent bonus form card</param>
    /// <param name="index">location of building, converted to array index</param>
    /// <param name="isReduction">if true, removes the value of matrix from target array</param>
    void ApplyBonus(Bonus bonus, Vector2Int index, bool isReduction = false)
    {
        switch (bonus.range)
        {
            case BonusRange.Adjacent:
                int fromX = index.x - Mathf.FloorToInt(bonus.targets.GetLength(0) / 2);
                int fromY = index.y - Mathf.FloorToInt(bonus.targets.GetLength(1) / 2);

                for (int i = 0; i < bonus.targets.GetLength(0); i++)
                {
                    for (int j = 0; j < bonus.targets.GetLength(1); j++)
                    {
                        int x = fromX + i;
                        int y = fromY + j;
                        if (x < 0 || x >= boardSize || y < 0 || y >= boardSize)
                        {
                            continue;
                        }
                        float value = bonus.targets[i, j] * bonus.value;
                        if (isReduction)
                        {
                            value = value * -1;
                        }
                        for (int k = 0; k < bonus.targetTypes.Length; k++)
                        {
                            bonusTable.Set(bonus.type, x, y, bonus.targetTypes[k], value);
                        }
                    }
                }
                break;
            case BonusRange.Horizontal:
                for (int i = 0; i < boardSize; i++)
                {
                    float value = bonus.value;
                    if (isReduction)
                    {
                        value = value * -1;
                    }
                    for (int k = 0; k < bonus.targetTypes.Length; k++)
                    {
                        bonusTable.Set(bonus.type, i, index.y, bonus.targetTypes[k], value);
                    }
                }
                break;
            case BonusRange.Vertical:
                for (int i = 0; i < boardSize; i++)
                {
                    float value = bonus.value;
                    if (isReduction)
                    {
                        value = value * -1;
                    }
                    for (int k = 0; k < bonus.targetTypes.Length; k++)
                    {
                        bonusTable.Set(bonus.type, index.x, i, bonus.targetTypes[k], value);
                    }
                }
                break;
            case BonusRange.Board:
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        float value = bonus.value;
                        if (isReduction)
                        {
                            value = value * -1;
                        }
                        for (int k = 0; k < bonus.targetTypes.Length; k++)
                        {
                            bonusTable.Set(bonus.type, i, j, bonus.targetTypes[k], value);
                        }
                    }
                }
                break;
        }
    }
  
    /// <summary>
    /// calculates the total upkeep of buildings on board
    /// </summary>
    public float TotalUpkeep()
    {
        // upkeeps[i, j] * (1 + ratio bonus + event ratio bonus) + fixed bonus + event fixed bonus
        float sum = 0;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (buildingCards[i, j] == null)
                {
                    continue;
                }
                sum += buildingCards[i, j].upkeep * 
                (1 + bonusTable.Get(BonusType.RatioUpkeep, i, j, buildingCards[i, j].buildingID.type) + 
                eventBonusTable.Get(BonusType.RatioUpkeep, buildingCards[i, j].buildingID.type)) +
                bonusTable.Get(BonusType.FixedUpkeep, i, j, buildingCards[i, j].buildingID.type) + 
                eventBonusTable.Get(BonusType.FixedUpkeep, buildingCards[i, j].buildingID.type);
            }
        }
        return sum;
    }

    /// <summary>
    /// calculates the total profit produced by buildings on board
    /// </summary>
    public float TotalProfit()
    {
        float sum = 0;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (buildingCards[i, j] == null)
                {
                    continue;
                }
                sum += buildingCards[i, j].profit * 
                (1 + bonusTable.Get(BonusType.RatioBonus, i, j, buildingCards[i, j].buildingID.type) + 
                eventBonusTable.Get(BonusType.RatioBonus, buildingCards[i, j].buildingID.type)) +
                bonusTable.Get(BonusType.FixedBonus, i, j, buildingCards[i, j].buildingID.type) + 
                eventBonusTable.Get(BonusType.FixedBonus, buildingCards[i, j].buildingID.type);
            }
        }
        return sum;
    }
    
    /// <summary>
    /// returns true if a tile is occupied
    /// </summary>
    /// <param name="location">cell location</param>
    public bool Occupied(Vector3Int location)
    {
        Vector2Int index = CellToIndex(location);
        return tiles[index.x, index.y] != null;
    }

    /// <summary>
    /// reduce effective turn count and removes all expired event bonus
    /// </summary>
    public void StepAndRemoveEventBonus()
    {
        for (int i = eventCards.Count - 1; i >= 0; i--)
        {
            eventCards[i].eventBonus.turns--;
            if (eventCards[i].eventBonus.turns == 0)
            {
                for (int j = 0; j < eventCards[i].eventBonus.targetTypes.Length; j++)
                {
                    eventBonusTable.Remove
                    (
                        eventCards[i].eventBonus.type,
                        eventCards[i].eventBonus.targetTypes[j],
                        eventCards[i].eventBonus.value
                    );
                }
                eventCards.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// convert cell location to array index
    /// </summary>
    /// <param name="location">cell location</param>
    Vector2Int CellToIndex(Vector3Int location)
    {
        int offset = Mathf.FloorToInt(boardSize / 2);
        return new Vector2Int(location.x + offset, location.y + offset);
    }
}
