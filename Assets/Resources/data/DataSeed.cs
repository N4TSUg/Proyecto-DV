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
        // cargar armas
        Armas espada1 = new Armas(Weapons.CLASE_B, Weapons.DAMAGE_B);
        Armas espada2 = new Armas(Weapons.CLASE_A, Weapons.DAMAGE_A);
        Armas espada3 = new Armas(Weapons.CLASE_S, Weapons.DAMAGE_S);
        // crear lista de armas
        Armas[] listaArmas = new Armas[] { espada1, espada2, espada3 };
        // cargar kits
        Kits kitGuerrero = new Kits("Guerrero", listaArmas);
        // cargar personajes
        Personaje guerrero = new Personaje("Guerrero", "fin/prefabs/fin", kitGuerrero);

        // guardar datos
        _gameData.kits = new Kits[] { kitGuerrero };
        _gameData.personajes = new Personaje[] { guerrero };
        _gameRepo.SaveData();
        Debug.Log("Datos cargados correctamente");
    }
}
