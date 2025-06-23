using System;
using UnityEngine;

[Serializable]
public class Personaje
{
    public string nombre { get; set; }
    public string tagPersonaje { get; set; }
    public Personaje(string nombre, string tagPersonaje)
    {
        this.nombre = nombre;
        this.tagPersonaje = tagPersonaje;
    }
    public void CargarPersonaje(string tagPersonaje, GameObject location)
    {
        if (tagPersonaje == null || location == null)
        {
            return;
        }

        // Eliminar cualquier hijo anterior (si solo se muestra uno a la vez)
        foreach (Transform child in location.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Cargar el prefab desde Resources
        GameObject prefab = Resources.Load<GameObject>($"Personajes/{tagPersonaje}");
        if (prefab == null)
        {
            return;
        }

        // Instanciar y posicionar
        GameObject instancia = GameObject.Instantiate(prefab, location.transform.position, location.transform.rotation);
        instancia.transform.SetParent(location.transform); // Lo hace hijo del contenedor
        instancia.transform.localPosition = Vector3.zero;  // Opcional: asegurar que esté centrado
        instancia.transform.localRotation = Quaternion.identity; // Opcional: resetear rotación local

    }
    public void CargarPersonajeVisual(string tagPersonaje, GameObject location)
    {
        if (tagPersonaje == null || location == null)
        {
            return;
        }

        // Eliminar cualquier hijo anterior
        foreach (Transform child in location.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Cargar el prefab desde Resources
        GameObject prefab = Resources.Load<GameObject>($"Personajes/{tagPersonaje}");
        if (prefab == null)
        {
            return;
        }

        // Instanciar y posicionar
        GameObject instancia = GameObject.Instantiate(prefab, location.transform.position, location.transform.rotation);
        instancia.transform.SetParent(location.transform);
        instancia.transform.localPosition = Vector3.zero;
        instancia.transform.localRotation = Quaternion.identity;

        // 🔻 Desactivar componentes de física e interacción
        // 1. Desactivar Rigidbody
        Rigidbody2D rb = instancia.GetComponentInChildren<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

        // 2. Desactivar Colliders
        Collider2D[] colliders = instancia.GetComponentsInChildren<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        // 3. Desactivar controladores (ej: PlayerController, CharacterController)
        MonoBehaviour[] scripts = instancia.GetComponentsInChildren<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script.GetType().Name.Contains("Controller")) // O más específico como == typeof(PlayerController)
            {
                script.enabled = false;
            }
        }

    }


}
