using UnityEngine;

public class CoinController : MonoBehaviour
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
            _gameData.coins++;
            _gameRepo.SaveData();
            Destroy(this.gameObject);
        }
    }
}
