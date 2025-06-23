using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private GameObject LocationRender;
    private Personaje[] personajes;
    private Database database;
    private DataRepo dataRepo;
    private AudioSource audioSource;
    public AudioClip audioClipBoton;
    private int indiceSeleccionado = 0;
    private int EscenaSeleccionada = 0;
    public Text MapaSeleccionado;
    void Start()
    {
        LocationRender = GameObject.Find("locationRender");
        if (LocationRender == null)
        {
            return;
        }
        new DataSeed();

        MapaSeleccionado.text = $"Mapa {EscenaSeleccionada}";


        dataRepo = DataRepo.GetInstance();
        database = dataRepo.GetData();

        personajes = database.personajes;
        if (personajes.Length == 0)
        {
            return;
        }

        indiceSeleccionado = 0;
        personajes[indiceSeleccionado].CargarPersonajeVisual(personajes[indiceSeleccionado].tagPersonaje, LocationRender);
        database.personajeSeleccionado = personajes[indiceSeleccionado].nombre;
        database.gameCompleted = false;
        database.gameOver = false;
        database.vidas = 20;
        database.maxVidas = 20;
        database.coins = 0;
        database.EnemigosMuertos = 0;
        dataRepo.SaveData();

        audioSource = GetComponent<AudioSource>();
    }

    public void IniciarJuego()
    {
        audioSource.PlayOneShot(audioClipBoton);
        Debug.Log("Iniciar Juego");
        SceneManager.LoadScene(EscenaSeleccionada);
    }

    public void nextPersonaje()
    {
        audioSource.PlayOneShot(audioClipBoton);
        indiceSeleccionado++;
        if (indiceSeleccionado >= personajes.Length)
            indiceSeleccionado = 0;

        personajes[indiceSeleccionado].CargarPersonajeVisual(personajes[indiceSeleccionado].tagPersonaje, LocationRender);
        database.personajeSeleccionado = personajes[indiceSeleccionado].nombre;
        dataRepo.SaveData();
    }

    public void previousPersonaje()
    {
        audioSource.PlayOneShot(audioClipBoton);
        indiceSeleccionado--;
        if (indiceSeleccionado < 0)
            indiceSeleccionado = personajes.Length - 1;

        personajes[indiceSeleccionado].CargarPersonajeVisual(personajes[indiceSeleccionado].tagPersonaje, LocationRender);
        database.personajeSeleccionado = personajes[indiceSeleccionado].nombre;
        dataRepo.SaveData();
    }

    // cambia entre esncenas 1 y 2
    public void nextScene()
    {
        audioSource.PlayOneShot(audioClipBoton);
        EscenaSeleccionada++;
        if (EscenaSeleccionada > 1)
            EscenaSeleccionada = 0;
        MapaSeleccionado.text = $"Mapa {EscenaSeleccionada}";
    }

    public void SalirJuego()
    {
        audioSource.PlayOneShot(audioClipBoton);
        Application.Quit();
    }
}