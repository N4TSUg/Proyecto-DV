using UnityEngine;
using UnityEngine.UI;

public class GameManagerController : MonoBehaviour
{
    private GameRepo _gameRepo;
    private GameData _gameData;

    public Text EnemigosMuertos;
    public Text EnemigosVivos;
    public Text VidaUsuario;

    [Header("Audio")]
    public bool _gameOver = false;
    private AudioSource _audioSource;
    public AudioClip _audioClipMuerte;

    void Start()
    {
        int enemigos = GameObject.FindGameObjectsWithTag("Enemie").Length;
        _audioSource = GetComponent<AudioSource>();
        _gameRepo = GameRepo.GetInstance();
        _gameData = _gameRepo.GetData();
        _gameData.EnemigosVivos = enemigos;
        _gameRepo.SaveData();
    }

    void Update()
    {
        if (_gameData.vidas <= 0 && _gameOver == false)
        {
            _gameOver = true;
            // detenemos el audio de la musica
            _audioSource.Stop();
            _audioSource.PlayOneShot(_audioClipMuerte);
        }
        EnemigosMuertos.text = $"Enemigos Muertos: {_gameData.EnemigosMuertos}";
        EnemigosVivos.text = $"Enemigos Vivos: {_gameData.EnemigosVivos}";
        VidaUsuario.text = $"Vida del Usuario: {_gameData.vidas}";
    }
}
