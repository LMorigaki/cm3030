using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Residential,
    Commercial,
    Industrial
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
	/// Directory of commercial building models in asset database
	/// </summary>
    public const string commercialDir = "Commercial/";
    /// <summary>
	/// Directory of industrial building models in asset database
	/// </summary>
    public const string industrialDir = "Industrial/";

    /// <summary>
    /// Loads a specific building from asset database
    /// </summary>
    /// <param name="id">building id<br/>
    /// character r/c/i followed by 2 digit number<br/>
    /// r : residential<br/>
    /// c : commercial<br/>
    /// i : industrial
    /// </param>
    /// <returns>building gameobject,
    /// if loading failed, returns empty gameobject</returns>
    public static GameObject LoadStructure(string id)
    {
        if (id.Length != 3)
        {
            Debug.LogError("Incorrect length of building ID, expected 3 get: '" + id.Length + "'");
            return new GameObject();
        }

        string _directory = "house_type" + id.Substring(1, 2);
        if (id[0] == 'r')
        {
            _directory = residentialDir + _directory;
        }
        else if (id[0] == 'c')
        {
            _directory = residentialDir + _directory;
        }
        else if (id[0] == 'i')
        {
            _directory = residentialDir + _directory;
        }
        else
        {
            Debug.LogError("Incorrect format of building ID, expected character r/c/i followed by 2 digit number get: '" + id + "'");
            return new GameObject();
        }

        GameObject structure;
        try
        {
            structure = Resources.Load<GameObject>(_directory);
        }
        catch (System.Exception)
        {
            Debug.LogError("Could not load building at 'Asset/Resources/" + _directory + "'");
            throw;
        }
        return structure;
    }

    /// <summary>
    /// Return random building id with specified type
    /// </summary>
    /// <param name="type">building type</param>
    public static string GetRandomBuilding(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Residential:
                return "r" + Random.Range(1, 22).ToString("00");
            case BuildingType.Commercial:
                return "c";
            case BuildingType.Industrial:
                return "i";
        }
        return "";
    }

}
