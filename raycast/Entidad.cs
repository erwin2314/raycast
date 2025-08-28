using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public class Entidad
{
    public Vector2 posicion;
    public float velociadDeRotacion;
    public float campoDeVision;
    public float angulo;
    public float velocidadDeMovimiento;
    public Texture2D sprite;

    public Entidad
    (
        Vector2 posicion = new Vector2(),
        float velociadDeRotacion = 1f,
        float campoDeVision = 90,
        float angulo = 0,
        float velocidadDeMovimiento = 1,
        Texture2D sprite = null
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
        this.sprite = sprite;
    }

    public void Rotar(float deltaTime, int rotarDerecha = 1) //1 = derecha, -1 = izquierda
    {
        angulo = angulo + (rotarDerecha * velociadDeRotacion) * deltaTime;
        angulo %= MathF.Tau; // MathF.Tau es igual a 2π
    }
}