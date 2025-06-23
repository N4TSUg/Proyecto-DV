using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerController : MonoBehaviour
{
    private DataRepo _gameRepo;
    private Database _gameData;

    private Text EnemigosMuertos;
    private Text VidaUsuario;
    private Text CoinsText;
    private Text PowerUpText;

    [Header("Audio")]
    public bool _gameOver = false;
    private AudioSource _audioSource;
    public AudioClip _audioClipMuerte;
    private GameObject _locationRender;

    private float tiempoHabilidadRestante = 0f;
    private bool habilidadEnCuentaAtras = false;
    private float tiempoHabilidad = 10f;
    private int vidaAnterior = -1;

    void Start()
    {
        EnemigosMuertos = GameObject.Find("EnemigosMuertos").GetComponent<Text>();
        VidaUsuario = GameObject.Find("VidaPlayer").GetComponent<Text>();
        CoinsText = GameObject.Find("CoinsText").GetComponent<Text>();
        PowerUpText = GameObject.Find("PowerUpText").GetComponent<Text>();
        _locationRender = GameObject.Find("locationRender");
        int enemigos = GameObject.FindGameObjectsWithTag(tagsClass.ENEMY).Length;
        _audioSource = GetComponent<AudioSource>();
        _gameRepo = DataRepo.GetInstance();
        _gameData = _gameRepo.GetData();

        Personaje personaje = Array.Find(_gameData.personajes, p => p.nombre == _gameData.personajeSeleccionado);

        personaje.CargarPersonaje(personaje.tagPersonaje, _locationRender);

        _gameData.EnemigosVivos = enemigos;
        _gameRepo.SaveData();

    }

    void Update()
    {
        if (_gameData.vidas <= 0 && _gameOver == false)
        {
            _gameOver = true;
            _audioSource.Stop();
            _audioSource.PlayOneShot(_audioClipMuerte);
            _gameData.gameOver = true;
            _gameData.gameCompleted = false;
            _gameRepo.SaveData();
        }

        if (_gameData.powerUpActivo && !habilidadEnCuentaAtras)
        {
            habilidadEnCuentaAtras = true;
            tiempoHabilidadRestante = tiempoHabilidad;
            StartCoroutine(DesactivarhabilidadEspecial());
        }

        if (habilidadEnCuentaAtras && tiempoHabilidadRestante > 0f)
        {
            tiempoHabilidadRestante -= Time.deltaTime;
            if (tiempoHabilidadRestante <= 0f)
            {
                tiempoHabilidadRestante = 0f;
            }
        }

        EnemigosMuertos.text = $"{_gameData.EnemigosMuertos}";
        VidaUsuario.text = $"Vida del Usuario: {_gameData.vidas}";
        CoinsText.text = $"{_gameData.coins}";
        PowerUpText.text = $"{tiempoHabilidadRestante:F1}s";

        if (vidaAnterior != _gameData.vidas)
        {
            vidaAnterior = _gameData.vidas;
        }
    }

    IEnumerator DesactivarhabilidadEspecial()
    {
        yield return new WaitForSeconds(tiempoHabilidad);
        _gameData.powerUpActivo = false;
        _gameRepo.SaveData();
        habilidadEnCuentaAtras = false;
    }
}
