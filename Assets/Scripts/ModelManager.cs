using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    residential,
    commercial,
    industrial
}

/// <summary>
/// Provides function for loading building models from asset database
/// </summary>
public static class ModelManager
{
    /// <summary>
	/// Directory of residential building models in asset database
	/// </summary>
    public const string residentialDir = "Residential/";

    /// <summary>
    /// Loads a random building from given type of buildings
    /// </summary>
    /// <param name="type">Type of structure</param>
    /// <param name="id">If given, loads a specified structure</param>
    public static GameObject LoadStructure(BuildingType type, int? id = null)
    {
        // Get random structure when id is not specified
        if (id is null)
        {
            switch (type)
            {
                case BuildingType.residential:
                    id = Random.Range(1, 21);
                    break;
                case BuildingType.commercial:
                    // todo: handle random id for commercial building
                    break;
                case BuildingType.industrial:
                    // todo: handle random id for industrial building
                    break;
                default:
                    break;
            }
        }

        string _directory = "house_type" + ((int)id).ToString("00");
        switch (type)
        {
            case BuildingType.residential:
                _directory = residentialDir + _directory;
                break;
            case BuildingType.commercial:
                _directory = "";
                break;
            case BuildingType.industrial:
                _directory = "";
                break;
            default:
                break;
        }
        GameObject structure;
        structure = Resources.Load<GameObject>(_directory);

        return structure;
    }
}
