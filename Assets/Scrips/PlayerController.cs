using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private string direccion = "Derecha";
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    public float fuerzaRebote = 10f;
    private bool puedeSaltar = true;
    private bool recibiendoDanio;

    [Header("Proyectiles")]
    public GameObject espada1;
    public GameObject espada2;
    public GameObject espada3;
    private bool pudeDisparar = true;
    private float tiempoInicioPresion;
    private bool teclaKPresionada = false;




    // DATOS DEL JUGADOR
    public int vidaPlayer;

    private AudioSource _audioSource;
    public AudioClip _audioClipFire;
    GameRepo _gameRepo;
    GameData _gameData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameRepo = GameRepo.GetInstance();
        _gameData = _gameRepo.GetData();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        _gameData.vidas = vidaPlayer;
        _gameRepo.SaveData();
    }

    // Update is called once per frame
    void Update()
    {
        SetupMoverseHorizontal();
        SetupSalto();
        LanzaEspada();
        animator.SetBool("recibiendoDanio", recibiendoDanio);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemie"))
        {
            EnemieController enemie = collision.gameObject.GetComponent<EnemieController>();
            Destroy(collision.gameObject);
            vidaPlayer -= enemie.damage;
            if (vidaPlayer <= 0)
            {
                Destroy(this.gameObject);
            }
            _gameData.EnemigosVivos = GameObject.FindGameObjectsWithTag("Enemie").Length;
            _gameData.vidas = vidaPlayer;
            _gameRepo.SaveData();
        }
    }

    void SetupMoverseHorizontal()
    {
        if (!recibiendoDanio)
            rb.linearVelocityX = 0;
        animator.SetInteger("Estado", 0);

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rb.linearVelocityX = 10;
            sr.flipX = false;
            direccion = "Derecha";
            animator.SetInteger("Estado", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rb.linearVelocityX = -10;
            sr.flipX = true;
            direccion = "Izquierda";
            animator.SetInteger("Estado", 1);
        }
    }

    void SetupSalto()
    {
        if (!recibiendoDanio)
            if (!puedeSaltar) return;
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
        {
            rb.linearVelocityY = 12.5f;
            animator.SetInteger("Estado", 2);
        }
    }

    public void RecibeDanio(Vector2 direction, int cantDanio)
    {
        if (!recibiendoDanio)
        {
            recibiendoDanio = true;
            Vector2 rebote = new Vector2(transform.position.x, 1).normalized;
            rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
        }
    }
    public void DesactivarDanio()
    {
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero;
    }

    void LanzaEspada()
    {
        if (!pudeDisparar) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            tiempoInicioPresion = Time.time;
            teclaKPresionada = true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {

            Espada(espada2, 2);
        }

        if (Input.GetKeyUp(KeyCode.K) && teclaKPresionada)
        {
            float duracionPresion = Time.time - tiempoInicioPresion;
            teclaKPresionada = false;

            GameObject espada = espada1;
            int _damageFire = 1;
            if (duracionPresion >= 2f)
            {
                espada = espada3;
                _damageFire = 3;
            }

            Espada(espada, _damageFire);

        }
    }

    void Espada(GameObject espada, int _damageFire)
    {
        GameObject _espada = Instantiate(espada, transform.position, Quaternion.Euler(0, 0, 0));
        SwordController _ExpadaController = _espada.GetComponent<SwordController>();

        _ExpadaController._damage = _damageFire;
        _ExpadaController.SetDireccion(direccion);

        if (_audioSource != null && _audioClipFire != null)
        {
            _audioSource.PlayOneShot(_audioClipFire);
        }
        else
        {
            Debug.LogWarning("AudioSource o AudioClip no asignado.");
        }
    }
}
