using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEditor;

public class TilemapTest : MonoBehaviour
{
	public TileBase tilePlain;
	public TileBase tileHighlighted;
	public const int boardSize = 10;
	public Camera cam;
	public EventSystem inputEventSystem;

	/// <summary>
	/// Directory of building models in asset database
	/// </summary>
	public const string sourceDir = "Assets/External/Models/";
	/// <summary>
	/// An empty object as a folder holding all buildings on board
	/// </summary>
	public Transform buildingFolder;

	private Tilemap tilemap;
	/// <summary>
	/// Cell position of hilighted tile
	/// </summary>
	private Vector3Int? higTile = null;
	/// <summary>
	/// Buildings of each tile on board
	/// </summary>
	private GameObject[,] buildings = new GameObject[boardSize, boardSize];
	/// <summary>
	/// Offset for converting cell position to array index
	/// </summary>
	private readonly int offset = Mathf.FloorToInt(boardSize / 2);

	void Start() {
		tilemap = GetComponentInChildren<Tilemap>();
		
		// populate tiles
		Vector3Int[] positions = new Vector3Int[boardSize * boardSize];
        TileBase[] tileArray = new TileBase[positions.Length];
		//int offset = Mathf.FloorToInt(boardSize / 2);
		
        for (int index = 0; index < positions.Length; index++)
        {
            positions[index] = new Vector3Int((index % boardSize) - offset, (index / boardSize) - offset, 0);
			//positions[index] = new Vector3Int((index % boardSize), (index / boardSize), 0);
			tileArray[index] = tilePlain;
        }
		
        tilemap.SetTiles(positions, tileArray);
		GameObject.Find("Board").transform.position = new Vector3(offset * 10, 0, offset * 10);
    }
	
    void FixedUpdate()
	{
		HighlightTile();
    }

	void Update() {
		if(higTile.HasValue) {
			tilemap.SetTile(higTile.Value, tileHighlighted);
		}

		// Place and remove test
		if (Input.GetKeyDown(KeyCode.Mouse0))
        {
			if (higTile != null)
            {
				PlaceStructure(Random.Range(1, 21), (Vector3Int)higTile);
			}
		}
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			if (higTile != null)
			{
				RemoveStructure((Vector3Int)higTile);
			}
		}
	}

	/// <summary>
	/// Highlight hovered tile 
	/// </summary>
	void HighlightTile()
    {
		// unset highlighted tile
		if (higTile.HasValue)
		{
			tilemap.SetTile(higTile.Value, tilePlain);
		}

		// abort if pointer on UI
		if (inputEventSystem.IsPointerOverGameObject())
		{
			return;
		}

		Vector2 mousePos = Mouse.current.position.ReadValue();

		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		Debug.DrawRay(ray.origin, ray.direction * 10 * boardSize, Color.yellow);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 10 * boardSize))
		{
			//Debug.DrawLine(ray.origin, hit.point);
			Vector3 localPos = hit.transform.InverseTransformPoint(hit.point);
			higTile = tilemap.LocalToCell(localPos);
		}
		else
		{
			higTile = null;
		}
	}

	/// <summary>
	/// Load and place building at specified location
	/// </summary>
	/// <param name="id">building id
	/// <br/>might be modified into string address later</param>
	/// <param name="location">cell position</param>
	public void PlaceStructure(int id, Vector3Int location)
	{
		location = new Vector3Int(location.x + offset, location.y + offset, 0);
		if (buildings[location.x, location.y] != null)
        {
			return;
        }

		string _directry = sourceDir + "house_type" + id.ToString("00") + ".fbx";
		Object structure;
		try
		{
			structure = AssetDatabase.LoadAssetAtPath(_directry, typeof(GameObject));
		}
		catch (System.Exception)
		{
			Debug.LogError("Could not load object at:'" + _directry + "'");
			throw;
		}
		
		Vector3 _vector = new Vector3(location.y * 10 + 5, location.z * 10, location.x * 10 + 5);
		// handle pivot problem on house_type 1 and 4
		if (id == 1 || id == 4)
        {
			_vector.x -= 5;
			_vector.z -= 5;
        }
		GameObject _object = (GameObject)Instantiate(structure, _vector, Quaternion.identity, buildingFolder);
		buildings[location.x, location.y] = _object;
	}

	/// <summary>
	/// Remove and destroy selected building
	/// </summary>
	/// <param name="location">cell position</param>
	public void RemoveStructure(Vector3Int location)
    {
		location = new Vector3Int(location.x + offset, location.y + offset, 0);
		if (buildings[location.x, location.y] == null)
		{
			return;
		}

		GameObject.Destroy(buildings[location.x, location.y]);
		buildings[location.x, location.y] = null;
	}

}
