using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Residential,
    Commercial,
    Industrial
}

public struct BuildingID
{
    public BuildingType type;
    public ushort id;
    public override string ToString()
    {
        string str = "";
        switch (type)
        {
            case BuildingType.Residential:
                str += "r";
                break;
            case BuildingType.Commercial:
                str += "c";
                break;
            case BuildingType.Industrial:
                str += "i";
                break;
            default:
                break;
        }
        str += id;
        return str;
    }
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
    /// Amount of residential building models in asset database
    /// </summary>
    public const ushort residentialCount = 21;
    /// <summary>
    /// Amount of commercial building models in asset database
    /// </summary>
    public const ushort commercialCount = 19;
    /// <summary>
    /// Amount of industrial building models in asset database
    /// </summary>
    public const ushort industrialCount = 7;

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
    public static GameObject LoadStructure(BuildingID id)
    {
        string _directory = GetDirectory(id);
        if (_directory == null)
        {
            return null;
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
    /// Loads image of a specific building from asset database
    /// </summary>
    /// <param name="id">building id</param>
    /// <returns></returns>
    public static Sprite LoadImage(BuildingID id)
    {
        string _directory = GetDirectory(id);
        if (_directory == null)
        {
            return null;
        }

        string[] vs = _directory.Split(' ');
        _directory = vs[0] + " img " + vs[1];
        Sprite img;
        try
        {
            img = Resources.Load<Sprite>(_directory);
        }
        catch (System.Exception)
        {
            Debug.LogError("Could not load sprite at 'Asset/Resources/" + _directory + "'");
            throw;
        }
        return img;
    }

    /// <summary>
    /// Return random building id with specified type
    /// </summary>
    /// <param name="type">building type</param>
    public static BuildingID GetRandomBuilding(BuildingType type)
    {
        BuildingID id;
        id.type = type;
        id.id = 0;
        switch (type)
        {
            case BuildingType.Residential:
                id.id = (ushort)Random.Range(1, residentialCount + 1);
                break;
            case BuildingType.Commercial:
                id.id = (ushort)Random.Range(1, commercialCount + 1);
                break;
            case BuildingType.Industrial:
                id.id = (ushort)Random.Range(1, industrialCount + 1);
                break;
        }
        return id;
    }

    /// <summary>
    /// Return random building id with random building type by weighted random
    /// </summary>
    public static BuildingID GetRandomBuilding()
    {
        BuildingID id = new BuildingID();

        // weighted random
        ushort key = (ushort)Random.Range(0, residentialCount + commercialCount + industrialCount);
        if (key < residentialCount)
        {
            id.type = BuildingType.Residential;
        }
        else if (key < residentialCount + commercialCount)
        {
            id.type = BuildingType.Commercial;
        }
        else
        {
            id.type = BuildingType.Industrial;
        }

        switch (id.type)
        {
            case BuildingType.Residential:
                id.id = (ushort)Random.Range(1, residentialCount + 1);
                break;
            case BuildingType.Commercial:
                id.id = (ushort)Random.Range(1, commercialCount + 1);
                break;
            case BuildingType.Industrial:
                id.id = (ushort)Random.Range(1, industrialCount + 1);
                break;
        }
        return id;
    }

    /// <summary>
    /// Obtain directory from building id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>If vaild id, returns directory of building, if invaild id, returns null</returns>
    static string GetDirectory(BuildingID id)
    {
        string _directory = "building (" + id.id + ")";
        switch (id.type)
        {
            case BuildingType.Residential:
                _directory = residentialDir + _directory;
                break;
            case BuildingType.Commercial:
                _directory = commercialDir + _directory;
                break;
            case BuildingType.Industrial:
                _directory = industrialDir + _directory;
                break;
            default:
                Debug.LogError("Incorrect format of building ID, get: '" + id.ToString() + "'");
                return null;
        }
        return _directory;
    }

}
