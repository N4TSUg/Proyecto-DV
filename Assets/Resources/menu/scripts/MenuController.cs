using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private GameObject LocationRender;
    private Personaje[] personajes;
    private Database database;
    private DataRepo dataRepo;
    private AudioSource audioSource;
    public AudioClip audioClipBoton;
    private int indiceSeleccionado = 0;
    void Start()
    {
        LocationRender = GameObject.Find("locationRender");
        if (LocationRender == null)
        {
            return;
        }
        new DataSeed();


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
        dataRepo.SaveData();

        audioSource = GetComponent<AudioSource>();
    }

    public void IniciarJuego()
    {
        audioSource.PlayOneShot(audioClipBoton);
        Debug.Log("Iniciar Juego");
        SceneManager.LoadScene(1);
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

    public void SalirJuego()
    {
        audioSource.PlayOneShot(audioClipBoton);
        Application.Quit();
    }
}