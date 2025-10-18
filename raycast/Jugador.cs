using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Riptide;

public class Jugador : Entidad
{
    public float vidaActual;
    public float vidaMaxima;
    public int vecesDisparadas = 0;
    public Jugador
    (
        Vector2 posicion = new Vector2(),
        float velocidadDeRotacion = 2f,
        float campoDeVision = 100,
        float angulo = 0,
        float velocidadDeMovimiento = 4f,
        Texture2D sprite = null,
        GestorTexturas.IdTextura idTextura = GestorTexturas.IdTextura.placeHolder,
        bool existeEnLocal = true,
        bool seDibujaComoBilldoard = true,
        float vidaMaxima = 5
    ) 
    :base
    (
        posicion,
        velocidadDeRotacion,
        campoDeVision,
        angulo,
        velocidadDeMovimiento,
        sprite,
        idTextura,
        existeEnLocal: existeEnLocal,
        seDibujaComoBilldoard: seDibujaComoBilldoard
    )
    {
        this.vidaMaxima = vidaMaxima;
        this.vidaActual = this.vidaMaxima;
        this.vecesDisparadas = 0;
    }

    public Jugador(Jugador jugador, bool boolExisteEnLocal)
    : base
    (
        jugador
    )
    {
        this.existeEnLocal = boolExisteEnLocal;
        this.vidaMaxima = jugador.vidaMaxima;
        this.vidaActual = jugador.vidaActual;
    }

    public Jugador(Entidad entidad, bool boolExisteEnLocal, float vidaMaxima = 5f, float vidaActual = 5f)
    :base(
        entidad
    )
    {
        this.existeEnLocal = boolExisteEnLocal;
        this.vidaMaxima = vidaMaxima;
        this.vidaActual = vidaActual;
    }

    public override void Update(float deltaTime, KeyboardState keyboardState, GamePadState gamePadState, Mapa mapa)
    {
        MoverseTeclado(deltaTime, keyboardState, mapa);
        MoverseControl(deltaTime, gamePadState, mapa);
        AccionesTeclado(keyboardState, mapa);
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
    public void AccionesTeclado(KeyboardState keyboardState, Mapa mapa)
    {
        if(keyboardState.IsKeyDown(Keys.Space) && vecesDisparadas == 0)
        {
            Vector2 direccion = new Vector2((Single)Math.Cos(angulo), (Single)Math.Sin(angulo));
            Proyectil proyectil = new Proyectil(posicion, direccion: direccion, velocidad: 5f);
            RayCastRenderer.instancia.AñadirEntidadAListaDeEntidades(proyectil);
            mapa.listaProyectiles.Add(proyectil);
            vecesDisparadas += 1;
            
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
    public override void SerializarObjetoCompleto(Message mensaje, bool existeEnLocal = false)
    {
        base.SerializarObjetoCompleto(mensaje, existeEnLocal);
        mensaje.Add(vidaMaxima);
        mensaje.Add(vidaActual);
    }
    public override void SerializarObjetoParcial(Message mensaje)
    {
        base.SerializarObjetoParcial(mensaje);
        mensaje.Add(vidaActual);
    }

    public override Jugador DeserializarObjetoCompleto(Message mensaje)
    {
        Jugador jugadorADevolver = new Jugador(base.DeserializarObjetoCompleto(mensaje),false, mensaje.GetFloat(), mensaje.GetFloat());
        jugadorADevolver.existeEnLocal = false;
        return jugadorADevolver;
    }

    public override void DeserializarObjetoParcial(Message mensaje)
    {
        base.DeserializarObjetoParcial(mensaje);
        vidaActual = mensaje.GetFloat();
    }
    
}