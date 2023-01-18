using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadJson<T>
{
    private const string StreamingAssetsPath = "/Resources/Raw/StreamingAssets";
    private const string SavedCharacteristicsPath = "/SavedCharacteristics.json";
    private const string StartCharacteristicsPath = "/StartCharacteristics.json";

    public T LoadData()
    {
        string path = Application.dataPath + StreamingAssetsPath + SavedCharacteristicsPath;

        if (!File.Exists(path))
        {
            path = Application.dataPath + StreamingAssetsPath + StartCharacteristicsPath;
        }

        T data = JsonUtility.FromJson<T>(File.ReadAllText(path));
        return data;
    }

    public void SaveData(T data)
    {
        File.WriteAllText(Application.dataPath + SavedCharacteristicsPath, JsonUtility.ToJson(data));
    }
}
