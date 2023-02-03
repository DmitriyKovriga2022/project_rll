using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadJson<T>
{
    private const string StreamingAssetsPath = "/Resources/Raw/StreamingAssets/";

    // private const string SavedPlayerCharacteristicsPath = "/SavedCharacteristics.json";
    // private const string StartPlayerCharacteristicsPath = "/StartCharacteristics.json";
    // private const string WeaponsCharacteristicsPath = "/SavedWeapons.json";

    //public enum PathName {SavedCharacteristics, StartCharacteristics, SavedWeapons};

    public T LoadData(string pathName)
    {
        string path = Application.dataPath + StreamingAssetsPath + pathName;

        // if (!File.Exists(path))
        // {
        //     path = Application.dataPath + StreamingAssetsPath + StartPlayerCharacteristicsPath;
        // }
        
        T data = JsonUtility.FromJson<T>(File.ReadAllText(path));
        return data;
    }

    public void SaveData(T data, string pathName)
    {
        File.WriteAllText(Application.dataPath + StreamingAssetsPath + pathName, JsonUtility.ToJson(data, true));
    }
}
