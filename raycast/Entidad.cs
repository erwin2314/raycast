using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    public float anchoSprite;
    public float alturaSprite;
    public float distanciaAJugador;

    public Entidad
    (
        Vector2 posicion = new Vector2(),
        float velociadDeRotacion = 1f,
        float campoDeVision = 90,
        float angulo = 0,
        float velocidadDeMovimiento = 1,
        Texture2D sprite = null,
        float anchoSprite = 256,
        float alturaSprite = 256,
        float distanciaAJugador = 0
    )
    {
        if (posicion == Vector2.Zero)
        {
            this.posicion = new Vector2(2, 2);
        }
        else
        {
            this.posicion = posicion;
        }

        this.velociadDeRotacion = velociadDeRotacion;
        this.campoDeVision = MathHelper.ToRadians(campoDeVision);
        this.angulo = MathHelper.ToRadians(angulo);
        this.velocidadDeMovimiento = velocidadDeMovimiento;

        this.sprite = sprite;
        this.anchoSprite = anchoSprite;
        this.alturaSprite = alturaSprite;
        this.distanciaAJugador = distanciaAJugador;
    }

    public Entidad
    (
        Entidad entidad
    )
    {
        if (entidad.posicion == Vector2.Zero)
        {
            this.posicion = new Vector2(2, 2);
        }
        else
        {
            this.posicion = entidad.posicion;
        }

        this.velociadDeRotacion = entidad.velociadDeRotacion;
        this.campoDeVision = MathHelper.ToRadians(entidad.campoDeVision);
        this.angulo = MathHelper.ToRadians(entidad.angulo);
        this.velocidadDeMovimiento = entidad.velocidadDeMovimiento;

        this.sprite = entidad.sprite;
        this.anchoSprite = entidad.anchoSprite;
        this.alturaSprite = entidad.alturaSprite;
        this.distanciaAJugador = entidad.distanciaAJugador;
    }

    public void Rotar(float deltaTime, int rotarDerecha = 1) //1 = derecha, -1 = izquierda
    {
        angulo = angulo + (rotarDerecha * velociadDeRotacion) * deltaTime;
        angulo %= MathF.Tau; // MathF.Tau es igual a 2π
    }
}