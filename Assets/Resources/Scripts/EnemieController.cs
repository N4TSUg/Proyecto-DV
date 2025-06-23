using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class EnemieController : MonoBehaviour
{
    [Header("Enemie Properties")]
    public int damage = 1;
    public float detectionRadius = 5.0f;
    public float speed = 5.0f;
    public int puntosVida = 3;

    [Header("Animation Properties")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isRunning = false;
    private bool isAttacking = false;

    [Header("Detection Settings")]
    public float margenY = 0.5f;

    private Rigidbody2D rb;
    private Transform player;

    private DataRepo dataRepo;
    private Database gameData;

    private void Awake()
    {
        dataRepo = DataRepo.GetInstance();
        gameData = dataRepo.GetData();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (gameData.gameOver || gameData.gameCompleted)
        {
            AnimacionIdle();
            return;
        }
        DetectarJugador();
        if (player != null)
        {
            DirigirseAlJugador();
        }
        else
        {
            // Si no hay jugador, asegúrate de estar en idle
            if (isRunning || isAttacking)
            {
                AnimacionIdle();
            }
        }
    }

    // --- Animaciones ---
    private void ActualizarAnimaciones()
    {
        animator.SetBool("run", isRunning);
        animator.SetBool("atack", isAttacking);
    }

    private void AnimacionAtacando()
    {
        isAttacking = true;
        isRunning = false;
        ActualizarAnimaciones();
    }

    private void AnimacionCorriendo()
    {
        isAttacking = false;
        isRunning = true;
        ActualizarAnimaciones();
    }

    private void AnimacionIdle()
    {
        isAttacking = false;
        isRunning = false;
        ActualizarAnimaciones();
    }

    // --- Detección de jugador ---
    private void DetectarJugador()
    {
        bool detectado = false;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            transform.position,
            new Vector2(detectionRadius * 2, margenY * 2),
            0f
        );
        foreach (var col in colliders)
        {
            if (col.CompareTag(tagsClass.PLAYER))
            {
                float diferenciaY = Mathf.Abs(col.transform.position.y - transform.position.y);
                float distanciaX = Mathf.Abs(col.transform.position.x - transform.position.x);
                if (distanciaX <= detectionRadius && diferenciaY < margenY)
                {
                    player = col.transform;
                    detectado = true;
                    break;
                }
            }
        }
        if (!detectado)
        {
            player = null;
        }
    }

    // --- Movimiento hacia el jugador ---
    private void DirigirseAlJugador()
    {
        if (player == null) return;

        float distanciaX = Mathf.Abs(player.position.x - transform.position.x);
        float diferenciaY = Mathf.Abs(player.position.y - transform.position.y);

        // Solo moverse si está dentro del rango horizontal y vertical permitido
        if (distanciaX > detectionRadius || diferenciaY > margenY)
        {
            player = null;
            AnimacionIdle(); // <-- Cambia a idle cuando pierde al jugador
            return;
        }
        if (distanciaX < 1.2f)
        {
            AnimacionAtacando();
        }
        else
        {
            AnimacionCorriendo();
        }

        float direccionX = Mathf.Sign(player.position.x - transform.position.x);

        // Voltea el sprite según la dirección antes de mover
        spriteRenderer.flipX = direccionX < 0;

        // Movimiento solo en X, velocidad constante
        Vector2 nuevaPos = new Vector2(
            rb.position.x + direccionX * speed * Time.deltaTime,
            rb.position.y
        );

        rb.MovePosition(nuevaPos);
    }

    // --- Recibir daño ---
    public void RecibirDanio(int danio)
    {
        puntosVida -= danio;
        if (puntosVida <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AtacarJugador()
    {
        if (player != null)
        {
            Debug.Log("Atacando al jugador");
            PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("Jugador detectado, infligiendo daño");
                playerController.RecibeDanio(Vector2.zero, damage);
            }
        }
    }

    // --- Gizmo para área de detección ---
    private void OnDrawGizmos()
    {
        // Área de detección general
        Gizmos.color = Color.yellow;
        Vector3 centro = transform.position;
        Vector3 size = new Vector3(detectionRadius * 2, margenY * 2, 0.1f);
        Gizmos.DrawWireCube(centro, size);

        // Área de ataque (cuando distanciaX < 0.6f)
        Gizmos.color = Color.red;
        Vector3 ataqueCentro = transform.position;
        Vector3 ataqueSize = new Vector3(2.4f, margenY * 2, 0.1f); // 0.6 * 2 = 1.2
        Gizmos.DrawWireCube(ataqueCentro, ataqueSize);
    }
}
