using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(SaveData data)
    {
        string dataJSON = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/save.json", dataJSON);
        Debug.Log(dataJSON);
    }

    public static SaveData Load()
    {
        string dataJSON = File.ReadAllText(Application.dataPath + "/save.json");
        SaveData saveData = JsonUtility.FromJson<SaveData>(dataJSON);

        return saveData;
    }
}
