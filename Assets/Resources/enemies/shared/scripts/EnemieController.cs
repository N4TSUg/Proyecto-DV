using System.Collections;
using UnityEngine;

public class EnemieController : MonoBehaviour
{
    public int damage;
    private Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 5.0f;
    public int puntosVida = 3;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool estaAtacando = false;
    private bool puedePerseguir = false;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damage = 1;
    }


    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(tagsClass.PLAYER).transform;
            if (player == null)
            {
                return;
            }
        }
        if (estaAtacando) return;

        Vector2 direccion = player.position - transform.position;
        float distanciaHorizontal = Mathf.Abs(direccion.x);
        float diferenciaAltura = Mathf.Abs(direccion.y);

        if (distanciaHorizontal < detectionRadius && diferenciaAltura < 1f)
        {
            // Vector2 direccion = (player.position - transform.position);
            transform.localScale = new Vector3(direccion.x < 0 ? -1f : 1f, 1f, 1f);

            if (!puedePerseguir)
            {
                StartCoroutine(AnimacionAtaqueLuegoPerseguir());
            }
        }

        if (puedePerseguir)
        {
            // Vector2 direccion = (player.position - transform.position).normalized;
            // movement = new Vector2(direccion.x, 0); // solo en eje X

            direccion = direccion.normalized;
            movement = new Vector2(direccion.x, 0);
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }
    }

    private IEnumerator AnimacionAtaqueLuegoPerseguir()
    {
        estaAtacando = true;
        animator.SetBool("atack", true);
        puedePerseguir = false;

        // Esperar que termine la animación de ataque
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("atack"))
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Esperar a que termine el clip (normalizedTime >= 1)
        while (stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Termina animación
        animator.SetBool("atack", false);
        animator.SetBool("run", true);
        puedePerseguir = true;
        estaAtacando = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagsClass.PLAYER))
        {
            puntosVida--;
            if (puntosVida <= 0)
            {
                Destroy(this.gameObject);
            }
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);
            collision.gameObject.GetComponent<PlayerController>().RecibeDanio(direccionDanio, 1);
        }
    }
}
