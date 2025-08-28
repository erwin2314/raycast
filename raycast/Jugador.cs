using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Jugador
{
    public Vector2 posicion;
    public float velociadDeRotacion;
    public float campoDeVision;
    public float angulo;
    public float velocidadDeMovimiento;

    public Jugador
    (
        Vector2 posicion = new Vector2(),
        float velociadDeRotacion = 1f,
        float campoDeVision = 90,
        float angulo = 0,
        float velocidadDeMovimiento = 1
    )
    {
        if (posicion == Vector2.Zero)
        {
            this.posicion = new Vector2(2, 2);
        }

        this.velociadDeRotacion = velociadDeRotacion;
        this.campoDeVision = MathHelper.ToRadians(campoDeVision);
        this.angulo = MathHelper.ToRadians(angulo);
        this.velocidadDeMovimiento = velocidadDeMovimiento;
    }

    public void Update(float deltaTime, KeyboardState keyboardState, Mapa mapa)
    {
        if (keyboardState.IsKeyDown(Keys.D))
        {
            Rotar(deltaTime, 1);
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            Rotar(deltaTime, -1);
        }
        if (keyboardState.IsKeyDown(Keys.W))
        {
            Vector2 siguientePosicion = posicion + (velocidadDeMovimiento * new Vector2(Convert.ToSingle(Math.Cos(angulo)), Convert.ToSingle(Math.Sin(angulo))) * deltaTime);
            if (!mapa.EsPared(siguientePosicion.X, siguientePosicion.Y))
            {
                posicion = siguientePosicion;
            }
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            Vector2 siguientePosicion = posicion + (velocidadDeMovimiento * new Vector2(Convert.ToSingle(-Math.Cos(angulo)), Convert.ToSingle(-Math.Sin(angulo))) * deltaTime);
            if (!mapa.EsPared(siguientePosicion.X, siguientePosicion.Y))
            {
                posicion = siguientePosicion;
            }
        }
        
    }

    public void Rotar(float deltaTime, int rotarDerecha = 1) //1 = derecha, -1 = izquierda
    {
        angulo = angulo + (rotarDerecha * velociadDeRotacion) * deltaTime;
        angulo %= MathF.Tau; // MathF.Tau es igual a 2π

    }
}