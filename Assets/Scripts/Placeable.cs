using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Placeable : MonoBehaviour
{
    public TilemapController tilemap;
    public bool Placed = false;
    public Vector2Int Size;
    public Vector3Int[] Location;
    public CardBehaviour cardBehaviour;

	private bool rotated = false;
    private Vector3 posOffset = Vector3.zero;
    private Vector3 offsetRotation;


    void Start()
    {
        tilemap = GameObject.FindGameObjectWithTag("TilemapController").GetComponent<TilemapController>();
        tilemap.TileSelected += HandleTileSelected;
        tilemap.TileHighlighted += HandleTileHighlighted;

        posOffset = tilemap.buildingOffset;
        offsetRotation = Vector3.right * transform.localScale.y;
    }

    void HandleTileSelected(object sender, Vector3Int? cellPos)
    {
        if (!cellPos.HasValue)
        {
            // cancel placing building
            OnPlaceCancelled();
            return;
        }

        bool canPlace = tilemap.CanPlace(cellPos.Value, Size);

		if (canPlace)
        {
            // confirm placing building
            OnPlace(cellPos);
        }
		else
        {
            // cancel placing building
            OnPlaceCancelled();
        }
    }

    void HandleTileHighlighted(object sender, Vector3Int? cellPos)
    {
        if (!cellPos.HasValue)
        {
			GetComponent<Renderer>().enabled = false;
            return;
        }
		
        transform.position = tilemap.CellWorldPosition(cellPos.Value) + posOffset;
		GetComponent<Renderer>().enabled = true;

		bool canPlace = tilemap.CanPlace(cellPos.Value, Size);
        if (!canPlace)
        {
			// tint object red
        }
    }

    void OnPlace(Vector3Int? cellPos)
    {
        // paint tiles in location
        Location = tilemap.PlaceTile(cellPos.Value, Size);

        // set placed to true
        Placed = true;
        transform.position = tilemap.CellWorldPosition(cellPos.Value) + posOffset;

        // remove event handlers
        tilemap.TileSelected -= HandleTileSelected;
        tilemap.TileHighlighted -= HandleTileHighlighted;

        // remove unused compoment
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<PlayerInput>());
        // instantiate building as child
        tilemap.PlaceStructure(transform, cardBehaviour.card.buildingID);

        GameObject.FindGameObjectWithTag("DeckFolder").GetComponent<DeckController>().ShowDeck();
        cardBehaviour.OnPlace();
        cardBehaviour = null;
    }

    void OnPlaceCancelled()
    {
        GameObject.FindGameObjectWithTag("DeckFolder").GetComponent<DeckController>().ShowDeck();
        cardBehaviour.OnPlaceCancelled();
        Destroy(gameObject);
    }

    void Rotate()
    {
        if (rotated)
        {
            posOffset = Vector3.zero;
			transform.Rotate(0.0f, 90.0f, 0.0f, Space.World);
            transform.position -= offsetRotation;
        }
		else
        {
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.World);
            posOffset = offsetRotation;
            transform.position += offsetRotation;
        }

        rotated = !rotated;
        Size = new Vector2Int(Size.y, Size.x);
    }

    void Update()
    {
		
    }

	public void OnRotate(InputValue value) {
        float pressed = value.Get<float>();
		
        if (pressed == 1.0f && !Placed)
        {
            Rotate();
        }
    }

    private void OnDestroy()
    {
		tilemap.TileSelected -= HandleTileSelected;
        tilemap.TileHighlighted -= HandleTileHighlighted;
    }
}
