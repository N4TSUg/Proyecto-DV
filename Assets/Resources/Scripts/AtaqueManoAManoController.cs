using UnityEngine;

public class AtaqueManoAManoController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] colliders = collision.gameObject.GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(tagsClass.ENEMY))
            {
                Debug.Log("Enemigo golpeado");
                collider.GetComponent<EnemieController>().RecibirDanio(1);
            }
            if (collider.CompareTag(tagsClass.BOOS))
            {
                Debug.Log("Boss golpeado");
                collider.GetComponent<ReyHeladoController>().RecibirDanio(1);
            }
        }
    }
}
