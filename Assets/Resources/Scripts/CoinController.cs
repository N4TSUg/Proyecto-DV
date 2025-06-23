using UnityEngine;

public class CoinController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    public AudioClip coinSound;
    private DataRepo _gameRepo;
    private Database _gameData;
    private bool _isVisible = true;
    void Start()
    {
        _gameRepo = DataRepo.GetInstance();
        _gameData = _gameRepo.GetData();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagsClass.PLAYER))
        {
            if (!_isVisible) return;

            _gameData.coins++;
            _gameRepo.SaveData();
            _isVisible = false;
            _spriteRenderer.enabled = false;
            _audioSource.PlayOneShot(coinSound);
            Destroy(this.gameObject, coinSound.length);
        }
    }
}
