using UnityEngine;

public class HeartController : MonoBehaviour
{
    private DataRepo _gameRepo;
    private Database _gameData;
    void Start()
    {
        _gameRepo = DataRepo.GetInstance();
        _gameData = _gameRepo.GetData();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagsClass.PLAYER))
        {
            if (_gameData.vidas >= _gameData.maxVidas) return;
            _gameData.vidas++;
            _gameRepo.SaveData();
            Destroy(this.gameObject);
        }
    }
}
