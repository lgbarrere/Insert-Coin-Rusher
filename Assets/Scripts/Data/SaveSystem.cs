using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static readonly string path = Application.persistentDataPath + "/data.save";
    public static void SaveData(int score)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(path, FileMode.Create);
        GameData data = new(score);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadData()
    {
        if (!File.Exists(path))
        {
            SaveData(0);
        }
        BinaryFormatter formatter = new();
        FileStream stream = new(path, FileMode.Open);
        GameData data = formatter.Deserialize(stream) as GameData;
        stream.Close();

        return data;
    }
}
