using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Riptide;

public class Jugador : Entidad
{
    public Jugador
    (
        Vector2 posicion = new Vector2(),
        float velociadDeRotacion = 2f,
        float campoDeVision = 100,
        float angulo = 0,
        float velocidadDeMovimiento = 2f,
        Texture2D sprite = null,
        GestorTexturas.IdTextura idTextura = GestorTexturas.IdTextura.placeHolder
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
        this.idTextura = idTextura;
        if (sprite == null)
        {
            sprite = GestorTexturas.ObtenerTextura(idTextura);
        }
        this.sprite = sprite;
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

    public override void SerializarObjetoCompleto(Message mensaje)
    {
        mensaje.Add(this.posicion.X);
        mensaje.Add(this.posicion.Y);
        mensaje.Add(this.velociadDeRotacion);
        mensaje.Add(this.campoDeVision);
        mensaje.Add(this.angulo);
        mensaje.Add(this.velocidadDeMovimiento);
        mensaje.Add((int)this.idTextura);
    }
    public override void SerializarObjetoParcial(Message mensaje)
    {
        mensaje.Add(this.posicion.X);
        mensaje.Add(this.posicion.Y);
        mensaje.Add(this.angulo);
    }

    public override Jugador DeserializarObjetoCompleto(Message mensaje)
    {
        return new Jugador(
            new Vector2(mensaje.GetFloat(), mensaje.GetFloat()),
            velociadDeRotacion: mensaje.GetFloat(),
            campoDeVision: mensaje.GetFloat(),
            angulo: mensaje.GetFloat(),
            velocidadDeMovimiento: mensaje.GetFloat(),
            sprite: null,
            idTextura: (GestorTexturas.IdTextura)mensaje.GetInt()
            );
    }

    public override void DeserializarObjetoParcial(Message mensaje)
    {
        posicion.X = mensaje.GetFloat();
        posicion.Y = mensaje.GetFloat();
        angulo = mensaje.GetFloat(); 
    }
    
}