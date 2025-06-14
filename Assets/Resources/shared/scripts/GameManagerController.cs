using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerController : MonoBehaviour
{
    private DataRepo _gameRepo;
    private Database _gameData;

    private Text EnemigosMuertos;
    private Text EnemigosVivos;
    private Text VidaUsuario;
    private Text CoinsText;

    [Header("Audio")]
    public bool _gameOver = false;
    private AudioSource _audioSource;
    public AudioClip _audioClipMuerte;
    private GameObject _locationRender;

    void Start()
    {
        EnemigosMuertos = GameObject.Find("EnemigosMuertos").GetComponent<Text>();
        EnemigosVivos = GameObject.Find("EnemigosVivos").GetComponent<Text>();
        VidaUsuario = GameObject.Find("VidaPlayer").GetComponent<Text>();
        CoinsText = GameObject.Find("CoinsText").GetComponent<Text>();
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
        }
        EnemigosMuertos.text = $"Enemigos Muertos: {_gameData.EnemigosMuertos}";
        EnemigosVivos.text = $"Enemigos Vivos: {_gameData.EnemigosVivos}";
        VidaUsuario.text = $"Vida del Usuario: {_gameData.vidas}";
        CoinsText.text = $"Coins: {_gameData.coins}";
    }
}
