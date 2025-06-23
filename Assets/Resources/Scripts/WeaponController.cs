using UnityEngine;

public class SwordController : MonoBehaviour
{
    private string _direccion = "Derecha";
    Rigidbody2D _rigidbody;
    SpriteRenderer _spriteRenderer;
    DataRepo _gameRepo;
    Database _gameData;
    public int _damage = 1;
    private AudioSource _audioSource;
    public AudioClip _audioClipSword;

    void Start()
    {
        _gameRepo = DataRepo.GetInstance();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameData = _gameRepo.GetData();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(_audioClipSword);
        Destroy(this.gameObject, 5f);
    }

    void Update()
    {
        if (_direccion == "Derecha")
        {
            _rigidbody.linearVelocityX = 15;
            _spriteRenderer.flipX = false;
        }
        else if (_direccion == "Izquierda")
        {
            _rigidbody.linearVelocityX = -15;
            _spriteRenderer.flipX = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(tagsClass.ENEMY))
        {
            other.gameObject.GetComponent<EnemieController>().RecibirDanio(_damage);
            Destroy(this.gameObject);
        }
        if (other.gameObject.CompareTag(tagsClass.BOOS))
        {
            other.gameObject.GetComponent<ReyHeladoController>().RecibirDanio(_damage);
            Destroy(this.gameObject);
        }
    }

    public void SetDireccion(string direccion)
    {
        this._direccion = direccion;
    }
}
