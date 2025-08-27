using System;
using Microsoft.Xna.Framework;

public class Jugador
{
    public Vector2 posicion;
    public float velociadDeRotacion;
    public float campoDeVision;
    public float angulo;

    public Jugador
    (
        Vector2 posicion = new Vector2(),
        float velociadDeRotacion = 0.05f,
        float campoDeVision = 90,
        float angulo = 0
    )
    {
        if (posicion == Vector2.Zero)
        {
            this.posicion = new Vector2(2, 2);
        }

        this.velociadDeRotacion = velociadDeRotacion;
        this.campoDeVision = campoDeVision;
    }

    public void Update(float deltaTime)
    {
        Rotar(deltaTime);
    }

    public void Rotar(float deltaTime)
    {

        angulo = angulo + velociadDeRotacion * deltaTime;
        angulo %= MathF.Tau; // MathF.Tau es igual a 2π

    }
}