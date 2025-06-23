using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private string direccion = "Derecha";
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    public float fuerzaRebote = 0.05f; // Ajusta este valor en el Inspector

    [Header("Proyectiles")]
    private bool pudeDisparar = true;
    private bool tranformado = false;

    // DATOS DEL JUGADOR
    private AudioSource _audioSource;
    public AudioClip _audioCDamage;

    DataRepo _gameRepo;
    Database _gameData;

    public GameObject _proyectilAtaque;
    public GameObject _proyectilBasico;
    public GameObject _proyectilUpPower;
    private GameObject _objetoALanzar;
    private int _damageFire;
    private bool recibiendoDanio = false;

    public Transform puntoLanzamientoEspada;


    void Start()
    {
        _gameRepo = DataRepo.GetInstance();
        _gameData = _gameRepo.GetData();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        _gameRepo.SaveData();
    }

    void Update()
    {
        SetupMoverseHorizontal();
        SetupSalto();
        AtaqueDeLargaDistancia();
        AtacarCuerpoACuerpo();
        Transformar();
    }
    void Animaciones(int estado)
    {
        if (HasParameter(animator, "tranformado"))
        {
            animator.SetBool("tranformado", tranformado);
        }
        // 0 = Quieto, 1 = Corriendo, 2 = Saltando, 3 = Atacando, 4 = Recibiendo daño, 5 = ataque especial
        animator.SetInteger("Estado", estado);
    }

    // Helper method to check if an Animator has a parameter
    bool HasParameter(Animator anim, string paramName)
    {
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == tagsClass.LIMITBOTTOM)
        {
            _gameData.vidas = 0;
            _gameRepo.SaveData();
            Destroy(this.gameObject);
        }
    }

    void SetupMoverseHorizontal()
    {
        rb.linearVelocityX = 0;
        if (!recibiendoDanio)
        {
            Animaciones(0);
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
        {
            rb.linearVelocityX = 15;
            sr.flipX = false;
            direccion = "Derecha";
            Animaciones(1);
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
        {
            rb.linearVelocityX = -15;
            sr.flipX = true;
            direccion = "Izquierda";
            Animaciones(1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocityX = 7;
            sr.flipX = false;
            direccion = "Derecha";
            Animaciones(1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocityX = -7;
            sr.flipX = true;
            direccion = "Izquierda";
            Animaciones(1);
        }
    }

    void SetupSalto()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
        {
            rb.linearVelocityY = 12.5f;
            Animaciones(2);
        }
    }

    public void RecibeDanio(Vector2 direction, int cantDanio)
    {
        recibiendoDanio = true;
        Animaciones(4);
        _audioSource.PlayOneShot(_audioCDamage);
        _gameData.vidas -= cantDanio;
        if (_gameData.vidas <= 0)
        {
            _gameData.vidas = 0;
            // desactivamos el sprite del jugador
            gameObject.SetActive(false);
        }
        _gameRepo.SaveData();
        Vector2 rebote = direction.normalized;
        rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
    }
    public void DesactivarDanio()
    {
        recibiendoDanio = false;
        Animaciones(0);
        rb.linearVelocity = Vector2.zero;
    }

    public void Transformar()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            tranformado = !tranformado;
            Animaciones(4);
        }

    }

    public void AtacarCuerpoACuerpo()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            _objetoALanzar = _proyectilAtaque;
            _damageFire = 1;
            Animaciones(3);
        }
    }

    void AtaqueDeLargaDistancia()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Lanzando ataque de larga distancia");
            Animaciones(5);
            if (_gameData.powerUpActivo)
            {
                _objetoALanzar = _proyectilUpPower;
                _damageFire = 3;
                return;
            }
            _objetoALanzar = _proyectilBasico;
            _damageFire = 1;
        }
    }
    public void LanzarAtaque()
    {
        Espada(_objetoALanzar, _damageFire);
    }

    void Espada(GameObject espada, int _damageFire)
    {
        // Instancia la espada en el punto de lanzamiento
        GameObject _espada = Instantiate(espada, puntoLanzamientoEspada.position, Quaternion.identity);

        // Ajusta la posición según el tamaño del sprite y la dirección
        SpriteRenderer sr = _espada.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float ancho = sr.bounds.size.x;
            if (direccion == "Derecha")
                _espada.transform.position += Vector3.right * (ancho / 2f);
            else
                _espada.transform.position += Vector3.left * (ancho / 2f);
        }

        // Asigna el daño y la dirección
        SwordController _EspadaController = _espada.GetComponent<SwordController>();
        if (_EspadaController != null)
        {
            _EspadaController._damage = _damageFire;
            _EspadaController.SetDireccion(direccion);
        }
    }

    void OnDrawGizmos()
    {
        if (puntoLanzamientoEspada != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(puntoLanzamientoEspada.position, 0.15f);
            Gizmos.DrawLine(transform.position, puntoLanzamientoEspada.position);
            UnityEditor.Handles.Label(puntoLanzamientoEspada.position + Vector3.up * 0.3f, "Lanzamiento espada");
        }
    }
}
