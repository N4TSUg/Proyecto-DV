using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBoosController : MonoBehaviour
{
    public GameObject _GameOverPanel;
    public GameObject _VictoryPanel;
    public Canvas _Canvas;
    AudioSource _audioSource;
    public AudioClip _audioClipWin;
    private DataRepo _gameRepo;
    private Database _gameData;

    void Start()
    {
        _gameRepo = DataRepo.GetInstance();
        _gameData = _gameRepo.GetData();
        _audioSource = GetComponent<AudioSource>();
        _Canvas.enabled = false;

        _GameOverPanel.GetComponent<SpriteRenderer>().enabled = false;
        _VictoryPanel.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        if (_gameData.gameOver)
        {
            _Canvas.enabled = true;
            _GameOverPanel.GetComponent<SpriteRenderer>().enabled = true;
            _VictoryPanel.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (_gameData.gameCompleted)
        {
            _Canvas.enabled = true;
            _VictoryPanel.GetComponent<SpriteRenderer>().enabled = true;
            _GameOverPanel.GetComponent<SpriteRenderer>().enabled = false;
            if (_audioSource != null && _audioClipWin != null && !_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(_audioClipWin);
            }
        }
        else
        {
            _Canvas.enabled = false;
            _GameOverPanel.GetComponent<SpriteRenderer>().enabled = false;
            _VictoryPanel.GetComponent<SpriteRenderer>().enabled = false;
        }

        // Centrar los paneles en la cámara
        Vector3 camPos = Camera.main.transform.position;
        camPos.z = 0; // Ajusta si tus paneles están en otro plano Z
        _GameOverPanel.transform.position = camPos;
        _VictoryPanel.transform.position = camPos;
    }

    public void MenuGame()
    {
        SceneManager.LoadScene(2);
    }
}
