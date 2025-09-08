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
        GestorTexturas.IdTextura idTextura = GestorTexturas.IdTextura.placeHolder,
        bool existeEnLocal = true
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

    public Jugador(Jugador jugador, bool boolExisteEnLocal)
    {
        new Jugador(posicion: jugador.posicion,
        velociadDeRotacion: jugador.velociadDeRotacion,
        campoDeVision: jugador.campoDeVision,
        angulo: jugador.angulo,
        velocidadDeMovimiento: jugador.velocidadDeMovimiento,
        sprite: jugador.sprite,
        idTextura: jugador.idTextura,
        existeEnLocal: jugador.existeEnLocal
        );
    }

    public void Update(float deltaTime, KeyboardState keyboardState, GamePadState gamePadState, Mapa mapa)
    {
        MoverseTeclado(deltaTime, keyboardState, mapa);
        MoverseControl(deltaTime, gamePadState, mapa);
    }

    public void MoverseTeclado(float deltaTime, KeyboardState keyboardState, Mapa mapa)
    {
        Vector2 siguientePosicion = new Vector2();
        if (keyboardState.IsKeyDown(Keys.A))
        {
            siguientePosicion += new Vector2((float)Math.Sin(angulo), -(float)Math.Cos(angulo));
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            siguientePosicion += new Vector2(-(float)Math.Sin(angulo), (float)Math.Cos(angulo));
        }
        if (keyboardState.IsKeyDown(Keys.W))
        {
            siguientePosicion += new Vector2(Convert.ToSingle(Math.Cos(angulo)), Convert.ToSingle(Math.Sin(angulo)));
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            siguientePosicion += new Vector2(Convert.ToSingle(-Math.Cos(angulo)), Convert.ToSingle(-Math.Sin(angulo)));
        }

        if (siguientePosicion != Vector2.Zero)
        {
            siguientePosicion.Normalize();
            siguientePosicion = siguientePosicion * velocidadDeMovimiento * deltaTime;
        }
        
        if (!mapa.EsPared(posicion.X + siguientePosicion.X, posicion.Y + siguientePosicion.Y))
        {
            posicion += siguientePosicion;
        }

        if (keyboardState.IsKeyDown(Keys.Left))
        {
            Rotar(deltaTime, -1);
        }

        if (keyboardState.IsKeyDown(Keys.Right))
        {
            Rotar(deltaTime);
        }
    }

    public void MoverseControl(float deltaTime, GamePadState gamePadState, Mapa mapa)
    {
        if (!gamePadState.IsConnected) { return; }

        Vector2 siguientePosicion = gamePadState.ThumbSticks.Left;
        
        if (siguientePosicion == Vector2.Zero && Math.Abs(gamePadState.ThumbSticks.Right.X) < 0.15) { return; }

        //Aplicar la fórmula de rotación 2D correcta.
        Vector2 movimientoRotado = new Vector2(
            (float)(siguientePosicion.Y * Math.Cos(angulo) - siguientePosicion.X * Math.Sin(angulo)),
            (float)(siguientePosicion.Y * Math.Sin(angulo) + siguientePosicion.X * Math.Cos(angulo))
        );

        if (movimientoRotado != Vector2.Zero)
        {
            movimientoRotado.Normalize();
            movimientoRotado = movimientoRotado * velocidadDeMovimiento * deltaTime;
        }

        if (!mapa.EsPared(posicion.X + movimientoRotado.X, posicion.Y + movimientoRotado.Y))
        {
            posicion += movimientoRotado;
        }

        if (Math.Abs(gamePadState.ThumbSticks.Right.X) >= 0.15)
        {
            Rotar(deltaTime, Math.Sign(gamePadState.ThumbSticks.Right.X), Math.Abs(gamePadState.ThumbSticks.Right.X));
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
        mensaje.Add(false);
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
            idTextura: (GestorTexturas.IdTextura)mensaje.GetInt(),
            existeEnLocal: mensaje.GetBool()
            );
    }

    public override void DeserializarObjetoParcial(Message mensaje)
    {
        posicion.X = mensaje.GetFloat();
        posicion.Y = mensaje.GetFloat();
        angulo = mensaje.GetFloat(); 
    }
    
}