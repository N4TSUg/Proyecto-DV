using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Kits
{
    public string nombre;
    public Armas[] armas;
    public Kits(string nombre, Armas[] armas)
    {
        this.nombre = nombre;
        this.armas = armas;
    }
}
