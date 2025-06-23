using UnityEngine;

public class ReyHeladoController : MonoBehaviour
{
    private int EstadoAnimacion = 0; // 0 = aparicion, 1 = idle, 2 = ataque aerio, 3 = muerte, 4 = daño recibido, 5 = moverse en el aire, 6 = moverse en el suelo
    private bool AparicionAnimacion = true;

    private Animator animator;
    private Transform player;
    private Rigidbody2D rb;

    public GameObject proyectil;
    public int vidaReyHelado = 30;

    [Header("Rangos y velocidades")]
    public float rangoDeteccion = 10f;
    public float rangoAtaque = 5f;
    public float diferenciaAlturaAtaque = 1.5f;
    public float velocidadAire = 4f;
    public float velocidadTierra = 2f;

    private bool jugadorDetectado = false;
    private bool JugadorDetectadoPorPrimeraVez = false;
    private SpriteRenderer spriteRenderer;

    public AudioClip audioClipDanio;
    private AudioSource audioSource;

    private DataRepo _gameRepo;
    private Database _gameData;

    public Transform puntoLanzamientoProyectil;

    void Start()
    {
        _gameRepo = DataRepo.GetInstance();
        _gameData = _gameRepo.GetData();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        BuscarJugador();
    }

    void Update()
    {
        if (_gameData.gameCompleted)
        {
            EstadoAnimacion = 3; // Muerte
            Animacion();
            return;
        }
        DetectarJugador();
        if (!JugadorDetectadoPorPrimeraVez && jugadorDetectado)
        {
            JugadorDetectadoPorPrimeraVez = true;
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
            AparicionAnimacion = true;
            EstadoAnimacion = 0;
            Animacion();
        }
        if (jugadorDetectado && !AparicionAnimacion)
        {
            FlipHaciaJugador();
            AccionesReyHelado();
        }
        else if (!AparicionAnimacion)
        {
            if (rb.linearVelocity.magnitude < 0.1f)
            {
                AnimacionIdle();
            }
        }
    }

    private void FlipHaciaJugador()
    {
        if (player == null) return;
        Vector3 escala = transform.localScale;
        if (player.position.x < transform.position.x)
            escala.x = -Mathf.Abs(escala.x);
        else
            escala.x = Mathf.Abs(escala.x);
        transform.localScale = escala;
    }

    private void BuscarJugador()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tagsClass.PLAYER);
        if (obj != null)
            player = obj.transform;
    }

    public void Animacion()
    {
        animator.SetInteger("Estado", EstadoAnimacion);
        animator.SetBool("Aparicion", AparicionAnimacion);
    }

    public void AnimacionIdle()
    {
        EstadoAnimacion = 1;
        AparicionAnimacion = false;
        Animacion();
    }

    public void DetectarJugador()
    {
        if (player == null)
        {
            BuscarJugador();
            jugadorDetectado = false;
            return;
        }

        float distancia = Vector2.Distance(transform.position, player.position);
        jugadorDetectado = distancia <= rangoDeteccion;
    }

    public void AccionesReyHelado()
    {
        if (player == null) return;

        float diferenciaY = Mathf.Abs(player.position.y - transform.position.y);
        float diferenciaX = Mathf.Abs(player.position.x - transform.position.x);

        if (diferenciaX <= rangoAtaque && diferenciaY < diferenciaAlturaAtaque)
        {
            AtacarJugador();
        }
        else if (player.position.y - transform.position.y > 1f)
        {
            VolarHaciaAlJugador();
        }
        else
        {
            MoverseEnTierraAlJugador();
        }
    }

    public void VolarHaciaAlJugador()
    {
        EstadoAnimacion = 5;
        Animacion();

        Vector2 direccion = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direccion.x * velocidadAire, direccion.y * velocidadAire);
    }

    public void MoverseEnTierraAlJugador()
    {
        EstadoAnimacion = 6;
        Animacion();

        float direccionX = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direccionX * velocidadTierra, rb.linearVelocity.y);
    }

    // Atacar al jugador
    public void AtacarJugador()
    {
        EstadoAnimacion = 2;
        Animacion();
        rb.linearVelocity = Vector2.zero;
    }


    public void LanzarProyectil()
    {
        if (proyectil != null && player != null && puntoLanzamientoProyectil != null)
        {
            GameObject nuevoProyectil = Instantiate(
                proyectil,
                puntoLanzamientoProyectil.position,
                Quaternion.identity
            );
            ProyectilReyHeladoController controlador = nuevoProyectil.GetComponent<ProyectilReyHeladoController>();
            if (controlador != null)
            {
                controlador.LanzarProyectil((Vector2)player.position);
            }
        }
    }

    public void RecibirDanio(int cantidadDanio)
    {
        vidaReyHelado -= cantidadDanio;
        EstadoAnimacion = 4;
        Animacion();

        if (audioSource != null && audioClipDanio != null)
            audioSource.PlayOneShot(audioClipDanio);

        if (vidaReyHelado <= 0)
        {
            Debug.Log("Rey Helado ha sido derrotado");
            _gameData.gameCompleted = true;
            _gameData.EnemigosMuertos++;
            _gameRepo.SaveData();
            EstadoAnimacion = 3;
            Animacion();
        }
    }

    private void OnDrawGizmos()
    {
        // Color para el rango de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);

        // Color para el rango de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);

        // Color para la diferencia de altura de ataque (arriba y abajo)
        Gizmos.color = Color.cyan;
        Vector3 arriba = transform.position + Vector3.up * diferenciaAlturaAtaque;
        Vector3 abajo = transform.position + Vector3.down * diferenciaAlturaAtaque;
        Gizmos.DrawLine(transform.position, arriba);
        Gizmos.DrawLine(transform.position, abajo);

        if (puntoLanzamientoProyectil != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(puntoLanzamientoProyectil.position, 0.18f);
        }

        string estado = "";
        switch (EstadoAnimacion)
        {
            case 0: estado = "Aparición"; Gizmos.color = Color.white; break;
            case 1: estado = "Idle"; Gizmos.color = Color.green; break;
            case 2: estado = "Ataque aéreo"; Gizmos.color = Color.red; break;
            case 3: estado = "Muerte"; Gizmos.color = Color.black; break;
            case 4: estado = "Daño recibido"; Gizmos.color = Color.magenta; break;
            case 5: estado = "Moverse en el aire"; Gizmos.color = Color.blue; break;
            case 6: estado = "Moverse en el suelo"; Gizmos.color = Color.gray; break;
            default: estado = "Desconocido"; Gizmos.color = Color.white; break;
        }
        UnityEditor.Handles.Label(transform.position + Vector3.up * 2, $"Estado: {estado}");
    }
}
