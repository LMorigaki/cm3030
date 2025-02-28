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
        string _directory = ValidateID(id);
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
    public static Sprite LoadImage(string id)
    {
        string _directory = ValidateID(id);
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
    public static string GetRandomBuilding(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Residential:
                return "r" + Random.Range(1, 22);
            case BuildingType.Commercial:
                return "c" + Random.Range(1, 20);
            case BuildingType.Industrial:
                return "i";
        }
        return "";
    }

    /// <summary>
    /// Validates building id and converts it to directory
    /// </summary>
    /// <param name="id"></param>
    /// <returns>If vaild id, returns directory of building, if invaild id, returns null</returns>
    static string ValidateID(string id)
    {
        string _directory = "building (" + id.Remove(0, 1) + ")";
        if (id[0] == 'r')
        {
            _directory = residentialDir + _directory;
        }
        else if (id[0] == 'c')
        {
            _directory = commercialDir + _directory;
        }
        else if (id[0] == 'i')
        {
            _directory = industrialDir + _directory;
        }
        else
        {
            Debug.LogError("Incorrect format of building ID, expected character r/c/i followed by number get: '" + id + "'");
            return null;
        }
        return _directory;
    }

}
