using System;
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
    private bool pudeDisparar = true;
    private float tiempoInicioPresion;
    private bool teclaKPresionada = false;


    // DATOS DEL JUGADOR
    private AudioSource _audioSource;
    public AudioClip _audioClipFire;
    public AudioClip _audioMoneda;

    DataRepo _gameRepo;
    Database _gameData;

    private Kits _kit;
    private GameObject _espada1;
    private GameObject _espada2;
    private GameObject _espada3;


    void Start()
    {
        _gameRepo = DataRepo.GetInstance();
        _gameData = _gameRepo.GetData();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        Kits personajeData = Array.Find(_gameData.kits, kit => kit.nombre == _gameData.kitSeleccionado);


        _kit = personajeData;
        _espada1 = _kit.armas[0].CargarArma(_kit.armas[0].tagArma);
        _espada2 = _kit.armas[1].CargarArma(_kit.armas[1].tagArma);
        _espada3 = _kit.armas[2].CargarArma(_kit.armas[2].tagArma);
        _gameData.vidas = PersonajesClass.VIDA_B;
        _gameData.maxVidas = PersonajesClass.VIDA_B;
        _gameRepo.SaveData();
    }

    void Update()
    {
        SetupMoverseHorizontal();
        SetupSalto();
        LanzaEspada();
        animator.SetBool("recibiendoDanio", recibiendoDanio);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == tagsClass.COIN)
        {
            _audioSource.PlayOneShot(_audioMoneda);
        }

        if (tag == tagsClass.LIMITBOTTOM)
        {
            _gameData.vidas = 0;
            _gameRepo.SaveData();
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagsClass.ENEMY))
        {
            EnemieController enemie = collision.gameObject.GetComponent<EnemieController>();
            // Destroy(collision.gameObject);
            int vidaPlayer = _gameData.vidas;
            vidaPlayer -= enemie.damage;
            if (vidaPlayer <= 0)
            {
                Destroy(this.gameObject);
            }
            _gameData.EnemigosVivos = GameObject.FindGameObjectsWithTag(tagsClass.ENEMY).Length;
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
            Espada(_espada2, _kit.armas[1].damage);
        }

        if (Input.GetKeyUp(KeyCode.K) && teclaKPresionada)
        {
            float duracionPresion = Time.time - tiempoInicioPresion;
            teclaKPresionada = false;

            if (duracionPresion >= 2f)
            {
                Espada(_espada3, _kit.armas[2].damage);
                return;
            }
            Espada(_espada1, _kit.armas[0].damage);

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
