using System.Diagnostics;
using UnityEngine;

public class EnemieController : MonoBehaviour
{
    public int damage;
    public Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 5.0f;
    public int puntosVida = 3;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool enMovimiento;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToplayer = Vector2.Distance(transform.position, player.position);
        if (distanceToplayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position);

            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-0.6f, 0.6f, 1);
            }
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(0.6f, 0.6f, 1);
            }

            movement = new Vector2(direction.x, 0);
            enMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        animator.SetBool("enMovimiento", enMovimiento);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);
            collision.gameObject.GetComponent<PlayerController>().RecibeDanio(direccionDanio, 1);
        }
    }
}
