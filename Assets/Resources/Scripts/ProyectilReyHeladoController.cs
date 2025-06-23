using UnityEngine;

public class ProyectilReyHeladoController : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    SpriteRenderer _spriteRenderer;
    public int _damage = 1;
    public float velocidad = 15f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        Destroy(this.gameObject, 5f);
    }

    // Llama a este método justo después de instanciar el proyectil
    public void LanzarProyectil(Vector2 destino)
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody2D>();
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 direccion = (destino - (Vector2)transform.position).normalized;
        _rigidbody.linearVelocity = direccion * velocidad;

        if (_spriteRenderer != null)
            _spriteRenderer.flipX = direccion.x < 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(tagsClass.PLAYER))
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
                player.RecibeDanio((Vector2)transform.position, _damage);
            Destroy(this.gameObject);
        }
    }
}
