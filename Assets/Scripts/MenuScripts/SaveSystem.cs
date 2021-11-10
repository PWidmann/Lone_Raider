using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveWorld(string _worldName, int[,] _biomeMapArray, int _seed, int _playerX, int _playerY)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.dataPath + "/SaveData/" + _worldName + ".save";

        if (!Directory.Exists(Application.dataPath + "/SaveData"))
        {
            Directory.CreateDirectory(Application.dataPath + "/SaveData");
        }

        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData saveData = new SaveData(_worldName, _biomeMapArray, _seed, _playerX, _playerY);

        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    


    public static SaveData LoadWorld(string _worldName)
    {
        string path = Application.dataPath + "/SaveData/" + _worldName + ".save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return saveData;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
