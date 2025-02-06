using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TilemapTest : MonoBehaviour
{
	public TileBase tilePlain;
	public TileBase tileHighlighted;
	public int boardSize;
	public Camera cam;
	public EventSystem inputEventSystem;
	
	private Tilemap tilemap;
	private Vector3Int? higTile = null;
	
    void Start() {
		tilemap = GetComponentInChildren<Tilemap>();
		
		// populate tiles
		Vector3Int[] positions = new Vector3Int[boardSize * boardSize];
        TileBase[] tileArray = new TileBase[positions.Length];
		int offset = Mathf.FloorToInt(boardSize / 2);
		
        for (int index = 0; index < positions.Length; index++)
        {
            positions[index] = new Vector3Int((index % boardSize) - offset, (index / boardSize) - offset, 0);
            tileArray[index] = tilePlain;
        }
		
        tilemap.SetTiles(positions, tileArray);
    }
	
    void FixedUpdate() {
		// unset highlighted tile
		if(higTile.HasValue) {
			tilemap.SetTile(higTile.Value, tilePlain);
		}

		// abort if pointer on UI
		if(inputEventSystem.IsPointerOverGameObject()) {
			return;
		}
		
		Vector2 mousePos = Mouse.current.position.ReadValue();
		
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) {
			//Debug.DrawLine(ray.origin, hit.point);
			Vector3 localPos = hit.transform.InverseTransformPoint(hit.point);
			higTile = tilemap.LocalToCell(localPos);
		} else {
			higTile = null;
		}
    }

	void Update() {
		if(higTile.HasValue) {
			tilemap.SetTile(higTile.Value, tileHighlighted);
		}
	}
}
