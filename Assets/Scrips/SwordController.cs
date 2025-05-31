using UnityEngine;

public class SwordController : MonoBehaviour
{
    private string _direccion = "Derecha";
    Rigidbody2D _rigidbody;
    SpriteRenderer _spriteRenderer;
    GameRepo _gameRepo;
    GameData _gameData;
    public int _damage = 1;


    void Start()
    {
        _gameRepo = GameRepo.GetInstance();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // _playerController = GetComponent<PlayerController>();
        _gameData = _gameRepo.GetData();

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
        if (other.gameObject.CompareTag("Enemie"))
        {
            EnemieController _enemie = other.gameObject.GetComponent<EnemieController>();

            if (_enemie == null) return;

            _enemie.puntosVida -= _damage;

            if (_enemie.puntosVida <= 0) Destroy(other.gameObject);

            Destroy(this.gameObject);
            _gameData.EnemigosMuertos++;
            _gameData.EnemigosVivos = GameObject.FindGameObjectsWithTag("Enemie").Length;
            _gameRepo.SaveData();
        }
    }

    public void SetDireccion(string direccion)
    {
        this._direccion = direccion;
    }
}
