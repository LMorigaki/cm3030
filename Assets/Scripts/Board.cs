using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// stores and provide functions about buildings, upkeeps, profits and adjacent bonus info on board
/// </summary>
public class Board : MonoBehaviour
{
    /// <summary>
    /// size of the board
    /// </summary>
    readonly int boardSize;
    /// <summary>
    /// placeable objects on board
    /// </summary>
    GameObject[,] tiles;
    /// <summary>
    /// card information of buildings on board
    /// </summary>
    BuildingCard[,] buildingCards;
    /// <summary>
    /// basic upkeep of each tile<br/>
    /// total upkeep = upkeeps * (1 + ratio upkeeps) + fixed upkeeps
    /// </summary>
    int[,] upkeeps;
    /// <summary>
    /// additional upkeeps rate<br/>
    /// total upkeep = upkeeps * (1 + ratio upkeeps) + fixed upkeeps
    /// </summary>
    float[,] ratioUpkeeps;
    /// <summary>
    /// additional upkeeps<br/>
    /// total upkeep = upkeeps * (1 + ratio upkeeps) + fixed upkeeps
    /// </summary>
    float[,] fixedUpkeeps;
    /// <summary>
    /// basic profit of each tile<br/>
    /// total profit = profits * (1 + ratio bonus) + fixed bonus
    /// </summary>
    int[,] profits;
    /// <summary>
    /// additional profit rate<br/>
    /// total profit = profits * (1 + ratio bonus) + fixed bonus
    /// </summary>
    float[,] ratioBonus;
    /// <summary>
    /// addition profit<br/>
    /// total upkeep = upkeeps * (1 + ratio bonus) + fixed bonus
    /// </summary>
    float[,] fixedBonus;

    public Board(int size)
    {
        boardSize = size;
        tiles = new GameObject[size, size];
        buildingCards = new BuildingCard[size, size];
        upkeeps = new int[size, size];
        ratioUpkeeps = new float[size, size];
        fixedUpkeeps = new float[size, size];
        profits = new int[size, size];
        ratioBonus = new float[size, size];
        fixedBonus = new float[size, size];
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

        // assigns upkeep and profit values
        upkeeps[index.x, index.y] = card.upkeep;
        profits[index.x, index.y] = card.profit;
        // updates adjacent bonus
        ApplyMatrix(card.fixedUpkeeps, fixedUpkeeps, index);
        ApplyMatrix(card.ratioUpkeeps, ratioUpkeeps, index);
        ApplyMatrix(card.fixedBonus, fixedBonus, index);
        ApplyMatrix(card.ratioBonus, ratioBonus, index);
    }

    /// <summary>
    /// adds the values from matrix to a target array at specified location
    /// </summary>
    /// <param name="matrix">array from card</param>
    /// <param name="target">target array in board</param>
    /// <param name="index">location of building, converted to array index</param>
    /// <param name="isReduction">if true, removes the value of matrix from target array</param>
    void ApplyMatrix(float[,] matrix, float[,] target, Vector2Int index, bool isReduction = false)
    {
        int fromX = index.x - Mathf.FloorToInt(matrix.GetLength(0) / 2);
        int fromY = index.y - Mathf.FloorToInt(matrix.GetLength(1) / 2);

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                int x = fromX + i;
                int y = fromY + j;
                if (x < 0 || x >= boardSize || y < 0 || y >= boardSize)
                {
                    continue;
                }
                if (isReduction)
                {
                    matrix[i, j] = matrix[i, j] * -1;
                }
                target[x, y] += matrix[i, j];
            }
        }
    }

    /// <summary>
    /// calculates the total upkeep of buildings on board
    /// </summary>
    public float TotalUpkeep()
    {
        // upkeeps[i, j] * (1 + ratio bonus) + fixed bonus
        float sum = 0;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                sum += upkeeps[i, j] * (1 + ratioUpkeeps[i, j]) + fixedUpkeeps[i, j];
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
                sum += profits[i, j] * (1 + ratioBonus[i, j]) + fixedBonus[i, j];
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
    /// convert cell location to array index
    /// </summary>
    /// <param name="location">cell location</param>
    Vector2Int CellToIndex(Vector3Int location)
    {
        int offset = Mathf.FloorToInt(boardSize / 2);
        return new Vector2Int(location.x + offset, location.y + offset);
    }
}
