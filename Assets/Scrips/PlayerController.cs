using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    private bool puedeSaltar = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Iniciando PlayerController");

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SetupMoverseHorizontal();
        SetupSalto();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemie"))
        {
            EnemieController enemie = collision.gameObject.GetComponent<EnemieController>();
            Debug.Log($"Colision con Enemigo: {enemie.puntosVida}");
            Destroy(collision.gameObject);
        }
    }

    void SetupMoverseHorizontal()
    {
        rb.linearVelocityX = 0;
        animator.SetInteger("Estado", 0);

        if (Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D))
        {
            rb.linearVelocityX = 10;
            sr.flipX = false;
            animator.SetInteger("Estado", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.A))
        {
            rb.linearVelocityX = -10;
            sr.flipX = true;
            animator.SetInteger("Estado", 1);
        }
    }

    void SetupSalto()
    {
        if (!puedeSaltar) return;
        if (Input.GetKeyUp(KeyCode.Space)|| Input.GetKey(KeyCode.W))
        {
            rb.linearVelocityY = 12.5f;
            animator.SetInteger("Estado", 2);
        }
    }
}
