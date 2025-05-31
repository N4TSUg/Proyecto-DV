using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class GameRepo
{
    static GameRepo _instance;
    GameData _gameData;

    public static GameRepo GetInstance()
    {
        if (_instance == null) _instance = new GameRepo();

        return _instance;
    }

    public GameData GetData()
    {
        if (_gameData != null) return _gameData;

        string path = Application.persistentDataPath + "/data.save";

        // Verifica existencia y tamaño del archivo
        if (!File.Exists(path) || new FileInfo(path).Length == 0)
        {
            Debug.LogWarning("Archivo de guardado inexistente o vacío. Se crea nuevo GameData.");
            _gameData = new GameData();
            return _gameData;
        }

        try
        {
            using (FileStream file = File.OpenRead(path))
            {
                _gameData = (GameData)new BinaryFormatter().Deserialize(file);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al deserializar: {ex.Message}");
            _gameData = new GameData(); // Fallback
        }

        return _gameData;
    }

    public void SaveData()
    {
        string path = Application.persistentDataPath + "/data.save";
        using (FileStream file = File.Create(path))
        {
            new BinaryFormatter().Serialize(file, _gameData);
        }
    }

}
