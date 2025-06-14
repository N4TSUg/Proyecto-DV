using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class DataRepo
{
    static DataRepo _instance;
    Database _gameData;

    public static DataRepo GetInstance()
    {
        if (_instance == null) _instance = new DataRepo();

        return _instance;
    }

    public Database GetData()
    {
        if (_gameData != null) return _gameData;

        string path = Application.persistentDataPath + "/data.json";

        // Verifica existencia y tamaño del archivo
        if (!File.Exists(path) || new FileInfo(path).Length == 0)
        {
            Debug.LogWarning("Archivo de guardado inexistente o vacío. Se crea nuevo Database.");
            _gameData = new Database();
            return _gameData;
        }

        try
        {
            using (FileStream file = File.OpenRead(path))
            {
                _gameData = (Database)new BinaryFormatter().Deserialize(file);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al deserializar: {ex.Message}");
            _gameData = new Database(); // Fallback
        }

        return _gameData;
    }

    public void SaveData()
    {
        string path = Application.persistentDataPath + "/data.json";
        using (FileStream file = File.Create(path))
        {
            new BinaryFormatter().Serialize(file, _gameData);
        }
    }

}
