using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TilemapController : MonoBehaviour
{
    public byte boardSize;
	public Camera cam;
	public EventSystem inputEventSystem;
    public TileBase fillTile;
    public TileBase highlightTile;
    public readonly Vector3Int buildingOffset = new Vector3Int(5, 0, 5);
    public Board board;
    public GameFlowController flowController;

    public event EventHandler<Vector3Int?> TileHighlighted;
    public event EventHandler<Vector3Int?> TileSelected;

    private Tilemap tilemap;
	private Vector3Int? higTile = null;
    

    void Awake()
    {
        board = new Board(boardSize);
        tilemap = GetComponentInChildren<Tilemap>();
    }
	
    public void OnClick(InputValue value)
    {

        if (inputEventSystem.IsPointerOverGameObject())
        {
            return;
        }

        TileSelected?.Invoke(this, higTile);
    }

    public void OnPoint(InputValue value)
    {
        if (inputEventSystem.IsPointerOverGameObject())
        {
            return;
        }
		
        Vector2 pointerPos = value.Get<Vector2>();
        Ray ray = cam.ScreenPointToRay(pointerPos);
        RaycastHit hit;
        Vector3Int? cellPos = null;
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 localPos = hit.transform.InverseTransformPoint(hit.point);
            cellPos = tilemap.LocalToCell(localPos);
        }

		if (cellPos == higTile)
        {
            return;
        }
		else
        {
            HighlightTile(null);
            higTile = cellPos;
            HighlightTile(highlightTile);
            TileHighlighted?.Invoke(this, higTile);
        }
    }

    private void HighlightTile(TileBase tile)
    {
        if (higTile.HasValue)
            tilemap.SetTile(higTile.Value, tile);
    }

    public Vector3Int? GetSelectedTile()
    {
        return higTile;
    }

    public Vector3 CellWorldPosition(Vector3Int cellLoc)
    {
        return tilemap.CellToWorld(cellLoc);
    }

    public bool CanPlace(Vector3Int cellLoc, Vector2Int size)
    {
        bool hasTile = false;
		
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                hasTile &= tilemap.HasTile(cellLoc + new Vector3Int(y, x, 0));
            }
        }

        return !hasTile;
    }

    public Vector3Int[] PlaceTile(Vector3Int cellLoc, Vector2Int size)
    {
        Vector3Int[] positions = new Vector3Int[size.x * size.y];
        TileBase[] tiles = new TileBase[positions.Length];

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int index = (x * size.y) + y;
                positions[index] = cellLoc + new Vector3Int(y, x, 0);
                tiles[index] = fillTile;
            }
        }
        
        tilemap.SetTiles(positions, tiles);

        return positions;
    }

    public void OnPlace(GameObject building, BuildingCard card, Vector3Int location)
    {
        board.InsertBuilding(building, card, location);
        flowController.OnBoardChange();
    }
}
