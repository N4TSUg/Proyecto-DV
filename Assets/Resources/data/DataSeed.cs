using UnityEngine;

public class DataSeed
{
    private DataRepo _gameRepo;
    private Database _gameData;
    public DataSeed()
    {
        _gameRepo = DataRepo.GetInstance();
        _gameData = _gameRepo.GetData();
        CargarDatos();

    }
    public void CargarDatos()
    {
        // crear lista de armas
        Personaje guerrero = new Personaje("Guerrero", "fin/prefabs/fin");
        Personaje Marcelin = new Personaje("Marcelin", "Marcelin/prefabs/Marcelin");

        // guardar datos
        _gameData.personajes = new Personaje[] { guerrero, Marcelin };
        _gameRepo.SaveData();
        Debug.Log("Datos cargados correctamente");
    }
}
