﻿using System;
using System.Collections.Generic;

[Serializable]
public class Database
{
    public int EnemigosMuertos = 0;
    public int EnemigosVivos = 0;
    public int vidas = 10;
    public int maxVidas = 10;
    public int coins = 0;
    public bool gameOver = false;
    public bool gameCompleted = false;
    public bool powerUpActivo = false;
    public string personajeSeleccionado = "personaje1";
    public Personaje[] personajes;
}

