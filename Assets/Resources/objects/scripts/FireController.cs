using UnityEngine;
using System.Timers;
using System.Collections;

public class FireController : MonoBehaviour
{
    private Database _gameData;
    private DataRepo _gameRepo;
    private GameObject _player;
    private bool _isPlayerInTrigger = false;
    private bool _puedeHacerDaño = true;
    void Start()
    {
        _gameRepo = DataRepo.GetInstance();
        _gameData = _gameRepo.GetData();
    }

    void Update()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag(tagsClass.PLAYER);
            if (_player == null)
            {
                Debug.LogError("Player not found in the scene.");
                return;
            }
        }
        if (_isPlayerInTrigger && _puedeHacerDaño)
        {
            StartCoroutine(HacerDañoConRetraso());
        }
    }

    private IEnumerator HacerDañoConRetraso()
    {
        _puedeHacerDaño = false;

        _gameData.vidas--;
        if (_gameData.vidas <= 0)
        {
            _gameData.vidas = 0;
            Destroy(_player);
        }

        _gameRepo.SaveData();

        yield return new WaitForSeconds(1f);
        _puedeHacerDaño = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagsClass.PLAYER))
        {
            _isPlayerInTrigger = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagsClass.PLAYER))
        {
            _isPlayerInTrigger = false;
        }
    }
}
