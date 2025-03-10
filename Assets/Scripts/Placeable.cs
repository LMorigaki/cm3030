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
    [HideInInspector]
    public CardBehaviour cardBehaviour;
    public Material matCanPlace, matCannotPlace;

	private bool rotated = false;
    private Vector3 posOffset = Vector3.zero;
    private Vector3 offsetRotation;
    /// <summary>
    /// flag indicating if building is rendered as placeable object<br/>
    /// this flag is used to reduce times of assgning materials to mesh renderer
    /// </summary>
    bool displayPlaceable = true;
    /// <summary>
    /// building as child of this object
    /// </summary>
    GameObject structure;
    /// <summary>
    /// mesh renderer of building
    /// </summary>
    Renderer meshRenderer;
    /// <summary>
    /// origianl materials of building
    /// </summary>
    Material[] materials;

    void Start()
    {
        tilemap = GameObject.FindGameObjectWithTag("TilemapController").GetComponent<TilemapController>();
        tilemap.TileSelected += HandleTileSelected;
        tilemap.TileHighlighted += HandleTileHighlighted;

        posOffset = tilemap.buildingOffset;
        offsetRotation = Vector3.right * transform.localScale.y;

        // load and assign building as child of this object
        structure = ModelManager.LoadStructure(((BuildingCard)cardBehaviour.card).buildingID);
        Instantiate<GameObject>(structure, transform);
        // save original materials of building and assign transparent meterial
        meshRenderer = GetComponentInChildren<Renderer>();
        materials = meshRenderer.sharedMaterials;
        SetMaterials(matCanPlace);
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
            return;
        }
		
        transform.position = tilemap.CellWorldPosition(cellPos.Value) + posOffset;

        //bool canPlace = tilemap.CanPlace(cellPos.Value, Size);
        bool canPlace = !tilemap.board.Occupied(cellPos.Value);
        // reduce assignment of materials
        if (canPlace != displayPlaceable)
        {
            if (canPlace)
            {
                SetMaterials(matCanPlace);
                displayPlaceable = true;
            }
            else
            {
                SetMaterials(matCannotPlace);
                displayPlaceable = false;
            }
        }
    }

    void OnPlace(Vector3Int? cellPos)
    {
        if (tilemap.board.Occupied(cellPos.Value))
        {
            return;
        }

        // paint tiles in location
        Location = tilemap.PlaceTile(cellPos.Value, Size);

        // set placed to true
        Placed = true;
        transform.position = tilemap.CellWorldPosition(cellPos.Value) + posOffset;

        // remove event handlers
        tilemap.TileSelected -= HandleTileSelected;
        tilemap.TileHighlighted -= HandleTileHighlighted;

        // remove unused compoment
        Destroy(GetComponent<PlayerInput>());

        // restore original materials
        meshRenderer.materials = materials;

        GameObject.FindGameObjectWithTag("DeckFolder").GetComponent<DeckController>().ShowDeck();
        tilemap.OnPlace(gameObject, (BuildingCard)(cardBehaviour.card), cellPos.Value);
        cardBehaviour.OnPlace();
        cardBehaviour = null;
    }

    void OnPlaceCancelled()
    {
        GameObject.FindGameObjectWithTag("DeckFolder").GetComponent<DeckController>().ShowDeck();
        cardBehaviour.OnPlaceCancelled();
        Destroy(gameObject);
    }

    /// <summary>
    /// sets all materials of building to single specified material
    /// </summary>
    /// <param name="material">specified material</param>
    void SetMaterials(Material material)
    {
        Material[] _materials = meshRenderer.sharedMaterials;
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = material;
        }
        meshRenderer.materials = _materials;
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
