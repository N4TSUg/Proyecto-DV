using System;
using UnityEngine;

[Serializable]
public class Armas
{
    public string tagArma;
    public int damage;

    public Armas(string tagArma, int damage)
    {
        this.tagArma = tagArma;
        this.damage = damage;
    }
    public GameObject CargarArma(string tagArma)
    {
        if (tagArma == null)
        {
            Debug.LogError("Datos del arma no válidos.");
            return null;
        }

        GameObject prefab = Resources.Load<GameObject>($"Weapons/prefabs/{tagArma}");
        if (prefab == null)
        {
            Debug.LogError($"No se pudo cargar el prefab de arma: {tagArma}");
            return null;
        }

        return prefab;
    }
}
